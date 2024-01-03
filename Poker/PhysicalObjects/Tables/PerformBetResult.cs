namespace Poker.PhysicalObjects.Tables;

/// <summary>
/// beeing used when a player tries to bet, in order to provide feedback
/// </summary>
public enum PerformBetResult
{
    Success,
    PlayerHasNoFunds,
    AllIn
}
