using Poker.Decks;
using Poker.Tables;

namespace Poker.Players;

public class Player
{
    // TODO: Not Yet Implemented
    
    public ulong Bank = 0;
    public string UniqueIdentifier = new Guid().ToString();

    /// <summary>
    /// The hand of Cards of the player
    /// </summary>
    public Hand PlayerHand = new Hand();

    public Seat? Seat = null;

    /// <summary>
    /// adds valne to the player bank
    /// </summary>
    /// <param name="amount"></param>
    public void AddPlayerBank(ulong amount)
    {
        Interlocked.Add(ref Bank, amount);
        Bank += amount;
    }

    /// <summary>
    /// Discards the players Hand, essentially skipping the Round
    /// </summary>
    public void Fold()
    {
        PlayerHand.Clear();
    }
}