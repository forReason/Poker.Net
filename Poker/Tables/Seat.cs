using Poker.Chips;
using Poker.Players;
using System.Collections.Concurrent;

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
        /// The SeatID is used as a Reference of the Tables Seat Position 
        /// </summary>
        public int SeatID { get; set; }

        /// <summary>
        /// the Table this Seat belongs to
        /// </summary>
        public Table Table { get; private set; }

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
    }
}
