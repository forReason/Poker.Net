using System.Collections;
using System.Text.Json;
using Poker.PhysicalObjects.Cards;

namespace Poker.PhysicalObjects.HandScores;

public class HandScoreEntry
{
    public Card[] Cards;
    public float WinRate;
    public uint EvaluatedRounds;
    /// <summary>
    /// parameterless constructor for serialisation
    /// </summary>
    public HandScoreEntry() { }
    public HandScoreEntry(Card[] cards) { Cards = cards; }
    public HandScoreEntry(Card[] handCards, Card[] communityCards)
    {
        handCards = handCards.Where(c => c != null).ToArray();
        if (handCards.Length < 2)
            throw new Exception("You can only evaluate your hand with cards in your hand!");
        communityCards = communityCards.Where(c => c != null).ToArray();
        CardSuit?[] cardMap = new CardSuit?[4];
        ulong foundCards = 0;
        // sort hand and community cards by rank descending
        InsertionSort(handCards);
        InsertionSort(communityCards);

        Cards = new Card[handCards.Length + communityCards.Length];
        int suitIndexOfCardToCheck;
        CardSuit suitTranslationOfCardToCheck;
        // hand cards
        for (int i = 0; i < handCards.Length; i++)
        {
            suitIndexOfCardToCheck = (int)handCards[i].Suit;
            // update CardMap if necessary
            if (cardMap[suitIndexOfCardToCheck] is null)
            {
                suitTranslationOfCardToCheck = (CardSuit)foundCards;
                cardMap[suitIndexOfCardToCheck] = suitTranslationOfCardToCheck;
                foundCards++;
            }
            else
            {
                suitTranslationOfCardToCheck = cardMap[suitIndexOfCardToCheck].Value;
            }
            // set Card
            Cards[i] = Card.GetCard(handCards[i].CardRank, suitTranslationOfCardToCheck);
        }
        // community cards
        for (int i = 0; i < communityCards.Length; i++)
        {
            if (communityCards[i] is null) continue;
            suitIndexOfCardToCheck = (int)communityCards[i].Suit;
            // update CardMap if necessary
            if (cardMap[suitIndexOfCardToCheck] is null)
            {
                suitTranslationOfCardToCheck = (CardSuit)foundCards;
                cardMap[suitIndexOfCardToCheck] = suitTranslationOfCardToCheck;
                foundCards++;
            }
            else
            {
                suitTranslationOfCardToCheck = cardMap[suitIndexOfCardToCheck].Value;
            }
            // set Card
            Cards[i + handCards.Length] = Card.GetCard(communityCards[i].CardRank, suitTranslationOfCardToCheck);
        }
    }
    public override int GetHashCode()
    {
        byte[] cardBytes = new byte[6]; // 6 bytes for 7 cards (42 bits)

        // Serialize cards to 6 bytes
        BitArray cardBits = new BitArray(42);
        for (int i = 0, bitIndex = 0; i < 7; i++)
        {
            if (Cards[i] == null)
                continue;

            BitArray cardBitArray = Cards[i].SerializeToBitArray();
            for (int j = 0; j < 6; j++, bitIndex++)
            {
                cardBits[bitIndex] = cardBitArray[j];
            }
        }
        cardBits.CopyTo(cardBytes, 0);

        // Use the byte array to calculate the hash code
        int hash = 17;
        foreach (var b in cardBytes)
        {
            hash = hash * 31 + b;
        }
        return hash;
    }
    public static byte[] ExtractCardBytesFromSerialized(byte[] serializedEntry)
    {
        if (serializedEntry == null || serializedEntry.Length < 6)
        {
            throw new ArgumentException("Serialized entry is not valid or too short.", nameof(serializedEntry));
        }

        byte[] cardBytes = new byte[6];
        Array.Copy(serializedEntry, 0, cardBytes, 0, 6);
        return cardBytes;
    }
    public static (byte[] Cards, float WinRate, uint EvaluatedRounds) ExtractRawDataFromSerialized(byte[] serializedEntry)
    {
        if (serializedEntry == null || serializedEntry.Length != 14)
        {
            throw new ArgumentException("Serialized entry is not valid or does not have the expected length.", nameof(serializedEntry));
        }

        byte[] cardBytes = new byte[6];
        Array.Copy(serializedEntry, 0, cardBytes, 0, 6);

        float winRate = BitConverter.ToSingle(serializedEntry, 6);
        uint evaluatedRounds = BitConverter.ToUInt32(serializedEntry, 10);

        return (cardBytes, winRate, evaluatedRounds);
    }


    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var other = (HandScoreEntry)obj;

        // Compare all relevant fields here. For example:
        return Cards.SequenceEqual(other.Cards);
    }

    private void InsertionSort(Card[] cards)
    {
        for (int i = 1; i < cards.Length; i++)
        {
            Card? temp = cards[i];
            int j = i - 1;
            while (j >= 0 && cards[j] < temp)
            {
                cards[j + 1] = cards[j];
                j--;
            }
            cards[j + 1] = temp;
        }
    }
    public static string SerializeHandScoreEntry(HandScoreEntry entry)
    {
        return JsonSerializer.Serialize(entry);
    }

    public static HandScoreEntry DeserializeHandScoreEntry(string entryString)
    {
        return JsonSerializer.Deserialize<HandScoreEntry>(entryString);
    }
    /// <summary>
    /// Serializes the HandScoreEntry to a byte array.
    /// </summary>
    /// <returns>A byte array representing the serialized HandScoreEntry.</returns>
    public byte[] Serialize()
    {
        byte[] serialized = new byte[14]; // 6 bytes for cards + 4 bytes for WinRate + 4 bytes for EvaluatedRounds

        // Serialize cards to 6 bytes (7 cards * 6 bits each = 42 bits)
        BitArray cardBits = new BitArray(42);
        for (int i = 0, bitIndex = 0; i < 7; i++)
        {
            BitArray cardBitArray = Cards[i].SerializeToBitArray();
            for (int j = 0; j < 6; j++, bitIndex++)
            {
                cardBits[bitIndex] = cardBitArray[j];
            }
        }
        cardBits.CopyTo(serialized, 0);

        // Serialize WinRate (4 bytes)
        Buffer.BlockCopy(BitConverter.GetBytes(WinRate), 0, serialized, 6, 4);

        // Serialize EvaluatedRounds (4 bytes)
        Buffer.BlockCopy(BitConverter.GetBytes(EvaluatedRounds), 0, serialized, 10, 4);

        return serialized;
    }

    public byte[] SerializeCards()
    {
        byte[] cardBytes = new byte[6]; // 6 bytes for 7 cards (42 bits)

        // Serialize cards to 6 bytes
        BitArray cardBits = new BitArray(42);
        for (int i = 0, bitIndex = 0; i < 7; i++)
        {
            if (Cards[i] == null)
                continue;

            BitArray cardBitArray = Cards[i].SerializeToBitArray();
            for (int j = 0; j < 6; j++, bitIndex++)
            {
                cardBits[bitIndex] = cardBitArray[j];
            }
        }
        cardBits.CopyTo(cardBytes, 0);
        return cardBytes;
    }

    /// <summary>
    /// Deserializes a byte array to a HandScoreEntry object.
    /// </summary>
    /// <param name="data">The byte array to deserialize.</param>
    /// <returns>The deserialized HandScoreEntry object.</returns>
    public static HandScoreEntry Deserialize(byte[] data)
    {
        if (data.Length != 14) // 6 bytes for cards + 8 bytes for WinRate and EvaluatedRounds
        {
            throw new ArgumentException("Data must be 14 bytes long.", nameof(data));
        }

        HandScoreEntry entry = new HandScoreEntry
        {
            Cards = new Card[7],
            WinRate = BitConverter.ToSingle(data, 6),
            EvaluatedRounds = BitConverter.ToUInt32(data, 10)
        };

        // Deserialize cards from first 6 bytes
        BitArray cardBits = new BitArray(data.Take(6).ToArray()); // Takes the first 6 bytes and converts to BitArray

        for (int i = 0, bitIndex = 0; i < 7; i++)
        {
            BitArray cardBitArray = new BitArray(6);
            for (int j = 0; j < 6; j++, bitIndex++)
            {
                cardBitArray[j] = cardBits[bitIndex];
            }
            entry.Cards[i] = Card.DeserializeFromBitArray(cardBitArray);
        }

        return entry;
    }

}