using Poker.Chips;
using Poker.Decks;
using Poker.Players;

namespace Poker.Tables
{
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
        public volatile Player? Player = null;

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
            if (this.Stack != null && this.Stack.PotValue > 0)
                return true;
            if (this.UncalledPendingBets != null && this.UncalledPendingBets.PotValue > 0)
                return true;
            if (PlayerPocketCards != null && PlayerPocketCards.CardCount > 0)
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
        public Pot PendingBets = new ();

        /// <summary>
        /// the stack of chips a player may have reserved for calling and raising. Used to perform the bet according to player action
        /// </summary>
        public Pot UncalledPendingBets = new();

        /// <summary>
        /// The stack of Chips in reserve which the Player can use to Bet
        /// </summary>
        public Pot Stack = new ();
        private readonly object _lock = new object ();
        public ulong StackValue => Stack.PotValue + UncalledPendingBets.PotValue;

        /// <summary>
        /// this function is used to set the Blinds and Ante for examble and cannot be revoked
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public PerformBetResult ForceBet(ulong value)
        {
            Pot temporaryPot = new Pot();
            PerformBetResult betResult = UncalledPendingBets.PerformBet(temporaryPot, value, this.Player);
            if (betResult == PerformBetResult.PlayerHasNoFunds || betResult == PerformBetResult.AllIn)
            {
                PerformBetResult stackBetResult = Stack.PerformBet(temporaryPot, value - temporaryPot.PotValue, this.Player);
            }
            return temporaryPot.PerformBet(PendingBets, value, this.Player);
        }
        /// <summary>
        /// moves all uncomitted Chips to the player bet which is locked up
        /// </summary>
        public void CommitUncalledPendingBets()
        {
            UncalledPendingBets.MoveAllChips(PendingBets, this.Player);
        }
        /// <summary>
        /// used by a player in order to wager a certain amount. At this point the money is not comitted yet
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public PerformBetResult MoveStackToUncalledPendingBets(ulong value)
        {

            return Stack.PerformBet(UncalledPendingBets, value, this.Player);
        }

        public bool IsAllIn => (Stack.PotValue == 0 && PendingBets.PotValue > 0 && UncalledPendingBets.PotValue == 0 && !IsFold);
        public bool IsAllInCall => (Stack.PotValue == 0 && (PendingBets.PotValue + UncalledPendingBets.PotValue) > 0 && !IsFold);
        public bool IsFold => PlayerPocketCards.IsFold;
        
        /// <summary>
        /// defines if and when the player chose to sit out.
        /// this is used for allowing sitting out in cash and Tournament games and for deciding if a player is to be kicked fron the Table
        /// </summary>
        public DateTime? SitOutTime { get; set; }
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
            Interlocked.Decrement(ref Table.SeatedPlayersCount);
            if (!IsParticipatingGame())
                Interlocked.Decrement(ref Table.SeatsWithStakesCount);

        }
        /// <summary>
        /// seats you back into the Table, actively participating in the game again
        /// </summary>
        public SitInResult SitIn(decimal buyIn = 0)
        {
            // fast precheck
            if (SitOutTime == null)
                return SitInResult.Sucess;
            
            // buyin check
            if (!IsParticipatingGame()) // need to perform buyin
            {
                // buyin prechecks
                if (buyIn == 0)
                    return SitInResult.BuyinTooLow;
                if (this.Player.Bank < buyIn )
                    return SitInResult.NotEnoughFunds;
                if (buyIn > this.Table.TableGame.Rules.MaxBuyIn)
                    return SitInResult.BuyinToHigh;
                if (buyIn < this.Table.TableGame.Rules.BuyIn)
                    return SitInResult.BuyinTooLow;
                // purchase chips
                ulong chipBuyInValue = (ulong)buyIn;
                if (this.Table.TableGame.Rules.Micro)
                {
                    chipBuyInValue = Bank.ConvertMicroToMacro(buyIn);
                }
                this.Stack.AddChips(Chips.Bank.DistributeValueForUse(chipBuyInValue), this.Player);
            }
            
            // activate
            SitOutTime = null;
            Interlocked.Increment(ref Table.SeatedPlayersCount);
            Interlocked.Increment(ref Table.SeatsWithStakesCount);
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
}
