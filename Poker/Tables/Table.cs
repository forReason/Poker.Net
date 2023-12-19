using Poker.Blinds;
using Poker.Chips;
using Poker.Decks;

namespace Poker.Tables;

public partial class Table
{
    public Table(
        uint seats, 
        BettingStructure structure,
        uint minimumPlayers = 2
        
        )
    {
        Seats = new Seat[seats];
        for (int i = 0; i < Seats.Length; i++)
        {
            Seats[i] = new Seat(i, this);
        }

        this.MinimumPlayerCount = minimumPlayers;
        this.BettingStructure = structure;
    }
    
    
    /// <summary>
    /// the amount of players when the game starts
    /// </summary>
    public uint MinimumPlayerCount { get; private set; }

    /// <summary>
    /// The timespan to wait before the game starts when the starting conditions are met
    /// </summary>
    public TimeSpan StartDelay { get; set; } = TimeSpan.FromSeconds(10);

    /// <summary>
    /// the pots in the center which the players Bet into
    /// </summary>
    public readonly List<Pot> CenterPots = new List<Pot>();

    public BettingStructure BettingStructure { get; set; }

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
    public ulong BuyIn { get; private set; }

}