namespace Poker.Logic.Blinds;

/// <summary>
/// Represents the ratio of the initial buy-in to the small blind in a poker game.
/// </summary>
public enum BlindToBuyInRatio : ulong
{
    /// <summary>
    /// A ratio where the buy-in is twenty-five times the small blind.
    /// </summary>
    OneToTwentyFive = 25,

    /// <summary>
    /// A ratio representing a "short stack" game, with the buy-in being five times the small blind.
    /// </summary>
    OneToFive = 5,

    /// <summary>
    /// Another "short stack" ratio, with the buy-in being ten times the small blind.
    /// </summary>
    OneToTen = 10,

    /// <summary>
    /// A ratio where the buy-in is fifty times the small blind.
    /// </summary>
    OneToFifty = 50,

    /// <summary>
    /// A ratio where the buy-in is eighty times the small blind.
    /// </summary>
    OneToEighty = 80,

    /// <summary>
    /// A ratio where the buy-in is one hundred times the small blind.
    /// </summary>
    OneToOneHundred = 100,

    /// <summary>
    /// A ratio where the buy-in is two hundred times the small blind.
    /// </summary>
    OneToTwoHundred = 200
}