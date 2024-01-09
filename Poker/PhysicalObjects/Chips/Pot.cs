using Poker.PhysicalObjects.Players;
using Poker.PhysicalObjects.Tables;


namespace Poker.PhysicalObjects.Chips;

/// <summary>
/// Represents the pot in a poker game, holding the chips bet by players.
/// This class provides thread-safe operations to manage and distribute chips.
/// </summary>
public class Pot
{
    /// <summary>
    /// represents all individual chips in the pot
    /// </summary>
    private Dictionary<PokerChip, ulong> _chips;
    /// <summary>
    /// lock to make sure interactions cannot cross each other
    /// </summary>
    private readonly object _lock = new object();
    /// <summary>
    /// all players affiliated with this pot
    /// </summary>
    private readonly HashSet<Player> _players;

    /// <summary>
    /// Initializes a new instance of the Pot class.
    /// </summary>
    public Pot()
    {
        _chips = new Dictionary<PokerChip, ulong>();
        _players = new HashSet<Player>();
    }
    
    

    /// <summary>
    /// returns the current instance of attached players to the Pot
    /// </summary>
    public ReadOnlyHashSet<Player> Players => new ReadOnlyHashSet<Player>(_players);

    /// <summary>
    /// Adds chips to the pot. This operation is thread-safe.
    /// </summary>
    /// <param name="chips">The chips to add to the pot.</param>
    /// <param name="player">The player which is being added to the Pots player list, meaning he has skin in the game.</param>
    public void AddChips(IDictionary<PokerChip, ulong> chips, Player player)
    {
        lock (_lock)
        {
            AddChipsInternal(chips, player);
        }
    }
    private void AddChipsInternal(IDictionary<PokerChip, ulong> chips, Player player)
    {
        _players.Add(player);
        _sortedChipsAscending = null;
        _sortedChipsDescending = null;
        foreach (var chip in chips)
        {
            this.PotValue += ((ulong)chip.Key) * chip.Value;
            if (_chips.ContainsKey(chip.Key))
            {
                _chips[chip.Key] += chip.Value;
            }
            else
            {
                _chips.Add(chip.Key, chip.Value);
            }
        }
    }

    /// <summary>
    /// moves chips to another pot in thread safe manner
    /// </summary>
    /// <param name="target"></param>
    /// <param name="owner"></param>
    public void MoveAllChips(Pot target, Player owner)
    {
        lock (_lock)
        {
            target.AddChips(RemoveValueInternal(PotValue), owner);
        }
    }

    private bool MoveValueInternal(Pot target, ulong value, Player owner)
    {
        if (PotValue < value)
            return false;
        target.AddChips(RemoveValueInternal(value), owner);
        return true;
    }
    /// <summary>
    /// moves a requested value to a betting pot, according to betting rules (if not enough balance, still perform)
    /// </summary>
    /// <remarks>
    /// please note that this function fills the target pot to the desired value.<br/>
    /// Example: target por is worth 10. You call PerformBet(target, 50)<br/>
    /// this means the remainder (40) is moved to the target pot so that its final Value is 50 as requested
    /// </remarks>
    /// <param name="target"></param>
    /// <param name="value"></param>
    /// <param name="owner"></param>
    /// <returns></returns>
    public PerformBetResult PerformBet(Pot target, ulong value, Player owner)
    {
        lock (_lock)
        {
            if (PotValue == 0)
            {
                return PerformBetResult.PlayerHasNoFunds;
            }
            if (PotValue + target.PotValue < value)
            {
                MoveAllChips(target, owner);
                return PerformBetResult.AllIn;
            }
            MoveValueInternal(target, value - target.PotValue, owner);
            return PerformBetResult.Success;
        }
    }
}
