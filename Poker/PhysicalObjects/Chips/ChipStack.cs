using Poker.PhysicalObjects.Chips;

public class ChipStack
{
    private readonly Dictionary<PokerChip, ulong> _chips;

    public ChipStack()
    {
        _chips = new Dictionary<PokerChip, ulong>();
    }

    public void Merge(ChipStack otherStack)
    {
        if (otherStack == null)
            throw new ArgumentNullException(nameof(otherStack));

        foreach (var pair in otherStack._chips)
        {
            if (_chips.ContainsKey(pair.Key))
                _chips[pair.Key] += pair.Value;
            else
                _chips.Add(pair.Key, pair.Value);
        }
        otherStack._chips.Clear();
    }

    public Dictionary<PokerChip, ulong> GetCurrentStack()
    {
        return new Dictionary<PokerChip, ulong>(_chips);
    }

    // Other methods...
}