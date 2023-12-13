using Poker.Cards;

namespace Poker.Decks;

/// <summary>
/// The SharedCards are the Middle Cards of the Table
/// </summary>
public class CommunityCards
{
    /// <summary>
    /// the 5 Slots on the Table
    /// </summary>
    private Card?[] _Slots = new Card?[5];

    /// <summary>
    /// Returns a shallow Copy of the current Slots, preventing external modifications
    /// </summary>
    public Card?[] Slots => (Card?[])_Slots.Clone()!;

    /// <summary>
    /// the current Stage we are in
    /// </summary>
    public CommunityCardStage Stage { get; private set; }

    /// <summary>
    /// Override method for setting custom cards. When you host a Game, you should use OpenNextStage() instead.
    /// </summary>
    /// <param name="cards">the cards to set, from left to right</param>
    /// <exception cref="InvalidOperationException">
    /// you either set less than 3 or more than 5 cards or the array contains null elements</exception>
    public void Set(Card[] cards)
    {
        if (cards.Length > 5)
            throw new InvalidOperationException("You cannot set more than 5 Cards!");
        if (cards.Length < 3)
            throw new InvalidOperationException("The Minimum Amount of Cards is 3! Use Clea() to empty!");
        Clear();
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i] == null)
                throw new InvalidOperationException("Cannot Assign an empty slot. Please assign cards from left to Right");
            _Slots[i] = cards[i];
        }

        Stage = (CommunityCardStage)(cards.Length - 2);
    }
    
    /// <summary>
    /// Opens the next stage and reveals more Cards
    /// </summary>
    /// <param name="deck"></param>
    /// <exception cref="InvalidOperationException">if you try to reveal cards when all cards are already revealed</exception>
    public void OpenNextStage(Deck deck)
    {
        if (Stage >= CommunityCardStage.River)
            throw new InvalidOperationException("All Cards are Revealed! You cannot reveal more Cards");
        if (Stage == CommunityCardStage.PreFlop)
        {
            for (int i = 0; i < 3; i++)
            {
                _Slots[i] = deck.DrawCard();
            }
        }
        else
        {
            _Slots[2 + (int)Stage] = deck.DrawCard();
        }
        Stage++;
    }

    /// <summary>
    /// Reveals all cards
    /// </summary>
    /// <param name="deck"></param>
    public void RevealAll(Deck deck)
    {
        while (Stage < CommunityCardStage.River)
        {
            OpenNextStage(deck);
        }
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

        Stage = 0;
    }
}