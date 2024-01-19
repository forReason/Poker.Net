using Xunit;
using Poker.Net.PhysicalObjects.Players;
using Poker.Net.PhysicalObjects.Tables;

namespace Poker.Tests.PhysicalObjects.Tables;
public class TableTests
{
    [Fact]
    public void Enqueue_AddsPlayerToQueue_Successfully()
    {
        // Arrange
        var table = new Table(5, null);
        var player = new Player();

        // Act
        table.Enqueue(player);

        // Assert
        Assert.Equal(1, table.EnqueuedPlayers.Count);
    }

    [Fact]
    public void CancelEnrollment_RemovesPlayerFromQueue_Successfully()
    {
        // Arrange
        var table = new Table(3, null);
        for (int i = 0; i < 3; i++)
        {
            table.Enqueue(new Player());
        }
        var player = new Player();
        table.Enqueue(player, preferredSeat:2);

        // Act
        table.CancelEnrollment(player);

        // Assert
        int seatPreference;
        table.EnqueuedPlayers.TryGetValue(player.UniqueIdentifier, out seatPreference);
        Assert.Equal(-1, seatPreference);
    }

    [Fact]
    public void SeatPlayers_SeatsPlayerToPreferredSeat_WhenAvailable()
    {
        // Arrange
        var table = new Table(5, null);
        var player = new Player();
        int preferredSeat = 3;
        table.Enqueue(player, preferredSeat);

        // Act
        // Assuming SeatPlayers is called within Enqueue
        var seatedPlayer = table.Seats[preferredSeat].Player;

        // Assert
        Assert.Equal(player, seatedPlayer);
    }

    [Fact]
    public void LeaveTable_RemovesPlayerFromSeat_Successfully()
    {
        // Arrange
        var table = new Table(5, null);
        var player = new Player();
        table.Enqueue(player); // Assume player is seated at Seat 0
        int seatId = 0;

        // Act
        player.Seat.Leave();

        // Assert
        Assert.Null(table.Seats[seatId].Player);
    }

    // Additional tests can be written for TryTakeSeat, TryTakeAnySeat, etc.
}