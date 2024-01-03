namespace Poker.PhysicalObjects.Chips;

/// <summary>
/// Provides utility methods for handling and distributing poker chips.
/// </summary>
public class Bank
{
    /// <summary>
    /// Recalculates and redistributes the given chips into a more usable denomination spread.
    /// </summary>
    /// <remarks>this method is not thread safe, please ensure locking or similar methods</remarks>
    /// <param name="chips">A dictionary of PokerChips and their quantities to be recolored.</param>
    /// <returns>A dictionary of PokerChips redistributed into a more practical denomination spread.</returns>
    public static IDictionary<PokerChip, ulong> Recolorize(IDictionary<PokerChip, ulong> chips)
    {
        ulong totalValue = ConvertChipsToValue(chips);
        return DistributeValueForUse(totalValue);
    }

    /// <summary>
    /// Converts a collection of poker chips to their total monetary value.
    /// </summary>
    /// <remarks>this method is not thread safe, please ensure locking or similar methods</remarks>
    /// <param name="chips">A dictionary of PokerChips and their quantities.</param>
    /// <returns>The total monetary value of the chips.</returns>
    public static ulong ConvertChipsToValue(IDictionary<PokerChip, ulong> chips)
    {
        ulong value = 0;
        foreach (KeyValuePair<PokerChip, ulong> stack in chips)
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
    public static ulong ConvertStackToValue(KeyValuePair<PokerChip, ulong> stack)
    {
        return ConvertChipsToValue(stack.Key, stack.Value);
    }

    
    
    /// <summary>
    /// Converts a monetary value into an equivalent set of poker chips.
    /// </summary>
    /// <remarks>this method is not thread safe, please ensure locking or similar methods</remarks>
    /// <param name="value">The monetary value to convert.</param>
    /// <returns>A dictionary of PokerChips equivalent to the given value.</returns>
    public static IDictionary<PokerChip, ulong> ConvertValueToChips(ulong value)
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

        return chipsToAdd;
    }
    
    /// <summary>
    /// Exchanges a single denomination of poker chips for smaller denominations.
    /// </summary>
    /// <remarks>this method is not thread safe, please ensure locking or similar methods</remarks>
    /// <param name="chip">The chip denomination to exchange.</param>
    /// <param name="amount">The amount of the specified chip to exchange.</param>
    /// <returns>A dictionary of smaller denomination PokerChips equivalent to the exchanged amount.</returns>
    public static IDictionary<PokerChip, ulong> ExchangeChipsForSmallerDenominations(PokerChip chip, ulong amount)
    {
        ulong totalValueToExchange = (ulong)chip * amount;
        var exchangedChips = new Dictionary<PokerChip, ulong>();

        // Assuming PokerChip is an enum where values represent chip denominations
        var smallerChips = Enum.GetValues(typeof(PokerChip))
            .Cast<PokerChip>()
            .OrderByDescending(v => v)
            .Where(v => v < chip);

        foreach (var smallerChip in smallerChips)
        {
            ulong smallerChipValue = (ulong)smallerChip;
            if (totalValueToExchange >= smallerChipValue)
            {
                ulong count = totalValueToExchange / smallerChipValue;
                exchangedChips[smallerChip] = count;
                totalValueToExchange -= count * smallerChipValue;
            }

            if (totalValueToExchange == 0) break;
        }

        return exchangedChips;
    }

    /// <summary>
    /// Merges two sets of poker chips into one.
    /// </summary>
    /// <remarks>this method is not thread safe, please ensure locking or similar methods</remarks>
    /// <param name="mainStack">The main set of chips to merge into (modified in-place).</param>
    /// <param name="valuesToAdd">The set of chips to add to the main stack.</param>
    public static void MergeStacks(IDictionary<PokerChip, ulong> mainStack, IDictionary<PokerChip, ulong> valuesToAdd)
    {
        foreach (var stack in valuesToAdd)
        {
            if (!mainStack.ContainsKey(stack.Key))
                mainStack[stack.Key] = stack.Value;
            else
                mainStack[stack.Key] += stack.Value;
        }
    }
    
    /// <summary>
    /// Distributes a monetary value into a practical set of poker chips for gameplay use.
    /// </summary>
    /// <remarks>this method is not thread safe, please ensure locking or similar methods</remarks>
    /// <param name="value">The monetary value to distribute.</param>
    /// <returns>A dictionary of PokerChips distributed into a usable set for gameplay.</returns>
    public static IDictionary<PokerChip, ulong> DistributeValueForUse(ulong value)
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

        // Handling any leftover value
        if (value > 0)
        {
            var leftover = ConvertValueToChips(value);
            MergeStacks(distributedChips, leftover);
        }
        
        return distributedChips; // Returning as IDictionary
    }
    
    /// <summary>
    /// Determines the maximum count for a denomination of chip based on the remaining value and total value.
    /// </summary>
    /// <param name="chip">The denomination of chip to evaluate.</param>
    /// <param name="remainingValue">The remaining value to be distributed.</param>
    /// <param name="totalValue">The total value of the distribution.</param>
    /// <returns>The calculated maximum count for the specified chip denomination.</returns>
    private static ulong DetermineMaxCountForDenomination(PokerChip chip, ulong remainingValue, ulong totalValue)
    {
        ulong chipValue = (ulong)chip;
        ulong maxCount = remainingValue / chipValue;

        // Strategy: Adjust the max count based on the chip's value relative to the total value
        ulong maxCountBasedOnTotal = totalValue / (chipValue * 5); // Adjust for spread

        // Additional logic to cap the count for lower denominations
        ulong lowValueThreshold = 50; // Threshold for considering a chip as low value
        ulong maxLowValueCount = 20;  // Maximum count for low-value chips

        if (chipValue <= lowValueThreshold)
        {
            return Math.Min(maxCount, Math.Min(maxLowValueCount, maxCountBasedOnTotal));
        }
        else
        {
            return Math.Min(maxCount, maxCountBasedOnTotal);
        }
    }

    public static double GetChipMicroValue(PokerChip chip)
    {
        return ConvertMacroToMicro((ulong)chip);
    }

    public static ulong ConvertMicroToMacro(double microValue)
    {
        double value = microValue * 100;
        value = Math.Round(value, 0);
        return (ulong)value;
    }
    public static ulong ConvertMicroToMacro(decimal microValue)
    {
        decimal value = microValue * 100;
        value = Math.Round(value, 0);
        return (ulong)value;
    }
    public static double ConvertMacroToMicro(ulong microValue)
    {
        double value = (double)microValue / 100;
        return Math.Round(value, 2);
    }
    public static decimal ConvertMacroToPreciseMicro(ulong microValue)
    {
        decimal value = (decimal)microValue / 100;
        return Math.Round(value, 2);
    }

}
