using Poker.Net.PhysicalObjects.Cards;

namespace Poker.Net.PhysicalObjects.Decks;

/// <summary>
/// represents a players hand with a maximum of 2 Cards
/// </summary>
public class PocketCards
{
    public PocketCards() { }
    /// <summary>
    /// override method to set a new hand for testing purposes only. <br/>
    /// When playing or simulating a Game, you should draw from deck with <see cref="DealCard"/> appropriately
    /// </summary>
    public PocketCards(Card? card1, Card? card2)
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
    /// the 5 Slots on the Table
    /// </summary>
    private readonly Card?[] _cards = new Card?[2];

    /// <summary>
    /// Returns a shallow Copy of the current Slots, preventing external modifications
    /// </summary>
    public Card?[] Cards => (Card?[])_cards.Clone();

    /// <summary>
    /// the current amount of cards in the player pocket
    /// </summary>
    public int CardCount { get; private set; }
    /// <summary>
    /// returns true if the player has 2 hands
    /// </summary>
    public bool HandIsFull => _cards[1] is null;
    /// <summary>
    /// checks if the player has at least 1 card
    /// </summary>
    public bool HasCards => CardCount > 0;

    /// <summary>
    /// Draws the Next Card
    /// </summary>
    /// <param name="deck"></param>
    /// <exception cref="InvalidOperationException">if you try to draw more than two cards</exception>
    public void DealCard(Deck deck)
    {
        _cards[CardCount++] = deck.DrawCard();

    }


    /// <summary>
    /// clears the Slots
    /// </summary>
    public void Clear()
    {
        _cards[0] = null;
        _cards[1] = null;
        CardCount = 0;
    }

}