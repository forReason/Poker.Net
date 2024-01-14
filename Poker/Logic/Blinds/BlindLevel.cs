namespace Poker.Net.Logic.Blinds;
/// <summary>
/// Represents a level in a blind structure for a poker game, including the sizes of the small blind, big blind, and ante.
/// </summary>
public struct BlindLevel
{
    /// <summary>
    /// Gets or sets the level number in the blind structure.
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// Gets or sets the value of the small blind for this level.
    /// </summary>
    public ulong SmallBlind { get; set; }

    /// <summary>
    /// Gets or sets the value of the big blind for this level.
    /// </summary>
    public ulong BigBlind { get; set; }

    /// <summary>
    /// Gets or sets the value of the ante for this level.
    /// </summary>
    public ulong Ante { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BlindLevel"/> struct.
    /// </summary>
    /// <param name="level">The level number.</param>
    /// <param name="smallBlind">The small blind amount.</param>
    /// <param name="bigBlind">The big blind amount.</param>
    /// <param name="ante">The ante amount.</param>
    public BlindLevel(int level, ulong smallBlind, ulong bigBlind, ulong ante)
    {
        Level = level;
        SmallBlind = smallBlind;
        BigBlind = bigBlind;
        Ante = ante;
    }
}
