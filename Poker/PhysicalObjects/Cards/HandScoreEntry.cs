using System.Text.Json;

namespace Poker.PhysicalObjects.Cards;

public class HandScoreEntry
{
    public Card[] Cards;
    public float WinRate;
    public uint EvaluatedRounds;
    /// <summary>
    /// parameterless constructor for serialisation
    /// </summary>
    public HandScoreEntry() { }
    public HandScoreEntry(Card[] cards) { this.Cards = cards; }
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
            Cards[i+handCards.Length] = Card.GetCard(communityCards[i].CardRank, suitTranslationOfCardToCheck);
        }
    }
    public override int GetHashCode()
    {
        int hash = 17;

        foreach (var card in Cards)
        {
            if (card is null)
                continue;
            int cardHash = card.GetHashCode(); // Ensure Card's GetHashCode is implemented
            hash = hash * 31 + cardHash; // Combines the hash of each card
        }
        
        return hash;
    }
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var other = (HandScoreEntry)obj;
        
        // Compare all relevant fields here. For example:
        return this.Cards.SequenceEqual(other.Cards);
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
}