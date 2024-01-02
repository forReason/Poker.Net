namespace Poker.Logic.GameLogic.BettingRounds;

public partial class BettingRound
{
    /// <summary>
    /// Evaluates the outcome of the current betting round in the poker game.
    /// </summary>
    /// <returns>
    /// The result of the betting round, indicating whether to open the next stage, reveal all cards, or if there's only one player remaining.
    /// </returns>
    /// <remarks>
    /// This method checks the number of players still in the betting round and their all-in status to determine the next course of action:<br/>
    /// - Returns LastManStanding if only one player remains in the round.<br/>
    /// - Returns RevealAllCards if all remaining players are all-in.<br/>
    /// - Otherwise, returns OpenNextStage to proceed to the next stage of the game.
    /// </remarks>
    public BettingRoundResult EvaluateBettingRound()
    {
        if (_game.GameTable.PlayersInBettingRoundCount <= 1)
            return BettingRoundResult.LastManStanding;
        return _game.GameTable.CheckAllPlayersAllIn() ? BettingRoundResult.RevealAllCards : BettingRoundResult.OpenNextStage;
    }
}