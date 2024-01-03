namespace Poker.Logic.Fees;

/// <summary>
/// Defines the maximum rake cap for a specific player count.
/// </summary>
public struct RakeCap
{
    /// <summary>
    /// Number of players to which this rake cap applies.
    /// </summary>
    public readonly int PlayerCount;

    /// <summary>
    /// The cap amount for the specified number of players.
    /// </summary>
    public readonly decimal Cap;

    public RakeCap(int playerCount, decimal cap)
    {
        PlayerCount = playerCount;
        Cap = cap;
    }
}