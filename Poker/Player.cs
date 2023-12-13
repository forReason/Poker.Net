using Poker.Decks;

namespace Poker;

public class Player
{
    // TODO: Not Yet Implemented
    public int Bank { get; private set; }
    public Hand PlayerHand = new Hand();

    /// <summary>
    /// Discards the players Hand, essentially skipping the Round
    /// </summary>
    public void DiscardHand()
    {
        PlayerHand.Clear();
    }
}