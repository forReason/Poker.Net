using Poker.Net.Logic.GameLogic.Rules;
using Poker.Net.PhysicalObjects.Tables;

namespace Poker.Net.Logic.GameLogic.GameManagement;

public partial class Game
{
    /// <summary>
    /// sits out player which do no longer have stash. they can then rebuy according to the gamerules
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void SitOutBrokePlayers()
    {
        // clean up players without cash from the table
        foreach (Seat seat in GameTable.Seats)
        {
            if (seat.IsParticipatingGame)
            {
                // player is out of chips
                if (Rules.GameMode == GameMode.Cash)
                {
                    seat.SitOut();
                }
                else if (Rules.GameMode == GameMode.Tournament)
                {
                    seat.Leave();
                }
            }
        }
    }
    /// <summary>
    /// checks if we have enough seated players to start a round
    /// </summary>
    /// <returns></returns>
    private async Task<bool> CheckRoundPrecondition()
    {
        // make sure we have enough players to play a round
        if (GameTable.SeatedPlayersCount < 1)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.1));
            return false;
        }
        return true;
    }
}