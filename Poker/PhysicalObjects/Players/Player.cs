using Poker.PhysicalObjects.Tables;

namespace Poker.PhysicalObjects.Players;

/// <summary>
/// represents a player character
/// </summary>
public class Player
{
    private decimal _bank;
    private readonly object _bankLock = new object();
    /// <summary>
    /// the unique identifier of the player. In online games, this might be the name or an #ID
    /// </summary>
    public string UniqueIdentifier = Guid.NewGuid().ToString();
    /// <summary>
    /// the seat on a table game which this player is part of
    /// </summary>
    public Seat? Seat = null;

    /// <summary>
    /// The Funds which the player has available
    /// </summary>
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
    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        Player player = obj as Player;
        return UniqueIdentifier == player.UniqueIdentifier;
    }

    public override int GetHashCode()
    {
        return UniqueIdentifier.GetHashCode();
    }
}
