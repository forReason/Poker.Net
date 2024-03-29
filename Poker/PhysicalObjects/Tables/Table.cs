using Poker.Net.Logic.GameLogic.GameManagement;
using Poker.Net.PhysicalObjects.Chips;
using Poker.Net.PhysicalObjects.Decks;

namespace Poker.Net.PhysicalObjects.Tables;

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
        
        if (game is not null)
        {
            this.TableGame = game;
            game.GameTable = this;
        }
    }
    /// <summary>
    /// backreference to a running game on the table
    /// </summary>
    public Game? TableGame { get; set; }  

    /// <summary>
    /// the pots in the center which the players Bet into
    /// </summary>
    public readonly List<Pot> CenterPots = [];

    /// <summary>
    /// Calculates the total value of the center pots
    /// </summary>
    /// <returns></returns>
    public ulong GetTotalStackValue()
    {
        ulong value = 0;
        foreach (Pot pot in CenterPots)
        {
            value += pot.StackValue;
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
        while ((!Seats[seatID].IsParticipatingGame || !Seats[seatID].PlayerPocketCards.HasCards || Seats[seatID].IsAllIn) && counter < Seats.Length);

        if (counter >= Seats.Length)
            return -1;
        return seatID;
    }
    /// <summary>
    /// returns the next active seat on the Table
    /// </summary>
    /// <param name="seatID"></param>
    /// <returns>-1 if no active Seat was found</returns>
    public int GetNextActiveSeat(int seatID)
    {
        int counter = 0;
        do
        {
            seatID = (seatID + 1) % Seats.Length;
            if (seatID > Seats.Length)
                seatID = 0;
            counter++;
        } 
        while (!Seats[seatID].IsParticipatingGame && counter < Seats.Length);

        if (counter >= Seats.Length)
            return -1;
        return seatID;
    }
    
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
        while (!Seats[seatID].IsParticipatingGame && counter < Seats.Length);

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
    public int SmallBlindSeat;
    /// <summary>
    /// the seat which as currently the big blind
    /// </summary>
    /// <remarks>
    /// the big blind is forced to make a full bet at the start of the round<br/>
    /// it is the player left of the SmallBlind
    /// </remarks>
    public int BigBlindSeat;
    /// <summary>
    /// Moves the Dealer and blind buttons appropriately
    /// </summary>
    public MoveButtonsResult MoveButtons()
    {
        // Move Dealer
        Seats[DealerSeat].IsDealer = false;
        DealerSeat = GetNextActiveSeat(DealerSeat);
        if (DealerSeat == -1)
            return MoveButtonsResult.NoActivePlayers;
        Seats[DealerSeat].IsDealer = true;
        // Move smallBlind
        Seats[SmallBlindSeat].IsSmallBlind = false;
        if (SeatedPlayersCount < 2)
            SmallBlindSeat = DealerSeat;
        else
        {
            SmallBlindSeat = GetNextActiveSeat(DealerSeat);
            if (SmallBlindSeat == -1)
                return MoveButtonsResult.NoActivePlayers;
        }
            
        Seats[SmallBlindSeat].IsSmallBlind = true;
        // Move BigBlind
        Seats[BigBlindSeat].IsBigBlind = false;
        BigBlindSeat = GetNextActiveSeat(SmallBlindSeat);
        if (BigBlindSeat == -1)
            return MoveButtonsResult.NoActivePlayers;
        Seats[BigBlindSeat].IsBigBlind = true;
        return MoveButtonsResult.Success;
    }

    /// <summary>
    /// clears all player hands, preparing for a new round or anything
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
    /// <remarks>only deals cards to seats with stakes</remarks>
    public void DealPlayerCards()
    {
        RevokePlayerCards();
        TableDeck.ShuffleCards();
        int seat = 0;
        for (int card = 1; card <= 2; card++)
        {
            int currentSeat = DealerSeat;
            if (this.TableGame != null)
            {
                for (seat = 0; seat < SeatsWithStakesCount; seat++)
                {
                    currentSeat = GetNextActiveSeat(currentSeat);
                    Seats[currentSeat].PlayerPocketCards.DealCard(TableDeck);
                }
            }
            else
            {
                for (seat = 0; seat < this.Seats.Length; seat++)
                {
                    if (Seats[seat].Player == null)
                        continue;
                    Seats[seat].PlayerPocketCards.DealCard(TableDeck);
                }
            }
        }
    }
    /// <summary>
    /// utility function to evaluate if every active player is all in
    /// </summary>
    /// <returns></returns>
    public bool CheckAllPlayersAllIn()
    {
        // precheck
        if (SeatsWithStakesCount == 0)
            return false;
        // go through seats
        foreach (var seat in Seats)
        {
            if (seat.IsParticipatingGame && seat.PlayerPocketCards.HasCards && !seat.IsAllIn)
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
        foreach (var seat in Seats)
        {
            if (!seat.IsParticipatingGame || !seat.PlayerPocketCards.HasCards || seat.IsAllIn) continue;
            // seat is in the game! compare Bet
            betValue ??= seat.PendingBets.StackValue;
            if (betValue != seat.PendingBets.StackValue)
                return false;
        }
        return true;
    }

    /// <summary>
    /// the card deck of the table
    /// </summary>
    public readonly Deck TableDeck = new ();
    /// <summary>
    /// the middle 5 cards of the table shared by each player, including the burn cards
    /// </summary>
    public readonly CommunityCards CommunityCards = new ();
    /// <summary>
    /// the table seats where players can sit
    /// </summary>
    public readonly Seat[] Seats;

    /// <summary>
    /// the number of players sitting at the table
    /// </summary>
    public int SeatedPlayersCount => SeatedPlayers.Count;
    /// <summary>
    /// while a player is not in his seat, the seats funds might still be at stake
    /// </summary>
    public int SeatsWithStakesCount = 0;
    /// <summary>
    /// the amount of players which are active in the current betting round
    /// </summary>
    public int PlayersInBettingRoundCount = 0;
}