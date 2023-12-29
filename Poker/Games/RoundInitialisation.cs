using System.Diagnostics;
using Poker.Tables;
using System.Reflection.Metadata.Ecma335;

namespace Poker.Games;

public partial class Game
{

    /// <summary>
    /// returns true if the game got started
    /// </summary>
    /// <returns></returns>
    private async Task InitializeRound()
    {
        // move buttons
        if (this.GameTable.DealerSeat == -1)
        {
            DetermineStartingDealer();
        }
        this.GameTable.MoveButtons();
        
        // shuffle deck and deal player cards
        this.GameTable.DealPlayerCards();
    }
    private async Task SitOutBrokePlayers()
    {
        // clean up players without cash from the table
        foreach (Seat seat in GameTable.Seats)
        {
            if (seat.BankChips.GetValue() <= 0)
            {
                // player is out of chips
                Debug.WriteLine($"Player {seat.Player.UniqueIdentifier} is broke and sits out from seat {seat.SeatID}!");
                seat.SitOut();
            }
        }
    }
    private async Task<bool> CheckRoundPrecondition()
    {
        // make sure we have enough players to play a round
        if (GameTable.ActivePlayers < 1)
        {
            if (this.GameTimeStructure == GameTimeStructure.RealTime)
                await Task.Delay(TimeSpan.FromSeconds(2));
            return false;
        }
        return true;
    }
}