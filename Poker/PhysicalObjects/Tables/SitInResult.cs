namespace Poker.Net.PhysicalObjects.Tables;

/// <summary>
/// Enumerates the possible outcomes when a player attempts to sit in or buy into a table.
/// Used to provide feedback based on the result of the sit-in attempt.
/// </summary>
public enum SitInResult
{
    /// <summary>
    /// Indicates an unknown result or state.
    /// </summary>
    Unknown,

    /// <summary>
    /// Indicates the player does not have enough funds for the buy-in.
    /// </summary>
    NotEnoughFunds,

    /// <summary>
    /// Indicates the buy-in amount is too low.
    /// </summary>
    BuyinTooLow,

    /// <summary>
    /// Indicates the buy-in amount is too high.
    /// </summary>
    BuyinToHigh,

    /// <summary>
    /// Indicates the maximum number of allowed buy-ins has been reached.
    /// </summary>
    MaxBuyinCounterReached,

    /// <summary>
    /// Indicates that buying in is currently not allowed.
    /// </summary>
    BuyinIsCurrentlyNotAllowed,

    /// <summary>
    /// Indicates that the sit-in or buy-in attempt was successful.
    /// </summary>
    Success,
}
