using Poker.Logic.GameLogic.Rules;
using Poker.PhysicalObjects.Chips;

namespace Poker.Logic.Fees;

/// <summary>
/// Represents the rake structure of a poker game, detailing the rake levels based on the small blind amount.
/// </summary>
public class RakeStructure
{
    /// <summary>
    /// Gets the percentage of the pot taken as rake.
    /// </summary>
    public decimal PercentageRake { get; set; }
    
    /// <summary>
    /// the internal list of rake levels for lookup purposes
    /// </summary>
    private readonly SortedList<decimal, RakeLevel> _rakeLevels;

    /// <summary>
    /// Initializes a new instance of the <see cref="RakeStructure"/> class with specified rake levels.
    /// </summary>
    /// <param name="rakeLevels">List of rake levels to be included in the rake structure.</param>
    public RakeStructure(List<RakeLevel> rakeLevels)
    {
        this._rakeLevels = new SortedList<decimal, RakeLevel>(rakeLevels.Count);
        foreach (var rakeLevel in rakeLevels)
        {
            this._rakeLevels.Add(rakeLevel.SmallBlind, rakeLevel);
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RakeStructure"/> class based on the PokerStars rake structure as of 2023.
    /// </summary>
    /// <param name="limit">The type of betting limit (NoLimit, PotLimit, or FixedLimit).</param>
    public RakeStructure(LimitType limit)
    {
        List<RakeLevel> levels;
        if (limit is LimitType.NoLimit or LimitType.PotLimit)
        {
            levels =
            [
                new RakeLevel(0.01m, 3.5m / 100, [new(2, 0.3m)]),
                new RakeLevel(0.02m, 4.15m / 100, [new(4, 0.5m), new(5, 1m)]),
                new RakeLevel(0.08m, 4.5m / 100, [new(2, 0.5m), new(4, 1m), new(5, 1.5m)]),
                new RakeLevel(0.1m, 4.5m / 100, [new(2, 0.5m), new(4, 1m), new(5, 2m)]),
                new RakeLevel(0.25m, 5m / 100, [new(4, 0.75m), new(5, 2m)]),
                new RakeLevel(0.5m, 5m / 100, [new(4, 1m), new(5, 2.5m)]),
                new RakeLevel(1m, 5m / 100, [new(4, 1.25m), new(5, 2.75m)]),
                new RakeLevel(2m, 5m / 100, [new(4, 1.5m), new(5, 3m)]),
                new RakeLevel(3m, 5m / 100, [new(4, 1.5m), new(5, 3.5m)]),
                new RakeLevel(5m, 4.5m / 100, [new(4, 1.5m), new(5, 3m)]),
                new RakeLevel(10m, 4.5m / 100, [new(4, 1.75m), new(5, 3m)]),
                new RakeLevel(25m, 4.5m / 100, [new(4, 2.25m), new(5, 3m)]),
                new RakeLevel(50m, 4.5m / 100, [new(2, 2.5m), new(4, 3m), new(5, 5m)]),
                new RakeLevel(100m, 4.5m / 100, [new(2, 3m), new(4, 5m)])
            ];
        }
        else
        {
            levels =
            [
                new RakeLevel(0.02m, 4.5m / 100, [new(2, 0.01m)]),
                new RakeLevel(0.04m, 4.5m / 100, [new(2, 0.04m)]),
                new RakeLevel(0.1m, 4.5m / 100, [new(2, 0.1m)]),
                new RakeLevel(0.25m, 5m / 100, [new(2, 0.16m)]),
                new RakeLevel(0.5m, 5m / 100, [new(2, 0.4m)]),
                new RakeLevel(1m, 3m / 100, [new(2, 0.5m), new(4, 0.7m), new(5, 0.8m)]),
                new RakeLevel(2m, 2.5m / 100, [new(2, 0.5m), new(4, 0.7m), new(5, 1.25m)]),
                new RakeLevel(3m, 2.5m / 100, [new(2, 0.5m), new(4, 2m), new(5, 3m)]),
                new RakeLevel(10m, 2.25m / 100, [new(2, 0.5m), new(4, 2m), new(5, 3m)]),
                new RakeLevel(15m, 2m / 100, [new(2, 1m), new(4, 2m), new(5, 3m)]),
                new RakeLevel(20m, 1m / 100, [new(2, 1m), new(4, 2m), new(5, 3m)]),
                new RakeLevel(150m, 1m / 100, [new(2, 2m), new(4, 5m)]),
            ];
        }

        _rakeLevels = new SortedList<decimal, RakeLevel>(levels.Count);
        foreach (var rakeLevel in levels)
        {
            this._rakeLevels.Add(rakeLevel.SmallBlind, rakeLevel);
        }
    }

    /// <summary>
    /// Calculates the rake for a given pot based on the small blind amount and game type.
    /// </summary>
    /// <param name="pot">The pot for which to calculate the rake.</param>
    /// <param name="smallBlind">The small blind amount used to determine the rake level.</param>
    /// <param name="micro">Indicates whether the game is in micro-stakes format.</param>
    /// <returns>The calculated rake amount for the pot.</returns>
    public ulong CalculateRake(Pot pot, ulong smallBlind, bool micro)
    {

        RakeLevel level = GetRakeLevelBasedOnSmallBlind(smallBlind, micro);
        decimal rake = pot.PotValue * level.PercentageRake;
        decimal cap = level.GetCapBasedOnPlayerCount(pot.Players.Count);
        if (micro)
        {
            cap *= 100;
        }

        return (ulong)Math.Min(rake, cap);
    }

    /// <summary>
    /// Retrieves the appropriate rake level based on the small blind amount.
    /// </summary>
    /// <param name="smallBlind">The small blind amount to match the rake level.</param>
    /// <param name="micro">Indicates whether the game is in micro-stakes format.</param>
    /// <returns>The matched rake level.</returns>
    public RakeLevel GetRakeLevelBasedOnSmallBlind(ulong smallBlind, bool micro)
    {
        decimal smallBlindDecimal = (decimal)smallBlind;
        if (micro)
            smallBlindDecimal /= 100;
        foreach (KeyValuePair<decimal, RakeLevel> rakeLevel in _rakeLevels)
        {
            if (smallBlindDecimal <= rakeLevel.Key)
            {
                return rakeLevel.Value;
            }
        }

        return _rakeLevels.Last().Value; // return highest level if blind larger than
    }
}
