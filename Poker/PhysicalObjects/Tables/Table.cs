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
            value += pot.PotValue;
        }
        return value;
    }
    public int GetNextBettingSeat(int seatID)
    {
        if (PlayersInBettingRoundCount <= 1)
            return -1;
        int counter = 0;
        do
        {
            seatID = (seatID + 1) % Seats.Length;
            counter++;
        }
        while ((!Seats[seatID].IsParticipatingGame() || Seats[seatID].IsFold || Seats[seatID].IsAllIn) && counter < Seats.Length);

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
        while (!Seats[seatID].IsParticipatingGame() && counter < Seats.Length);

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
        while (!Seats[seatID].IsParticipatingGame() && counter < Seats.Length);

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
        if (SeatedPlayersCount < 2)
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
            seat.PlayerPocketCards.Clear();
        }
    }

    public void DealPlayerCards()
    {
        RevokePlayerCards();
        TableDeck.ShuffleCards();
        for (int card = 1; card == 2; card++)
        {
            int currentSeat = DealerSeat;
            for (int activeSeat = 0; activeSeat < SeatsWithStakesCount; activeSeat++)
            {
                currentSeat = GetNextActiveSeat(currentSeat);
                Seats[currentSeat].PlayerPocketCards.DealCard(TableDeck);
            }
        }
    }
    public bool CheckAllPlayersAllIn()
    {
        // precheck
        if (SeatsWithStakesCount == 0)
            return false;
        // go through seats
        for (int i = 0; i < Seats.Length; i++)
        {
            if (Seats[i].IsParticipatingGame() && !Seats[i].PlayerPocketCards.IsFold && !Seats[i].IsAllIn)
                return false;
        }
        return true;
    }
    public bool CheckBetsAreAllEqual()
    {
        if (SeatsWithStakesCount <= 1)
            return true; // only 1 player left
        // go through seats
        ulong? betValue = null;
        for (int i = 0; i < Seats.Length; i++)
        {
            if (Seats[i].IsParticipatingGame() && !Seats[i].IsFold && !Seats[i].IsAllIn)
            {
                // seat is in the game! compare Bet
                if (betValue == null) // betvalue is uninitialized
                    betValue = Seats[i].PendingBets.PotValue;
                if (betValue != Seats[i].PendingBets.PotValue)
                    return false;
            }
        }
        return true;
    }

    public Deck TableDeck = new Deck();
    public CommunityCards CommunityCards = new CommunityCards();
    public Seat[] Seats;

    /// <summary>
    /// the number of players sitting at the table
    /// </summary>
    public int SeatedPlayersCount = 0;
    /// <summary>
    /// while a player is not in his seat, the seats funds might still be at stake
    /// </summary>
    public int SeatsWithStakesCount = 0;
    public int PlayersInBettingRoundCount = 0;
}