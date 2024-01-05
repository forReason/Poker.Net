
using Poker.Logic.GameLogic.GameManagement;
using Poker.Logic.GameLogic.Rules;
using Poker.PhysicalObjects.Chips;
using Poker.PhysicalObjects.Decks;
using Poker.PhysicalObjects.Players;

namespace Poker.PhysicalObjects.Tables;

/// <summary>
/// a TableSeat where a player can Sit
/// </summary>
public class Seat
{
    /// <summary>
    /// Creates a Seat attached to a Table.
    /// </summary>
    /// <param name="seatID"></param>
    public Seat(int seatID, Table table)
    {
        SeatID = seatID;
        Table = table;
    }

    /// <summary>
    /// The Player sitting on this Seat. If it's null, the seat is Free
    /// </summary>

    public Player? Player => _player;
    private volatile Player? _player= null;

    /// <summary>
    /// checks wether a seat is still active or not. <br/>
    /// This is the case when the player still has funds or still has cards to participate
    /// </summary>
    /// <remarks>
    /// relevant for several actions in tournament rule games
    /// </remarks>
    /// <returns></returns>
    public bool IsParticipatingGame()
    {
        if (this.Stack.PotValue > 0)
            return true;
        if (PlayerPocketCards.CardCount > 0)
            return true;
        return false;
    }

    /// <summary>
    /// The SeatID is used as a Reference of the Tables Seat Position 
    /// </summary>
    public int SeatID { get; set; }

    /// <summary>
    /// the Table this Seat belongs to
    /// </summary>
    public Table Table { get; private set; }

    /// <summary>
    /// The hand of Cards of the player
    /// </summary>
    public PocketCards PlayerPocketCards = new PocketCards();

    /// <summary>
    /// The SmallBlind must reserve half the minimum bet, even before seeing his Cards
    /// </summary>
    /// <remarks>
    /// the SmallBlind is the Player left of the Dealer. If only 2 players are on the table, the Dealer himself is the smallBlind.
    /// The dealer is always the Master
    /// </remarks>
    public bool IsSmallBlind { get; set; } = false;

    /// <summary>
    /// The SmallBlind must reserve the minimum bet, even before seeing his Cards
    /// </summary>
    /// <remarks>The BigBlind is always the person sitting left of the SmallBlind</remarks>
    public bool IsBigBlind { get; set; } = false;

    /// <summary>
    /// The Dealer starts giving Cards beginning with the Person left of the dealer (The Small Blind).<br/>
    /// If the Table only has 2 active players, the Dealer is at the Same Time the Small Blind ad starts dealing cards to the other player (BigBlind)
    /// </summary>
    /// <remarks>In Casinos, the Dealer does not Handle the Cards but the Casinos Croupier.</remarks>
    public bool IsDealer { get; set; } = false;

    /// <summary>
    /// The stack of Chips in front of the player representing the bet (but not yet added to the tablePot)
    /// </summary>
    public Pot PendingBets = new();

    /// <summary>
    /// Represents a handler for player actions in a poker game. Assign an implementation of <see cref="IPlayerActionHandler"/> 
    /// to manage player actions.
    /// </summary>
    /// <remarks>
    /// Usage:
    /// - Implement <see cref="IPlayerActionHandler"/> in a class with logic for handling player actions.
    /// - Assign an instance of this class to the ActionHandler property of the Seat object.
    /// - The TakeAction method in your class will be invoked to process player actions.
    /// <br/>
    /// Example:
    /// <code>
    /// class MyPlayerActionHandler : IPlayerActionHandler 
    /// {
    ///     public ulong TakeAction(ulong callValue, ulong minRaise, ulong maxRaise) 
    ///     {
    ///         // Implement action logic here
    ///     }
    /// }
    /// // Assigning to a Seat object
    /// seat.ActionHandler = new MyPlayerActionHandler();
    /// </code>
    /// This allows for flexible player action handling in different game environments like simulations or live games.
    /// </remarks>
    public IPlayerActionHandler ActionHandler { get; set; }

    /// <summary>
    /// this method is being used by the game engine to call the player for action
    /// </summary>
    /// <param name="callValue">the minimum bet to make a call</param>
    /// <param name="minRaise">the minimum bet to make a raise</param>
    /// <param name="maxRaise">the maximum bet (in limit games)</param>
    /// <param name="isSimulationMode">if simulation is enabled, runs synchronously, for better performance</param>
    /// <param name="timeout">the timeout after which to cancel player action when in live mode</param>
    /// <returns></returns>
    public ulong? CallForPlayerAction(ulong callValue, ulong minRaise, ulong maxRaise, GameTime isSimulationMode, TimeSpan? timeout = null)
    {
        if (isSimulationMode == GameTime.Simulated)
        {
            // Synchronous call for simulation
            return ActionHandler.TakeAction(callValue, minRaise, maxRaise, CancellationToken.None);
        }
        else
        {
            // Asynchronous call for live game with timeout
            return CallForPlayerAction(callValue, minRaise, maxRaise, timeout).Result;
        }
    }

    private async Task<ulong?> CallForPlayerAction(ulong callValue, ulong minRaise, ulong maxRaise, TimeSpan? timeout)
    {
        CancellationTokenSource cts = timeout.HasValue ? new CancellationTokenSource(timeout.Value) : new CancellationTokenSource();
        try
        {
            return await Task.Run(() => ActionHandler.TakeAction(callValue, minRaise, maxRaise, cts.Token), cts.Token);
        }
        catch (OperationCanceledException)
        {
            cts.Cancel();
            // Handle timeout or cancellation
            return null;
        }
    }
    
    /// <summary>
    /// The stack of Chips in reserve which the Player can use to Bet
    /// </summary>
    public Pot Stack = new();
    private readonly object _lock = new object();
    public ulong StackValue => Stack.PotValue;

    /// <summary>
    /// this function is used to set the Blinds and Ante for examble and cannot be revoked
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public PerformBetResult ForceBet(ulong value)
    {
        return Stack.PerformBet(PendingBets, value - PendingBets.PotValue, this.Player);
    }

    public bool IsAllIn => (Stack.PotValue == 0 && PendingBets.PotValue > 0 && !IsFold);
    public bool IsAllInCall => (Stack.PotValue == 0 && PendingBets.PotValue  > 0 && !IsFold);
    public bool IsFold => PlayerPocketCards.IsFold;

    /// <summary>
    /// defines if and when the player chose to sit out.
    /// this is used for allowing sitting out in cash and Tournament games and for deciding if a player is to be kicked fron the Table
    /// </summary>
    public DateTime? SitOutTime { get; set; } = null;
    /// <summary>
    /// sits out of the table
    /// </summary>
    /// <remarks>
    /// in cash games this will reserve the seat for you without significant penalties<br/>
    /// - after a certain time treshold, You might be kicked out from the Table<br/>
    /// - after Big Blind Passed you, you may have to Bet Big Blind whein returning
    /// <br/><br/>
    /// in tournement games you do not get dealt cards but you continuely have to set blinds and ante until you are eliminated from the tournament<br/>
    /// Staying away for too long does not get you Kicked
    /// </remarks>
    public void SitOut()
    {
        if (SitOutTime != null)
            return;
        SitOutTime = DateTime.Now;
        Table.SeatedPlayersInternal.Remove(this._player);
        if (!IsParticipatingGame())
            Interlocked.Decrement(ref Table.SeatsWithStakesCount);
    }

    public void Leave()
    {
        if (_player == null)
            return;

        if (this.Table.TableGame == null || this.Table.TableGame.Rules.GameMode == GameMode.Cash || !IsParticipatingGame())
        {
            Player.AddPlayerBank(Stack.Clear());
            this.Table.SeatedPlayersInternal.Remove(_player);
            this.Table.TakenSeatsInternal.Remove(this);
            _player.Seat = null;
            this._player = null;
            this.Table.SeatPlayers();
        }
        else if (this.Table.TableGame.Rules.GameMode == GameMode.Tournament)
        {
            SitOut();
        }
        else
        {
            throw new NotImplementedException(
                $"The behaviour on leaving a table under the ruleset {this.Table.TableGame.Rules.GameMode} is not defined!");
        }
    }
    /// <summary>
    /// seats you back into the Table, actively participating in the game again
    /// </summary>
    /// <remarks>This can be used to perform a buyin as well and to sit onto the seat</remarks>
    public SitInResult SitIn(Player player, decimal buyIn = 0)
    {
        // fast precheck
        if (this._player == null)
            this._player = player;
        else if (!this._player.Equals(player))
            throw new InvalidOperationException("The Seat is already occupied by another player!");

        // buyin check
        if (!IsParticipatingGame()) // need to perform buyin
        {
            // buyin prechecks
            if (buyIn == 0)
                return SitInResult.BuyinTooLow;
            if (this._player.Bank < buyIn)
                return SitInResult.NotEnoughFunds;
            if (this.Table.TableGame != null && buyIn > this.Table.TableGame.Rules.MaxBuyIn)
                return SitInResult.BuyinToHigh;
            if (this.Table.TableGame != null && buyIn < this.Table.TableGame.Rules.BuyIn)
                return SitInResult.BuyinTooLow;
            // purchase chips
            ulong chipBuyInValue = (ulong)buyIn;
            if (this.Table.TableGame != null && this.Table.TableGame.Rules.Micro)
            {
                chipBuyInValue = Bank.ConvertMicroToMacro(buyIn);
            }
            this.Stack.AddChips(Chips.Bank.DistributeValueForUse(chipBuyInValue), this._player);
            Interlocked.Increment(ref Table.SeatsWithStakesCount);
        }

        // activate
        SitOutTime = null;
        Table.SeatedPlayersInternal.Add(_player);
        
        return SitInResult.Sucess;
    }
    /// <summary>
    /// Discards the player's hand, essentially skipping the round.
    /// </summary>
    public void Fold()
    {
        this.PlayerPocketCards.Clear();
        Interlocked.Decrement(ref this.Table.PlayersInBettingRoundCount);
    }
}
