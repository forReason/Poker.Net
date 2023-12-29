using Poker.Decks;
using Poker.Tables;

namespace Poker.Players;

public class Player
{
    // TODO: Not Yet Implemented
    
    public ulong Bank = 0;
    public string UniqueIdentifier = new Guid().ToString();

    

    public Seat? Seat = null;

    /// <summary>
    /// adds value to the player bank
    /// </summary>
    /// <param name="amount"></param>
    public void AddPlayerBank(ulong amount)
    {
        Interlocked.Add(ref Bank, amount);
    }
    /// <summary>
    /// adds value to the player bank
    /// </summary>
    /// <param name="amount"></param>
    public bool TryRemovePlayerBank(ulong amount)
    {
        while (true)
        {
            // Capture the original value
            ulong original = Bank;

            // Check if there are enough funds
            if (original < amount)
                return false;  // Not enough funds

            // Compute the new value
            ulong newValue = original - amount;

            // Atomically update if the original value has not changed
            if (Interlocked.CompareExchange(ref Bank, newValue, original) == original)
                return true;  // Successfully updated
        }
    }

    /// <summary>
    /// Discards the players Hand, essentially skipping the Round
    /// </summary>
    public void Fold()
    {
        this.Seat.PlayerHand.Clear();
    }
}