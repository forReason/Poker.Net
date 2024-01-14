namespace Poker.Net.Logic.GameLogic.GameManagement;

public partial class Game
{
    // TODO: This belongs into rules
    /// <summary>
    /// defines the maximum count of Rebuys
    /// </summary>
    /// <remarks>
    /// set to -1 (default) for unlimited Amount
    /// </remarks>
    public int MaxRebuys { get; set; } = -1;
    /// <summary>
    /// the maximum game level where rebuys are allowed
    /// </summary>
    /// <remarks>
    /// set to -1 (default) for unlimited Time
    /// </remarks>
    public int MaxRebuyLevel { get; set; } = -1;
    /// <summary>
    /// The maximum amount of chips a player is allowed to have in his stack before he can take a rebuy
    /// </summary>
    /// <remarks>
    ///default is 0, beaning only players with 0 chips on the Table are allowed to Rebuy
    /// </remarks>
    public int MaxChipsForRebuy { get; set; } = 0;

    /// <summary>
    /// Method to determine if rebuy is allowed
    /// </summary>
    /// <param name="currentRebuys"></param>
    /// <param name="tournamentTime"></param>
    /// <param name="currentChips"></param>
    /// <returns></returns>
    public bool CanRebuy(int currentRebuys, TimeSpan tournamentTime, int currentChips)
    {
        // TODO: implement Method?
        return currentRebuys < MaxRebuys && 
            CurrentBlindLevel.Level <= MaxRebuyLevel && 
               currentChips <= MaxChipsForRebuy;
    }
}
