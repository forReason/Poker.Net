using Poker.Cards;

namespace Poker.Decks;

/// <summary>
/// represents a players hand with a maximum of 2 Cards
/// </summary>
public class Hand
{
    // <summary>
    /// the 5 Slots on the Table
    /// </summary>
    private Card?[] _Slots = new Card?[2];

    /// <summary>
    /// Returns a shallow Copy of the current Slots, preventing external modifications
    /// </summary>
    public Card?[] Slots => (Card?[])_Slots.Clone()!;

    public int CardCount { get; private set; } = 0;

    /// <summary>
    /// override method to set a new hand. When simulating a Game, you should draw apropriately
    /// </summary>
    public void SetHand(Card card1, Card card2)
    {
        _Slots[0] = card1;
        _Slots[1] = card2;
    }

    /// <summary>
    /// Draws the Next Card
    /// </summary>
    /// <param name="deck"></param>
    /// <exception cref="InvalidOperationException">if you try to draw more than two cards</exception>
    public void DrawCard(Deck deck)
    {
        if (CardCount >= 2)
            throw new InvalidOperationException("You Cannot draw more than two Cards!");
        _Slots[CardCount] = deck.DrawCard();
        CardCount++;
    }
    

    /// <summary>
    /// clears the Slots
    /// </summary>
    public void Clear()
    {
        for (int i = 0; i < _Slots.Length; i++)
        {
            _Slots[i] = null;
        }
        CardCount = 0;
    }
}