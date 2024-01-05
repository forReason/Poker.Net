using Poker.PhysicalObjects.Players;
using Poker.PhysicalObjects.Tables;

namespace Poker.Tests.PhysicalObjects.Tables;

public class example
{
    [Fact]
    public void RunGame()
    {
        Table myTable = new Table(seats: 6, null);

        Player julian = new Player();
        julian.UniqueIdentifier = "Julian";
        
        myTable.Enqueue(julian, preferredSeat: 2);
        
        myTable.TableDeck.ShuffleCards();
        myTable.DealPlayerCards();
        myTable.CommunityCards.RevealAll(myTable.TableDeck);
        
        {}
        
    }
}