using System.Collections.Concurrent;
using Poker.Chips;
using Poker.Decks;

namespace Poker;

public class Table
{
    public Table(int seats, int buyIn)
    {
        this.Seats = new Player?[seats];
        this.BuyIn = buyIn;
    }
    /// <summary>
    /// the Count of Seats Available
    /// </summary>
    public int TakenSeats { get; private set; } = 0;
    
    /// <summary>
    /// the count of Seats which are still Free
    /// </summary>
    public int FreeSeats => Seats.Length - TakenSeats;

    public ConcurrentBag<PokerChip> Pot = new ConcurrentBag<PokerChip>();
    public int PotValue { get; private set; }

    public Deck TableDeck = new Deck();
    public SharedCards TableCards = new SharedCards();

    public Player?[] Seats;
    
    public int BuyIn { get; private set; }
}