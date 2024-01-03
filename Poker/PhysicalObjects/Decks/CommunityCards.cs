using Poker.PhysicalObjects.Cards;

namespace Poker.PhysicalObjects.Decks;

/// <summary>
/// Manages the community cards (shared cards) on the table in a poker game.
/// This includes handling of both the visible community cards and the burn cards.
/// </summary>
public class CommunityCards
{
    /// <summary>
    /// the 5 Slots on the Table
    /// </summary>
    private readonly Card?[] _tableCards = new Card?[5];

    /// <summary>
    /// Gets a shallow copy of the current community cards on the table.
    /// This prevents external modification of the cards array.
    /// </summary>
    public Card?[] TableCards => (Card?[])_tableCards.Clone();
    
    /// <summary>
    /// the burn cards which are put before each stage reveal
    /// </summary>
    private readonly Card?[] _burnCards = new Card?[3];
    
    /// <summary>
    /// Gets a shallow copy of the burn cards used during the game.
    /// Burn cards are discarded unseen to prevent any unfair advantage.
    /// </summary>
    public Card?[] BurnCards => (Card?[])_burnCards.Clone();
    
    /// <summary>
    /// the current Stage we are in
    /// </summary>
    public CommunityCardStage Stage { get; private set; }

    /// <summary>
    /// Sets custom community cards for the game. This method is primarily used for testing or specific game scenarios.
    /// </summary>
    /// <param name="cards">The array of cards to set as community cards, ordered from left to right.</param>
    /// <exception cref="InvalidOperationException">Thrown if the array length is not within 3 to 5 cards.</exception>
    public void Set(Card[] cards)
    {
        if (cards.Length > 5)
            throw new InvalidOperationException("You cannot set more than 5 Cards!");
        if (cards.Length < 3)
            throw new InvalidOperationException("The Minimum Amount of Cards is 3! Use Clea() to empty!");
        Clear();
        for (int i = 0; i < cards.Length; i++)
        {
            _tableCards[i] = cards[i];
        }

        Stage = (CommunityCardStage)(cards.Length - 2);
    }
    
    /// <summary>
    /// Advances the game to the next stage by revealing community cards and burning one card.
    /// In the PreFlop stage, three community cards are drawn. In subsequent stages, one additional card is drawn.
    /// </summary>
    /// <param name="deck">The deck from which cards are drawn.</param>
    /// <exception cref="InvalidOperationException">Thrown if attempting to reveal cards when all have already been revealed.</exception>
    public void OpenNextStage(Deck deck)
    {
        if (Stage >= CommunityCardStage.River)
            throw new InvalidOperationException("All Cards are Revealed! You cannot reveal more Cards");
        _burnCards[(ulong)Stage] = deck.DrawCard();
        if (Stage == CommunityCardStage.PreFlop)
        {
            for (int i = 0; i < 3; i++)
            {
                _tableCards[i] = deck.DrawCard();
            }
        }
        else
        {
            _tableCards[2 + (int)Stage] = deck.DrawCard();
        }
        Stage++;
    }

    /// <summary>
    /// Reveals all remaining community cards until the River stage is reached.
    /// This method consecutively calls OpenNextStage until all community cards are revealed.
    /// </summary>
    /// <param name="deck">The deck from which cards are drawn.</param>
    public void RevealAll(Deck deck)
    {
        while (Stage < CommunityCardStage.River)
        {
            OpenNextStage(deck);
        }
    }

    /// <summary>
    /// Clears all community and burn cards from the table and resets the game stage.
    /// This method is used for resetting the community cards at the end of a hand or when starting a new game.
    /// </summary>
    public void Clear()
    {
        Array.Clear(_tableCards, 0, _tableCards.Length);
        Array.Clear(_burnCards, 0 , _burnCards.Length);
        Stage = 0;
    }
}