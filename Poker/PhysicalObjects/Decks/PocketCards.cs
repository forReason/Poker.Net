using Poker.PhysicalObjects.Cards;

namespace Poker.PhysicalObjects.Decks;

/// <summary>
/// represents a players hand with a maximum of 2 Cards
/// </summary>
public class PocketCards
{
    // <summary>
    /// the 5 Slots on the Table
    /// </summary>
    private Card?[] _cards = new Card?[2];

    /// <summary>
    /// Returns a shallow Copy of the current Slots, preventing external modifications
    /// </summary>
    public Card?[] Cards => (Card?[])_cards.Clone()!;

    /// <summary>
    /// the current amount of cards in the player pocket
    /// </summary>
    public int CardCount { get; private set; } = 0;
    /// <summary>
    /// returns true if the player has 2 hands
    /// </summary>
    public bool HandIsFull => CardCount >= 2;
    /// <summary>
    /// checks if the player has at least 1 card
    /// </summary>
    public bool HasCards => CardCount > 0;
    /// <summary>
    /// checks if the pleayer has no cards (fold)
    /// </summary>
    public bool IsFold => CardCount == 0;

    /// <summary>
    /// override method to set a new hand. When simulating a Game, you should draw apropriately
    /// </summary>
    public void SetHand(Card? card1, Card? card2)
    {
        int count = 0;
        _cards[0] = card1;
        if (card1 != null)
            count++;
        _cards[1] = card2;
        if (card2 != null)
            count++;
        CardCount = count;
    }

    /// <summary>
    /// Draws the Next Card
    /// </summary>
    /// <param name="deck"></param>
    /// <exception cref="InvalidOperationException">if you try to draw more than two cards</exception>
    public void DealCard(Deck deck)
    {
        if (HandIsFull)
            throw new InvalidOperationException("You Cannot draw more than two Cards!");
        _cards[CardCount] = deck.DrawCard();
        CardCount++;
    }
    

    /// <summary>
    /// clears the Slots
    /// </summary>
    public void Clear()
    {
        for (int i = 0; i < _cards.Length; i++)
        {
            _cards[i] = null;
        }
        CardCount = 0;
    }
}