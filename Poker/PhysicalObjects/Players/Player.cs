using Poker.Decks;
using Poker.Tables;
using System.Threading;

namespace Poker.Players;

public class Player
{
    private decimal _bank;
    private readonly object _bankLock = new object();
    public string UniqueIdentifier = Guid.NewGuid().ToString();
    public Seat? Seat = null;

    public decimal Bank
    {
        get
        {
            lock (_bankLock)
            {
                return _bank;
            }
        }
        private set
        {
            lock (_bankLock)
            {
                _bank = value;
            }
        }
    }

    /// <summary>
    /// Adds value to the player's bank.
    /// </summary>
    /// <param name="amount">Amount to add.</param>
    public void AddPlayerBank(decimal amount)
    {
        lock (_bankLock)
        {
            _bank += amount;
        }
    }

    /// <summary>
    /// Tries to remove a specified amount from the player's bank.
    /// </summary>
    /// <param name="amount">Amount to remove.</param>
    /// <returns>True if removal was successful, false otherwise.</returns>
    public bool TryRemovePlayerBank(decimal amount)
    {
        lock (_bankLock)
        {
            if (_bank >= amount)
            {
                _bank -= amount;
                return true;
            }
            return false;
        }
    }
    public void CallForAction(ulong callValue, ulong minRaise, ulong maxRaise)
    {
        // TODO: implement playerAction
        throw new NotImplementedException("player action mechanic is not yet implemented");
    }
}