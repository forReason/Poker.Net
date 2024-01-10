using Poker.PhysicalObjects.Players;
using Poker.PhysicalObjects.Tables;


namespace Poker.PhysicalObjects.Chips;

/// <summary>
/// Represents the pot in a poker game, holding the chips bet by players.
/// This class provides thread-safe operations to manage and distribute chips.
/// </summary>
public class Pot : ChipStack
{
    /// <summary>
    /// all players affiliated with this pot
    /// </summary>
    private readonly HashSet<Player> _players;

    /// <summary>
    /// Initializes a new instance of the Pot class.
    /// </summary>
    public Pot()
    {
        _players = new HashSet<Player>();
    }
    
    /// <summary>
    /// returns the current instance of attached players to the Pot
    /// </summary>
    public ReadOnlyHashSet<Player> Players => new ReadOnlyHashSet<Player>(_players);
    
    public void AddChips(IReadOnlyDictionary<PokerChip, ulong> chips, Player player)
    {
        _players.Add(player);
        AddChips(chips);
    }
    /// <summary>
    /// add chips to this pot
    /// </summary>
    /// <param name="chips"></param>
    /// <param name="player"></param>
    public void AddChips(ChipStack chips, Player player)
    {
        _players.Add(player);
        Merge(chips);
    }

    /// <summary>
    /// moves chips to another pot in thread safe manner
    /// </summary>
    /// <param name="target"></param>
    /// <param name="owner"></param>
    public void MoveAllChips(Pot target, Player owner)
    {
        target._players.Add(owner);
        target.Merge(this);
    }
    /// <summary>
    /// removes all chips from this pot and return the chips in a stack
    /// </summary>
    public ChipStack RemoveAllChips()
    {
        ChipStack chips = new ChipStack(_chips);
        Clear();
        return chips;
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
            if (StackValue == 0)
            {
                return PerformBetResult.PlayerHasNoFunds;
            }
            if (StackValue + target.StackValue < value)
            {
                MoveAllChips(target, owner);
                return PerformBetResult.AllIn;
            }
            MoveValue(target, value - target.StackValue, owner);
            return PerformBetResult.Success;
    }
    /// <summary>
    /// moves a certain value to another pot
    /// </summary>
    /// <param name="target"></param>
    /// <param name="value"></param>
    /// <param name="owner"></param>
    /// <returns></returns>
    public bool MoveValue(Pot target, ulong value, Player owner)
    {
        if (StackValue < value)
            return false;
        target.AddChips(RemoveValue(value), owner);
        return true;
    }
    /// <summary>
    /// clears the pot and cleans it returning its value
    /// </summary>
    /// <returns></returns>
    public override ulong Clear()
    {
        ulong totalValue = StackValue;
        UpdateStackValue(0);
        _chips.Clear();
        _players.Clear();
        _sortedChipsAscending = null;
        _sortedChipsDescending = null;
        return totalValue;
    }
}
