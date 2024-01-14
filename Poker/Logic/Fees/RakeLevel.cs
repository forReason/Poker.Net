namespace Poker.Net.Logic.Fees;

/// <summary>
/// Represents a level of rake structure in a poker game, including the small blind, rake percentage, and caps based on player count.
/// </summary>
public class RakeLevel
{
    /// <summary>
    /// The small blind amount associated with this rake level.
    /// </summary>
    public readonly decimal SmallBlind;

    /// <summary>
    /// The percentage of the pot taken as rake.
    /// </summary>
    public readonly decimal PercentageRake;

    private readonly SortedList<int, decimal> _rakeCaps;

    /// <summary>
    /// Initializes a new instance of the <see cref="RakeLevel"/> class.
    /// </summary>
    /// <param name="smallBlind">The small blind amount.</param>
    /// <param name="percentageRake">The rake percentage.</param>
    /// <param name="rakeCaps">A collection of rake caps sorted by player count.</param>
    public RakeLevel(decimal smallBlind, decimal percentageRake, IReadOnlyCollection<RakeCap> rakeCaps)
    {
        SmallBlind = smallBlind;
        PercentageRake = percentageRake;
        _rakeCaps = new SortedList<int, decimal>(rakeCaps.Count);
        foreach (var rakeCap in rakeCaps)
        {
            _rakeCaps.Add(rakeCap.PlayerCount, rakeCap.Cap);
        }
    }

    /// <summary>
    /// Gets the rake cap based on the given player count.
    /// </summary>
    /// <param name="playerCount">The number of players.</param>
    /// <returns>The cap amount for the specified number of players.</returns>
    public decimal GetCapBasedOnPlayerCount(int playerCount)
    {
        foreach (var rakeCap in _rakeCaps)
        {
            if (playerCount <= rakeCap.Key)
            {
                return rakeCap.Value;
            }
        }
        return _rakeCaps.Last().Value; // Default cap if player count exceeds all defined caps
    }
}