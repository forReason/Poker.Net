using Poker.Tables;
using System.Reflection.Metadata.Ecma335;

namespace Poker.Games;

public partial class Game
{

    /// <summary>
    /// returns true if the game got started
    /// </summary>
    /// <returns></returns>
    private async Task<bool> InitializeRound()
    {
        // initialize dealer
        for(int i = 0; i < GameTable.Seats.Length; i++)
        {
            if (GameTable.Seats[i].IsDealer)
            {

            }
        }
    }
    private async Task<bool> CheckRoundPrecondition()
    {
        // clean up players without cash from the table
        foreach (Seat seat in GameTable.Seats)
        {
            if (seat.BankChips.GetValue() <= 0)
            {
                // player is out of chips
                if (BettingStructure.RuleSet == Blinds.TableRuleSet.Cash)
                {
                    seat.SitOut();
                }
                else if(BettingStructure.RuleSet == Blinds.TableRuleSet.Tournament)
                {
                    GameTable.LeaveTable(seat.SeatID);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
        // make sure we have enough players to play a round
        if (GameTable.ActivePlayers < 1)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.1));
            return false;
        }
        return true;
    }
}