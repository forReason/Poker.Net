using Poker.Cards;

namespace Poker.Decks;

/// <summary>
/// Represents the Raw Deck with all 52 Cards. Primarily used for shuffling and drawing
/// </summary>
public class Deck
{
    /// <summary>
    /// Initializes the Deck with the 42 Poker Cards
    /// </summary>
    public Deck()
    {
        GenerateDeck();
    }

    /// <summary>
    /// The count of cards left in the Deck which are available to be drawn
    /// </summary>
    public int _CardCount { get; private set; } = 0;

    /// <summary>
    /// All 52 Cards in a Deck
    /// </summary>
    private Cards.Card[] _ShuffledCards = new Cards.Card[52];

    /// <summary>
    /// Draws a new Card from the Shuffled Deck
    /// </summary>
    /// <returns>The Drawn Card</returns>
    public Cards.Card DrawCard()
    {
        if (_CardCount <= 0)
            throw new InvalidOperationException("You can not draw more cards than the Deck has in Total! Please reshuffle!");
        Cards.Card selectedCard = _ShuffledCards[_CardCount-1];
        _CardCount--;
        return selectedCard;
    }
    
    /// <summary>
    /// Shuffles all the cards in the Deck
    /// </summary>
    /// <summary>
    /// Shuffles all the cards in the Deck
    /// </summary>
    public void ShuffleCards()
    {
        Random rng = new Random();
        int n = _ShuffledCards.Length;
        Cards.Card tempCache;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            tempCache = _ShuffledCards[k];
            _ShuffledCards[k] = _ShuffledCards[n];
            _ShuffledCards[n] = tempCache;
        }

        _CardCount = _ShuffledCards.Length;
    }

    /// <summary>
    /// Generates the Deck of 52 Cards, one of each rank and suit combination
    /// </summary>
    private void GenerateDeck()
    {
        int i = 0;
        foreach (Rank rank in Enum.GetValuesAsUnderlyingType<Rank>())
        {
            foreach (Suit suit in Enum.GetValuesAsUnderlyingType<Suit>())
            {
                _ShuffledCards[i] = new Cards.Card(rank, suit);
                i++;
            }
        }

        ShuffleCards();
    }
}