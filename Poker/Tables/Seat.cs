using Poker.Chips;
using Poker.Decks;
using Poker.Games;
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
        /// checks wether a seat is still active or not. This is the case when there are coins in the bank/pot or the player has cards
        /// </summary>
        /// <remarks>
        /// relevant for several actions in tournament rule games
        /// </remarks>
        /// <returns></returns>
        public bool IsActive()
        {
            if (this.BankChips != null && this.BankChips.GetValue() > 0)
                return true;
            if (this.Bet != null && this.Bet.GetValue() > 0)
                return true;
            if (PlayerHand != null && PlayerHand.CardCount > 0)
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
        public Hand PlayerHand = new Hand();

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
        /// The stack of Chips in front of the player representing the bet
        /// </summary>
        public Pot Bet = new ();

        /// <summary>
        /// The stack of Chips in reserve which the Player can use to Bet
        /// </summary>
        public Pot BankChips = new ();

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
            Interlocked.Decrement(ref Table.ActivePlayers);
            if (!IsActive())
                Interlocked.Decrement(ref Table.ActiveSeats);
        }
        /// <summary>
        /// seats you back into the Table, actively participating in the game again
        /// </summary>
        public SitInResult SitIn(ulong buyIn = 0)
        {
            // fast precheck
            if (SitOutTime == null)
                return SitInResult.Sucess;
            
            // buyin check
            if (!IsActive()) // need to perform buyin
            {
                // buyin prechecks
                if (buyIn == 0)
                    return SitInResult.BuyinTooLow;
                if (this.Player.Bank < buyIn )
                    return SitInResult.NotEnoughFunds;
                if (buyIn > this.Table.TableGame.BettingStructure.MaxBuyIn)
                    return SitInResult.BuyinToHigh;
                if (buyIn < this.Table.TableGame.BettingStructure.BuyIn)
                    return SitInResult.BuyinTooLow;
                // purchase chips
                this.BankChips.AddChips(Chips.Bank.DistributeValueForUse(buyIn), this.Player);
            }
            
            // activate
            SitOutTime = null;
            Interlocked.Increment(ref Table.ActivePlayers);
            Interlocked.Increment(ref Table.ActiveSeats);
            return SitInResult.Sucess;
        }
    }
}
