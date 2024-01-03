namespace Poker.Logic.GameLogic.Rules;
/// <summary>
/// defines the limit structure of a game.
/// </summary>
/// <remarks>The Limit Structure is being used to determine which bet sizes are allowed</remarks>
public enum LimitType
{
    /// <summary>
    /// players can bet however much they like
    /// </summary>
    /// <remarks>when choosing to raise, a player must at least double the previous bet</remarks>
    NoLimit,
    /// <summary>
    /// - The minimum Bet equals BigBlind<br/>
    /// - The maximum Bet always equals the current Pot size + all players bets + the current players call amount<br/>
    /// - When choosing to raise a player must at least double the previous bet<br/>
    /// </summary>
    /// <remarks>in potLimit, there is no Cap such as in FixedLimit</remarks>
    PotLimit,
    /// <summary>
    /// players can only bet up to the set amount
    /// </summary>
    /// <remarks>
    /// The Limit in the pre-Flop is usually set to the BigBlind. After the Flop, the Limit is set to 2*BigBlind,<br/>
    /// The players can bet up to 4 Times. Set (1) Raise (2) re-raise (3) Cap (4)
    /// </remarks>
    FixedLimit
}
