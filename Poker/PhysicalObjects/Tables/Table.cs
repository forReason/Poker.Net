using Poker.Logic.GameLogic.GameManagement;
using Poker.PhysicalObjects.Chips;
using Poker.PhysicalObjects.Decks;

namespace Poker.PhysicalObjects.Tables;

/// <summary>
/// table is the central hub for all physical game actions. It contains the cards, chips, seats and players
/// </summary>
public partial class Table
{
    public Table(
        uint seats,
        Game? game
        )
    {
        Seats = new Seat[seats];
        for (int i = 0; i < Seats.Length; i++)
        {
            Seats[i] = new Seat(i, this);
        }
        this.TableGame = game;
    }
    /// <summary>
    /// backreference to a running game on the table
    /// </summary>
    public Game? TableGame { get; set; } = null; 

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
    /// <summary>
    /// returns the next active player seat who can perform a bet
    /// </summary>
    /// <param name="seatID"></param>
    /// <returns></returns>
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
    /// <summary>
    /// the seat which is currently set as the dealer
    /// </summary>
    public int DealerSeat = -1;
    /// <summary>
    /// the seat which as currently the small blind
    /// </summary>
    /// <remarks>
    /// the small blind is forced to make half a bet at the start of the round<br/>
    /// when only 2 players are at the Table, the Dealer is the small Blind at the same Time. Otherwise, its the seat left of the dealer.
    /// </remarks>
    public int SmallBlindSeat = 0;
    /// <summary>
    /// the seat which as currently the big blind
    /// </summary>
    /// <remarks>
    /// the big blind is forced to make a full bet at the start of the round<br/>
    /// it is the player left of the SmallBlind
    /// </remarks>
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
    /// <summary>
    /// shuffles the deck according to official shuffling ruled and deals 2 cards to each player, one by one.
    /// </summary>
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
    /// <summary>
    /// utility function to evaluate if evewry active player is all in
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// Utility function to verify that every active player has the same bet (or is all in if lower)
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// the card deck of the table
    /// </summary>
    public Deck TableDeck = new Deck();
    /// <summary>
    /// the middle 5 cards of the table shared by each player, including the burn cards
    /// </summary>
    public CommunityCards CommunityCards = new CommunityCards();
    /// <summary>
    /// the table seats where players can sit
    /// </summary>
    public Seat[] Seats;

    /// <summary>
    /// the number of players sitting at the table
    /// </summary>
    public int SeatedPlayersCount = 0;
    /// <summary>
    /// while a player is not in his seat, the seats funds might still be at stake
    /// </summary>
    public int SeatsWithStakesCount = 0;
    /// <summary>
    /// the amount of players which are active in the current betting round
    /// </summary>
    public int PlayersInBettingRoundCount = 0;
}