using System.Collections;
using System.Text.Json;
using Poker.Net.PhysicalObjects.Cards;

namespace Poker.Net.PhysicalObjects.HandScores;

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

        BitArray cardBits = new BitArray(42, false);
        for (int i = 0, bitIndex = 0; i < Cards.Length && i < 7; i++)
        {
            byte serializedCard = Cards[i] != null ? Cards[i].SerializeToByte() : (byte)0;
            BitArray cardBitArray = new BitArray(new byte[] { serializedCard });
            for (int j = 0; j < 6; j++, bitIndex++)
            {
                cardBits[bitIndex] = cardBitArray[j];
            }
        }
        cardBits.CopyTo(serialized, 0);

        Buffer.BlockCopy(BitConverter.GetBytes(WinRate), 0, serialized, 6, 4);
        Buffer.BlockCopy(BitConverter.GetBytes(EvaluatedRounds), 0, serialized, 10, 4);

        return serialized;
    }



    public byte[] SerializeCards()
    {
        byte[] serialized = new byte[6]; // 6 bytes for cards + 4 bytes for WinRate + 4 bytes for EvaluatedRounds

        BitArray cardBits = new BitArray(42, false);
        for (int i = 0, bitIndex = 0; i < Cards.Length && i < 7; i++)
        {
            byte serializedCard = Cards[i] != null ? Cards[i].SerializeToByte() : (byte)0;
            BitArray cardBitArray = new BitArray(new byte[] { serializedCard });
            for (int j = 0; j < 6; j++, bitIndex++)
            {
                cardBits[bitIndex] = cardBitArray[j];
            }
        }
        cardBits.CopyTo(serialized, 0);
        return serialized;
    }

    /// <summary>
    /// Deserializes a byte array to a HandScoreEntry object.
    /// </summary>
    /// <param name="data">The byte array to deserialize.</param>
    /// <returns>The deserialized HandScoreEntry object.</returns>
    public static HandScoreEntry Deserialize(byte[] data)
    {
        if (data.Length != 14)
        {
            throw new ArgumentException("Data must be 14 bytes long.", nameof(data));
        }

        ;
        List<Card> cards = new List<Card>();
        BitArray cardBits = new BitArray(data.Take(6).ToArray());
        for (int i = 0, bitIndex = 0; i < 7; i++)
        {
            byte[] cardByte = new byte[1];
            for (int j = 0; j < 6; j++, bitIndex++)
            {
                cardByte[0] |= (byte)(cardBits[bitIndex] ? (1 << j) : 0);
            }

            if (cardByte[0] == 0) // No more cards
            {
                break;
            }

            cards.Add(Card.DeserializeFromByte(cardByte[0]));
        }
        HandScoreEntry entry = new HandScoreEntry
        {
            Cards = cards.ToArray(),
            WinRate = BitConverter.ToSingle(data, 6),
            EvaluatedRounds = BitConverter.ToUInt32(data, 10)
        };

        return entry;
    }



}