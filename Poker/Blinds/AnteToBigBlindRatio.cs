namespace Poker.Blinds;

/// <summary>
/// Represents the ratio of the ante to the big blind in a game of poker.
/// </summary>
public enum AnteToBigBlindRatio
{
    /// <summary>
    /// No ante is required in relation to the big blind.
    /// </summary>
    None = 0,

    /// <summary>
    /// The ante is one-tenth the size of the big blind.
    /// </summary>
    OneToTen = 10,

    /// <summary>
    /// The ante is one-fifth the size of the big blind.
    /// </summary>
    OneToFive = 5,

    /// <summary>
    /// The ante is half the size of the big blind.
    /// </summary>
    OneToTwo = 2,

    /// <summary>
    /// The ante is equal to the size of the big blind.
    /// </summary>
    OneToOne = 1
}
