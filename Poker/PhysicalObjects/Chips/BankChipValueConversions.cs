namespace Poker.Net.PhysicalObjects.Chips;

/// <summary>
/// Provides utility methods for handling and distributing poker chips.
/// </summary>
public static partial class Bank
{
    /// <summary>
    /// Converts a collection of poker chips to their total monetary value.
    /// </summary>
    /// <remarks>this method is not thread safe, please ensure locking or similar methods</remarks>
    /// <param name="chips">A dictionary of PokerChips and their quantities.</param>
    /// <returns>The total monetary value of the chips.</returns>
    public static ulong ConvertChipsToValue(IReadOnlyDictionary<PokerChip, ulong> chips)
    {
        ulong value = 0;
        foreach (var stack in chips)
        {
            value += (ulong)stack.Key * stack.Value;
        }

        return value;
    }


    /// <summary>
    /// Converts a number of one chip into value
    /// </summary>
    /// <param name="chips"></param>
    /// <param name="chipCount"></param>
    /// <returns></returns>
    public static ulong ConvertChipsToValue(PokerChip chips, ulong chipCount)
    {
        return (ulong)chips * chipCount;
    }
    /// <summary>
    /// Converts a stack of one chip into value
    /// </summary>
    /// <param name="stack"></param>
    /// <returns></returns>
    public static ulong ConvertChipsToValue(KeyValuePair<PokerChip, ulong> stack)
    {
        return ConvertChipsToValue(stack.Key, stack.Value);
    }

    
    
    /// <summary>
    /// Converts a monetary value into an equivalent set of poker chips.
    /// </summary>
    /// <remarks>this method is not thread safe, please ensure locking or similar methods</remarks>
    /// <param name="value">The monetary value to convert.</param>
    /// <returns>A dictionary of PokerChips equivalent to the given value.</returns>
    public static ChipStack ConvertValueToChips(ulong value)
    {
        var chipsToAdd = new Dictionary<PokerChip, ulong>();
    
        // Assuming PokerChip is an enum where values represent chip denominations
        var sortedChips = Enum.GetValues(typeof(PokerChip))
            .Cast<PokerChip>()
            .OrderByDescending(v => v);

        foreach (var chip in sortedChips)
        {
            ulong chipValue = (ulong)chip;
            if (value >= chipValue)
            {
                ulong count = value / chipValue;
                chipsToAdd[chip] = count;
                value -= count * chipValue;
            }

            if (value == 0) break;
        }

        return new ChipStack(chipsToAdd);
    }
    
    /// <summary>
    /// Distributes a monetary value into a practical set of poker chips for gameplay use.
    /// </summary>
    /// <remarks>this method is not thread safe, please ensure locking or similar methods</remarks>
    /// <param name="value">The monetary value to distribute.</param>
    /// <returns>A dictionary of PokerChips distributed into a usable set for gameplay.</returns>
    public static ChipStack DistributeValueForUse(ulong value)
    {
        ulong originalValue = value;
        var distributedChips = new Dictionary<PokerChip, ulong>();
        var chipDenominations = Enum.GetValues(typeof(PokerChip)).Cast<PokerChip>().OrderByDescending(v => v);

        foreach (var chip in chipDenominations)
        {
            ulong chipValue = (ulong)chip;
            ulong maxCountForDenomination = DetermineMaxCountForDenomination(chip, value, originalValue);

            if (maxCountForDenomination > 0)
            {
                distributedChips[chip] = maxCountForDenomination;
                value -= chipValue * maxCountForDenomination;
            }

            if (value == 0) break;
        }

        ChipStack result = new ChipStack(distributedChips);
        // Handling any leftover value
        if (value > 0)
        {
            var leftover = ConvertValueToChips(value);
            result.Merge(leftover);
        }
        
        return result; // Returning as IDictionary
    }
}
