using System.Data;
using Poker.Logic.Blinds;
using Poker.Logic.GameLogic.GameManagement;
using Poker.Logic.GameLogic.Rules;
using Xunit;
using Poker.PhysicalObjects.Tables;
using Poker.PhysicalObjects.Players;
using Poker.PhysicalObjects.Chips;

namespace Poker.Tests.PhysicalObjects.Tables;
public class TableQueueTests
{
    [Fact]
    public void MoveButtons_MovesDealerSmallBlindBigBlind_Successfully()
    {
        // Arrange
        var table = new Table(5, null);

        for (int i = 0; i < 3; i++)
        {
            Player player = new();
            player.AddPlayerBank(100);
            table.Enqueue(player);
            player.Seat.SitIn(player, 100);
        }
        // Assume initial positions
        table.DealerSeat = 0;
        table.SmallBlindSeat = 1;
        table.BigBlindSeat = 2;

        // Act
        table.MoveButtons();

        // Assert - Assuming GetNextActiveSeat works correctly
        Assert.Equal(1, table.DealerSeat);
        Assert.Equal(2, table.SmallBlindSeat);
        Assert.Equal(0, table.BigBlindSeat);
    }

    [Fact]
    public void DealPlayerCards_DealsTwoCardsToEachPlayer()
    {
        // Arrange
        var table = new Table(5, null);
        // Add players to the table
        for (int i = 0; i < 5; i++)
        {
            table.Seats[i].SitIn(new Player());
        }

        // Act
        table.DealPlayerCards();

        // Assert
        foreach (var seat in table.Seats)
        {
            Assert.Equal(2, seat.PlayerPocketCards.CardCount);
        }
    }

    [Fact]
    public void CheckAllPlayersAllIn_ReturnsTrue_WhenAllActivePlayersAllIn()
    {
        // Arrange
        var table = new Table(5, null);
        // Add players
        for (int i = 0; i < 5; i++)
        {
            var player = new Player();
            player.AddPlayerBank(100);
            SitInResult sitInResult = table.Seats[i].SitIn(player, 100);
            Assert.Equal(sitInResult, SitInResult.Success);
            table.Seats[i].SitIn(player);
        }
        // Deal player Cards
        table.DealPlayerCards();
        // set Bets
        for (int i = 0; i < 5; i++)
        {
            table.Seats[i].ForceBet(100);
        }

        // Act
        bool result = table.CheckAllPlayersAllIn();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CheckBetsAreAllEqual_ReturnsTrue_WhenAllBetsEqualOrPlayersAllIn()
    {
        // Arrange
        var table = new Table(5, null);
        // Add players and set equal bets or all-in
        ulong betValue = 50;
        for (int i = 0; i < 5; i++)
        {
            var player = new Player();
            table.Seats[i].SitIn(player);
            table.Seats[i].PendingBets = new Pot();
            table.Seats[i].PendingBets.AddChips(new Dictionary<PokerChip, ulong> { { PokerChip.White, betValue } }, player);
            betValue = (i % 2 == 0) ? betValue : 0; // Alternate bet values
        }

        // Act
        bool result = table.CheckBetsAreAllEqual();

        // Assert
        Assert.True(result);
    }

    // Additional tests for GetNextBettingSeat, GetNextActiveSeat, GetPreviousActiveSeat, etc.
}
