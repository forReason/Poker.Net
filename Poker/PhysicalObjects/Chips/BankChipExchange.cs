namespace Poker.PhysicalObjects.Chips;

/// <summary>
/// Provides utility methods for handling and distributing poker chips.
/// </summary>
public partial class Bank
{
    /// <summary>
    /// Recalculates and redistributes the given chips into a more usable denomination spread.
    /// </summary>
    /// <remarks>this method is not thread safe, please ensure locking or similar methods</remarks>
    /// <param name="chips">A dictionary of PokerChips and their quantities to be recolored.</param>
    /// <returns>A dictionary of PokerChips redistributed into a more practical denomination spread.</returns>
    public static IDictionary<PokerChip, ulong> Recolorize(IReadOnlyDictionary<PokerChip, ulong> chips)
    {
        ulong totalValue = ConvertChipsToValue(chips);
        return DistributeValueForUse(totalValue);
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
}
