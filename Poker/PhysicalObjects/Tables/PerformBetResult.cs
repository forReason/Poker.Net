namespace Poker.PhysicalObjects.Tables;

/// <summary>
/// Enumerates the possible outcomes when a player attempts to bet.
/// Being used to provide feedback based on the bet action's result.
/// </summary>
public enum PerformBetResult
{
    /// <summary>
    /// Indicates that the betting action was successful.
    /// </summary>
    Success,

    /// <summary>
    /// Indicates that the betting action failed because the player does not have sufficient funds.
    /// </summary>
    PlayerHasNoFunds,

    /// <summary>
    /// Indicates that the player has placed all of their available funds into the bet (all-in).
    /// </summary>
    AllIn
}

