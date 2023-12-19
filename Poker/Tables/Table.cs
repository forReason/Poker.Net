using Poker.Chips;
using Poker.Decks;

namespace Poker.Tables;

public partial class Table
{
    public Table(
        uint seats,
        Games.Game? game
        )
    {
        Seats = new Seat[seats];
        for (int i = 0; i < Seats.Length; i++)
        {
            Seats[i] = new Seat(i, this);
        }
        this.TableGame = game;
    }

    public Games.Game? TableGame { get; set; } = null; 

    /// <summary>
    /// the pots in the center which the players Bet into
    /// </summary>
    public readonly List<Pot> CenterPots = new List<Pot>();

    

    /// <summary>
    /// Calculates the total value of the center pots
    /// </summary>
    /// <returns></returns>
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
    public int ActivePlayers = 0;
}