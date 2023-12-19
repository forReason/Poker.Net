using Poker.Tables;

namespace Poker.Games;

public partial class Game
{
    /// <summary>
    /// The timespan to wait before the game starts when the starting conditions are met
    /// </summary>
    public TimeSpan StartDelay { get; set; } = TimeSpan.FromSeconds(10);

    public DateTime StartGameAfter { get; set; } = DateTime.MinValue;

    /// <summary>
    /// returns true if the game got started
    /// </summary>
    /// <returns></returns>
    private async Task<bool> CheckStartGame()
    {
        // pre condition check
        if (DateTime.Now < StartGameAfter || GameTable.TakenSeats < MinimumPlayerCount)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.1));
            return false;
        }
        
        // apply delay
        if (StartDelay != TimeSpan.FromSeconds(0))
        {
            await Task.Delay(StartDelay);
        }
        
        // final start condition check
        return GameTable.TakenSeats >= MinimumPlayerCount;
    }
}