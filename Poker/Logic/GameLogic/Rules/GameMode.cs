namespace Poker.Net.Logic.GameLogic.Rules;

/// <summary>
/// The GameMode defines wether the Table follows Tournament or Cash Game Rules
/// </summary>
public enum GameMode
{
    /// <summary>
    /// In a cash game:<br/>
    /// - Players can join or leave the table at any time.<br/>
    /// - Chips at the table represent real money.<br/>
    /// - Players can cash out their chips and leave with their profits or losses at any time.<br/>
    /// - The blinds remain constant, and each hand is an independent event.<br/>
    /// - There is no predetermined end time; the game continues as long as players are willing to play.<br/>
    /// </summary>
    Cash,

    /// <summary>
    /// In a tournament game:<br/>
    /// - Players pay an entry fee and receive a set amount of tournament chips.<br/>
    /// - Players cannot leave and take their chips; leaving means forfeiting the tournament.<br/>
    /// - Chips do not represent real money and cannot be cashed out until the tournament ends.<br/>
    /// - The blinds increase at regular intervals, creating a rising pressure on players.<br/>
    /// - The game ends when one player accumulates all the chips or when a final deal is made among the remaining players.<br/>
    /// - Rebuys or re-entries may be allowed depending on tournament rules.<br/>
    /// </summary>
    Tournament
}
