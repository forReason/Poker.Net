using Poker.PhysicalObjects.Chips;
using Poker.PhysicalObjects.Players;
using System.Collections.ObjectModel;

public class ChipStack
{
    private readonly Dictionary<PokerChip, ulong> _chips;
    /// <summary>
    /// represents the current Value of the pot as ulong
    /// </summary>
    public ulong StackValue { get; private set; }
    public ChipStack()
    {
        _chips = new Dictionary<PokerChip, ulong>();
    }
    /// <summary>
    /// returns the current poker chips of the pot in descending individual chipValue
    /// </summary>
    /// <param name="reverse">sort by ascending chip value</param>
    /// <returns></returns>
    public ReadOnlyCollection<KeyValuePair<PokerChip, ulong>> GetSortedChips(bool reverse = false)
    {
        if (!reverse)
        {
            if (_sortedChipsDescending != null) return _sortedChipsDescending;
            List<KeyValuePair<PokerChip, ulong>> sortedChips = _chips.OrderByDescending(chip => chip.Key).ToList();
            _sortedChipsDescending = new ReadOnlyCollection<KeyValuePair<PokerChip, ulong>>(sortedChips);
            return _sortedChipsDescending;
        }
        else
        {
            if (_sortedChipsAscending != null) return _sortedChipsAscending;
            var sortedChipsReverse = _chips.OrderBy(chip => chip.Key).ToList();
            _sortedChipsAscending = new ReadOnlyCollection<KeyValuePair<PokerChip, ulong>>(sortedChipsReverse);
            return _sortedChipsAscending;
        }
    }
    private ReadOnlyCollection<KeyValuePair<PokerChip, ulong>>? _sortedChipsDescending;
    private ReadOnlyCollection<KeyValuePair<PokerChip, ulong>>? _sortedChipsAscending;
    /// <summary>
    /// Gets a read-only snapshot of the chips in the pot.
    /// </summary>
    /// <remarks>This dictionary dos not update when the Pot updates</remarks>
    /// <returns>A read-only dictionary representing the chips currently in the pot.</returns>
    public ReadOnlyDictionary<PokerChip, ulong> GetChips()
    {
        return new ReadOnlyDictionary<PokerChip, ulong>(_chips);
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
    /// <summary>
    /// Removes chips equivalent to the specified value from the pot.
    /// </summary>
    /// <remarks>
    /// automatically exchanges chips with the bank if the colors do not match
    /// </remarks>
    /// <param name="value">The monetary value to remove.</param>
    /// <returns>The actual chips removed from the pot.</returns>
    private ChipStack RemoveValue(ulong value)
    {
        // Step 1: Calculate total value in the pot before removal
        if (value > StackValue)
        {
            throw new InvalidOperationException($"Cannot remove more value than {StackValue} which is present in the stack.");
        }

        // Step 2: Remove Value as close as possible
        ChipStack removedChips = new ();
        ulong remainingValue = value;
        do
        {
            Dictionary<PokerChip, ulong> remainderToRemove = new Dictionary<PokerChip, ulong>();
            // select chips which can be removed
            foreach (KeyValuePair<PokerChip, ulong> stack in GetSortedChips())
            {
                ulong chipCountToRemove = remainingValue / (ulong)stack.Key;
                chipCountToRemove = Math.Min(chipCountToRemove, stack.Value);
                remainderToRemove[stack.Key] = chipCountToRemove;
                remainingValue -= Bank.ConvertChipsToValue(stack.Key, chipCountToRemove);
                if (remainingValue <= 0)
                    break;
            }
            // remove the chips
            ChipStack removed = RemoveChips(remainderToRemove);
            removedChips.Merge(removed);
            // recolorize the pot
            Recolorize();
        }
        while (remainingValue > 0);

        return removedChips;
    }
    /// <summary>
    /// Recolorizes the chips.
    /// </summary>
    private void Recolorize()
    {
        // Assuming Bank.Recolorize returns IDictionary<PokerChip, ulong>
        IDictionary<PokerChip, ulong> recolorizedChips = Bank.Recolorize(this._chips);

        // Clear the current contents of _chips
        this._chips.Clear();

        // Add the recolorized chips back into _chips
        foreach (var chip in recolorizedChips)
        {
            this._chips.Add(chip.Key, chip.Value);
        }
        _sortedChipsAscending = null;
        _sortedChipsDescending = null;
    }
    /// <summary>
    /// Attempts to remove the specified chips from the ChipStack.
    /// </summary>
    /// <param name="chipsToRemove">Dictionary of chips to remove, with PokerChip as key and quantity as value.</param>
    /// <returns>A ChipStack representing the removed chips.</returns>
    private ChipStack RemoveChips(IReadOnlyDictionary<PokerChip, ulong> chipsToRemove)
    {
        ChipStack removedChips = new ChipStack();
        Dictionary<PokerChip, ulong> remainingChips = new Dictionary<PokerChip, ulong>(chipsToRemove);

        foreach (var chip in remainingChips)
        {
            PokerChip chipType = chip.Key;
            ulong requestedCount = chip.Value;
            ulong individualChipValue = (ulong)chipType;

            if (_chips.TryGetValue(chipType, out ulong currentAmountInStack))
            {
                ulong removableCount = Math.Min(requestedCount, currentAmountInStack);
                ulong removedValue = removableCount * individualChipValue;

                // Update the main stack
                _chips[chipType] -= removableCount;
                StackValue -= removedValue;

                // Update the removed chips
                removedChips._chips[chipType] = removableCount;
                removedChips.StackValue += removedValue;

                if (_chips[chipType] == 0)
                    _chips.Remove(chipType);
            }
        }

        // Handle the case where not enough chips are available
        ulong totalRemainingValue = 0;
        foreach (var chip in chipsToRemove)
        {
            ulong chipValue = (ulong)chip.Key * chip.Value;
            totalRemainingValue += chipValue;
        }
        totalRemainingValue -= removedChips.StackValue;

        if (totalRemainingValue > 0)
        {
            RemoveValue(totalRemainingValue); // Assuming this method adjusts StackValue and _chips accordingly
            var remainder = Bank.ConvertValueToChips(totalRemainingValue);
            foreach (var chip in remainder)
            {
                ulong chipCount = chip.Value;
                PokerChip chipType = chip.Key;

                if (removedChips._chips.TryGetValue(chipType, out ulong currentCount))
                {
                    removedChips._chips[chipType] = currentCount + chipCount;
                }
                else
                {
                    removedChips._chips[chipType] = chipCount;
                }
            }
            this.Clear();
        }

        return removedChips;
    }
    private ulong Clear()
    {
        ulong totalValue = StackValue;
        StackValue = 0;
        _chips.Clear();
        _sortedChipsAscending = null;
        _sortedChipsDescending = null;
        return totalValue;
    }
}