using Poker.Chips;
using Poker.Decks;

namespace Poker.Tables;

public partial class Table
{
    public Table(
        uint seats,
        Games.Game? game
        )
    {
        Seats = new Seat[seats];
        for (int i = 0; i < Seats.Length; i++)
        {
            Seats[i] = new Seat(i, this);
        }
        this.TableGame = game;
    }

    public Games.Game? TableGame { get; set; } = null; 

    /// <summary>
    /// the pots in the center which the players Bet into
    /// </summary>
    public readonly List<Pot> CenterPots = new List<Pot>();

    

    /// <summary>
    /// Calculates the total value of the center pots
    /// </summary>
    /// <returns></returns>
    public ulong GetTotalPotValue()
        {
            ulong value = 0;
            foreach (Pot pot in CenterPots)
            {
                value += pot.GetValue();
            }
        return value;
        }

    /// <summary>
    /// returns the next active seat on the Table
    /// </summary>
    /// <param name="seatID"></param>
    /// <returns>-1 if no active Seat was found</returns>
    /// <summary>
    /// Returns the next active seat on the Table
    /// </summary>
    /// <param name="seatID"></param>
    /// <returns>-1 if no active Seat was found</returns>
    public int GetNextActiveSeat(int seatID)
    {
        int counter = 0;
        do
        {
            seatID = (seatID + 1) % Seats.Length;
            counter++;
        } 
        while (!Seats[seatID].IsActive() && counter < Seats.Length);

        if (counter >= Seats.Length)
            return -1;
        return seatID;
    }

    /// <summary>
    /// returns the next active seat on the Table
    /// </summary>
    /// <param name="seatID"></param>
    /// <returns>-1 if no active Seat was found</returns>
    /// <summary>
    /// Returns the previous active seat on the Table
    /// </summary>
    /// <param name="seatID"></param>
    /// <returns>-1 if no active Seat was found</returns>
    public int GetPreviousActiveSeat(int seatID)
    {
        int counter = 0;
        do
        {
            seatID--;
            if (seatID < 0)
                seatID = Seats.Length - 1;
            counter++;
        } 
        while (!Seats[seatID].IsActive() && counter < Seats.Length);

        if (counter >= Seats.Length)
            return -1;
        return seatID;
    }

    public int DealerSeat = -1;
    public int SmallBlindSeat = 0;
    public int BigBlindSeat = 0;
    /// <summary>
    /// Moves the Dealer and blind buttons apropriately
    /// </summary>
    public void MoveButtons()
    {
        // Move Dealer
        Seats[DealerSeat].IsDealer = false;
        DealerSeat = GetNextActiveSeat(DealerSeat);
        Seats[DealerSeat].IsDealer = true;
        // Move smallBlind
        Seats[SmallBlindSeat].IsSmallBlind = false;
        if (ActivePlayers < 2)
            SmallBlindSeat = DealerSeat;
        else
            SmallBlindSeat = GetNextActiveSeat(DealerSeat);
        Seats[SmallBlindSeat].IsSmallBlind = true;
        // Move BigBlind
        Seats[BigBlindSeat].IsBigBlind = false;
        BigBlindSeat = GetNextActiveSeat(SmallBlindSeat);
        Seats[BigBlindSeat].IsBigBlind = true;
    }

    /// <summary>
    /// clears all playerhands, preparing for a new round or anything
    /// </summary>
    public void RevokePlayerCards()
    {
        foreach (Seat seat in Seats)
        {
            seat.PlayerHand.Clear();
        }
    }

    public void DealPlayerCards()
    {
        RevokePlayerCards();
        TableDeck.ShuffleCards();
        for (int card = 1; card == 2; card++)
        {
            int currentSeat = DealerSeat;
            for (int activeSeat = 0; activeSeat < ActiveSeats; activeSeat++)
            {
                currentSeat = GetNextActiveSeat(currentSeat);
                Seats[currentSeat].PlayerHand.DealCard(TableDeck);
            }
        }
    }

    public Deck TableDeck = new Deck();
    public CommunityCards TableCards = new CommunityCards();
    public Seat[] Seats;
    public int ActivePlayers = 0;
    public int ActiveSeats = 0;
}