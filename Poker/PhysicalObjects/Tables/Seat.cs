
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
    /// <param name="seatID">the seat number at the table</param>
    /// <param name="table">the table where this seat belongs to for backreference</param>
    public Seat(int seatID, Table table)
    {
        SeatID = seatID;
        Table = table;
    }

    /// <summary>
    /// The Player sitting on this Seat. If it's null, the seat is Free
    /// </summary>

    public Player? Player => _player;
    private volatile Player? _player;

    /// <summary>
    /// Checks whether a seat is still active in the game. <br/>
    /// A seat is considered active if the player still has funds or cards to participate.
    /// </summary>
    /// <remarks>
    /// This property is relevant for determining active participants in tournament rule games.
    /// </remarks>
    /// <returns>
    /// Returns true if the player's stack has a positive pot value or if the player has more than zero pocket cards.
    /// </returns>
    public bool IsParticipatingGame => this.Stack.PotValue > 0 || PlayerPocketCards.CardCount > 0;

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
    public readonly PocketCards PlayerPocketCards = new PocketCards();

    /// <summary>
    /// The SmallBlind must reserve half the minimum bet, even before seeing his Cards
    /// </summary>
    /// <remarks>
    /// the SmallBlind is the Player left of the Dealer. If only 2 players are on the table, the Dealer himself is the smallBlind.
    /// The dealer is always the Master
    /// </remarks>
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public bool IsSmallBlind { get; set; }

    /// <summary>
    /// The SmallBlind must reserve the minimum bet, even before seeing his Cards
    /// </summary>
    /// <remarks>The BigBlind is always the person sitting left of the SmallBlind</remarks>
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public bool IsBigBlind { get; set; }

    /// <summary>
    /// The Dealer starts giving Cards beginning with the Person left of the dealer (The Small Blind).<br/>
    /// If the Table only has 2 active players, the Dealer is at the Same Time the Small Blind ad starts dealing cards to the other player (BigBlind)
    /// </summary>
    /// <remarks>In Casinos, the Dealer does not Handle the Cards but the Casinos Croupier.</remarks>
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public bool IsDealer { get; set; }

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
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public IPlayerActionHandler? ActionHandler { get; set; }

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
        if (ActionHandler == null)
            return 0;
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
        if (ActionHandler == null)
            return 0;
        CancellationTokenSource cts = timeout.HasValue ? new CancellationTokenSource(timeout.Value) : new CancellationTokenSource();
        try
        {
            return await Task.Run(() => ActionHandler.TakeAction(callValue, minRaise, maxRaise, cts.Token), cts.Token);
        }
        catch (OperationCanceledException)
        {
            await cts.CancelAsync();
            // Handle timeout or cancellation
            return null;
        }
    }
    
    /// <summary>
    /// The stack of Chips in reserve which the Player can use to Bet
    /// </summary>
    public readonly Pot Stack = new();
    public ulong StackValue => Stack.PotValue;

    /// <summary>
    /// this function is used to set the Blinds and Ante for example and cannot be revoked
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public PerformBetResult ForceBet(ulong value)
    {
        if (this._player == null && PendingBets.Players.Count > 0)
        {
            return Stack.PerformBet(PendingBets, value - PendingBets.PotValue, PendingBets.Players.First());
        }
        else if (this._player == null)
            return Stack.PerformBet(PendingBets, value - PendingBets.PotValue, new Player());
        return Stack.PerformBet(PendingBets, value - PendingBets.PotValue, this.Player!);
    }

    /// <summary>
    /// returns true if the player has no funds left but has cards on hand
    /// </summary>
    /// <remarks>returns false if the player has no funds an no cards (in which case the player is game over)</remarks>
    public bool IsAllIn => (Stack.PotValue == 0 && this.PlayerPocketCards.HasCards);

    /// <summary>
    /// defines if and when the player chose to sit out.
    /// this is used for allowing sitting out in cash and Tournament games and for deciding if a player is to be kicked from the Table
    /// </summary>
    public DateTime? SitOutTime { get; set; } 
    /// <summary>
    /// sits out of the table
    /// </summary>
    /// <remarks>
    /// in cash games this will reserve the seat for you without significant penalties<br/>
    /// - after a certain time threshold, You might be kicked out from the Table<br/>
    /// - after Big Blind Passed you, you may have to Bet Big Blind when returning
    /// <br/><br/>
    /// in tournament games you do not get dealt cards but you continually have to set blinds and ante until you are eliminated from the tournament<br/>
    /// Staying away for too long does not get you Kicked
    /// </remarks>
    public void SitOut()
    {
        if (this._player == null || SitOutTime != null)
            return;
        SitOutTime = DateTime.Now;
        
        Table.SeatedPlayersInternal.Remove(this._player);
        if (!IsParticipatingGame)
            Interlocked.Decrement(ref Table.SeatsWithStakesCount);
    }

    /// <summary>
    /// Leaves the game table
    /// </summary>
    /// <remarks>
    /// in a cash game, withdraws all funds from the stash into the player bank and leaves the table<br/>
    /// in a tournament game, leaves all funds at the table and sits out, unless the player has no funds, in which case
    /// the player Leaves the Table
    /// </remarks>
    /// <exception cref="NotImplementedException"></exception>
    public void Leave()
    {
        if (_player == null)
            return;

        if (this.Table.TableGame == null || this.Table.TableGame.Rules.GameMode == GameMode.Cash || !IsParticipatingGame)
        {
            _player.AddPlayerBank(Stack.Clear());
            this.Table.SeatedPlayersInternal.Remove(_player);
            this.Table.TakenSeatsInternal.Remove(this);
            _player.Seat = null;
            this._player = null;
            this.Table.SeatPlayers();
            this.BuyInCount = 0;
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

        // Perform buyin if not participating
        if (!IsParticipatingGame)
        {
            var buyInResult = PerformBuyIn(buyIn);
            if (buyInResult != SitInResult.Success)
                return buyInResult;
        }

        // activate
        SitOutTime = null;
        Table.SeatedPlayersInternal.Add(_player);

        return SitInResult.Success;
    }

    /// <summary>
    /// Performs a buyin
    /// </summary>
    /// <remarks>The player Needs to sit on the Table first. It's recommended to use <see cref="SitIn"/> as it performs
    /// a buyin as well if a buyin amount is set.
    /// </remarks>
    /// <param name="buyIn"></param>
    /// <returns></returns>
    private SitInResult PerformBuyIn(decimal buyIn)
    {
        // buyin prechecks
        if (buyIn == 0)
            return SitInResult.BuyinTooLow;
        if (_player == null)
            throw new InvalidOperationException("A Player needs to be sitting in order to perform a buyin! Use SitIn() instead please!");
        if (_player.Bank < buyIn)
            return SitInResult.NotEnoughFunds;
        if (this.Table.TableGame != null)
        {
            // checks which can only be performed on an active game
            if (buyIn > this.Table.TableGame.Rules.MaxBuyIn)
                return SitInResult.BuyinToHigh;
            if (buyIn < this.Table.TableGame.Rules.BuyIn)
                return SitInResult.BuyinTooLow;
            if (this.Table.TableGame.Rules.MaxBuyInCount > 0 && BuyInCount >= this.Table.TableGame.Rules.MaxBuyInCount)
                return SitInResult.MaxBuyinCounterReached;
            if (this.Table.TableGame.Rules.LatestAllowedBuyIn != null && this.Table.TableGame.GameLength != null &&
                this.Table.TableGame.GameLength > this.Table.TableGame.Rules.LatestAllowedBuyIn)
                return SitInResult.BuyinIsCurrentlyNotAllowed;
        }
        

        // purchase chips
        ulong chipBuyInValue = (ulong)buyIn;
        if (this.Table.TableGame != null && this.Table.TableGame.Rules.Micro)
        {
            chipBuyInValue = Bank.ConvertMicroToMacro(buyIn);
        }
        this.Stack.AddChips(Bank.DistributeValueForUse(chipBuyInValue), _player);
        Interlocked.Increment(ref Table.SeatsWithStakesCount);

        BuyInCount++;
        return SitInResult.Success;
    }

    /// <summary>
    /// the number of buyins performed by the player. This is mostly used for Tournament rules or analysis
    /// </summary>
    public ulong BuyInCount { get; private set; } 

    /// <summary>
    /// Discards the player's hand, essentially skipping the round.
    /// </summary>
    public void Fold()
    {
        this.PlayerPocketCards.Clear();
        Interlocked.Decrement(ref this.Table.PlayersInBettingRoundCount);
    }
}
