using Poker.Tables;

namespace Poker.Games;

public partial class Game
{
    private void SitOutBrokePlayers()
    {
        // clean up players without cash from the table
        foreach (Seat seat in GameTable.Seats)
        {
            if (seat.IsParticipatingGame())
            {
                // player is out of chips
                if (BettingStructure.RuleSet == GameMode.Cash)
                {
                    seat.SitOut();
                }
                else if (BettingStructure.RuleSet == GameMode.Tournament)
                {
                    GameTable.LeaveTable(seat.SeatID);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
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