using Poker.Chips;
using Poker.Decks;

namespace Poker.Tables;

public partial class Table
{
    public Table(int seats, int buyIn, TimeSpan? increaseBlind  )
    {
        Seats = new Seat[seats];
        for (int i = 0; i < Seats.Length; i++)
        {
            Seats[i] = new Seat(i, this);
        }
        BuyIn = buyIn;
    }
    

    public readonly List<Pot> CenterPots = new List<Pot>();
    public ulong GetTotalPotValue()
        {
            ulong value = 0;
            foreach (Pot pot in CenterPots)
            {
                value += pot.GetValue();
            }
        return value;
        }

    public Deck TableDeck = new Deck();
    public CommunityCards TableCards = new CommunityCards();
    public Seat[] Seats;
    public int BuyIn { get; private set; }

}