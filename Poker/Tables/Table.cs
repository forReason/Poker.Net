using System.Collections.Concurrent;
using Poker.Chips;
using Poker.Decks;

namespace Poker.Tables;

public partial class Table
{
    public Table(int seats, int buyIn)
    {
        Seats = new Seat[seats];
        for (int i = 0; i < Seats.Length; i++)
        {
            Seats[i] = new Seat(i, this);
        }
        BuyIn = buyIn;
    }
    

    public ConcurrentDictionary<PokerChip, uint> Pot = new ConcurrentDictionary<PokerChip, uint>();
    public int PotValue { get; private set; }

    public Deck TableDeck = new Deck();
    public CommunityCards TableCards = new CommunityCards();

    public Seat[] Seats;

    public int BuyIn { get; private set; }

}