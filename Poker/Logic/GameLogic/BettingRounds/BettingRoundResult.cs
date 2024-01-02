namespace Poker.Logic.GameLogic.BettingRounds;

/// <summary>
/// Enumerates possible outcomes of a betting round in a poker game.
/// </summary>
public enum BettingRoundResult
{
    /// <summary>
    /// Indicates that the game should proceed to the next stage, such as revealing more community cards.
    /// </summary>
    OpenNextStage,

    /// <summary>
    /// Indicates that all remaining cards should be revealed, typically used in a showdown scenario.
    /// </summary>
    RevealAllCards,

    /// <summary>
    /// Indicates that only one player remains after others have folded, awarding the pot to the last player.
    /// </summary>
    LastManStanding
}
