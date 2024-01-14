namespace Poker.Net.PhysicalObjects.Tables;

/// <summary>
/// Represents the possible results of a move button operation.
/// </summary>
public enum MoveButtonsResult
{
    /// <summary>
    /// Indicates that the move button operation was successful.
    /// </summary>
    Success, 

    /// <summary>
    /// Indicates that the move button operation failed due to no active players.
    /// </summary>
    NoActivePlayers
}
