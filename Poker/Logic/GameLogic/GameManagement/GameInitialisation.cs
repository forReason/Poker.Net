

using Poker.PhysicalObjects.Cards;
using Poker.PhysicalObjects.Tables;

namespace Poker.Logic.GameLogic.GameManagement;

public partial class Game
{
    /// <summary>
    /// The timespan to wait before the game starts when the starting conditions are met
    /// </summary>
    public TimeSpan StartDelay { get; set; } = TimeSpan.FromSeconds(10);

    /// <summary>
    /// can be used to shedule a Game start
    /// </summary>
    public DateTime StartGameAfter { get; set; } = DateTime.MinValue;

    /// <summary>
    /// returns true if the game got started
    /// </summary>
    /// <returns></returns>
    private async Task<bool> CheckStartGame()
    {
        // pre condition check
        if (DateTime.Now < StartGameAfter || GameTable.TakenSeatsInternal.Count < Rules.MinimumPlayerCount)
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
        return GameTable.TakenSeatsInternal.Count >= Rules.MinimumPlayerCount;
    }

    /// <summary>
    /// this function deals cards to the players and chooses the player with the highest CardRank as starting dealer
    /// </summary>
    private void DetermineStartingDealer()
    {
        this.GameTable.TableDeck.ShuffleCards();
        // determine active Seats
        HashSet<int> ActiveSeats = new HashSet<int>();
        foreach (Seat seat in this.GameTable.Seats)
        {
            if (seat.IsParticipatingGame)
                ActiveSeats.Add(seat.SeatID);
        }

        do
        {
            // remove cards from previous rounds
            foreach (int seatId in ActiveSeats)
            {
                this.GameTable.Seats[seatId].PlayerPocketCards.Clear();
            }
            // shuffle deck if too little cards remaining
            if (this.GameTable.TableDeck.CardCount < ActiveSeats.Count)
                this.GameTable.TableDeck.ShuffleCards();
            // deal Card.GetCards
            foreach (int seatId in ActiveSeats)
            {
                this.GameTable.Seats[seatId].PlayerPocketCards.DealCard(this.GameTable.TableDeck);
            }
            // evaluate
            Card highestCard = Card.GetCard(CardRank.Two, CardSuit.Hearts);
            // need two rounds. one for determining the highest card, and one to remove all players with a lower card
            for (int i = 0; i < 2; i++)
            {
                HashSet<int> remainingSeats = new HashSet<int>();
                foreach (int seatId in ActiveSeats)
                {
                    if (this.GameTable.Seats[seatId].PlayerPocketCards.Cards[0] < highestCard)
                    {
                        this.GameTable.Seats[seatId].PlayerPocketCards.Clear();
                        continue;
                    }
                    else if (this.GameTable.Seats[seatId].PlayerPocketCards.Cards[0] > highestCard)
                        highestCard = this.GameTable.Seats[seatId].PlayerPocketCards.Cards[0];
                    remainingSeats.Add(seatId);
                }
                ActiveSeats = remainingSeats;
            }
        } while (ActiveSeats.Count > 1);
        // Set Dealer (need to go to the previous active player, since the buttons will be moved with the start of the first round
        this.GameTable.DealerSeat = this.GameTable.GetPreviousActiveSeat(ActiveSeats.First());
    }
}