namespace Poker.PhysicalObjects.Tables;

/// <summary>
/// used to provide feedback when a player tries to sit in / buy into a table
/// </summary>
public enum SitInResult
{
    Unknown,
    NotEnoughFunds,
    BuyinTooLow,
    BuyinToHigh,
    // TODO: Implement according to GameRules
    MaxBuyinCounterReached,
    BuyinsCurrentlyNotAllowed,
    Sucess,
}