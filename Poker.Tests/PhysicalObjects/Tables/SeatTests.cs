using Poker.Logic.Blinds;
using Poker.Logic.GameLogic.GameManagement;
using Poker.Logic.GameLogic.Rules;
using Poker.PhysicalObjects.Chips;
using Xunit;
using Poker.PhysicalObjects.Tables;
using Poker.PhysicalObjects.Players;

namespace Poker.Tests.PhysicalObjects.Seats;

public class SeatTests
{
    [Fact]
    public void IsParticipatingGame_ReturnsTrue_WhenPlayerHasFundsOrCards()
    {
        // Arrange
        var table = new Table(6, null);
        var seat = new Seat(1, table);
        var player = new Player();
        player.AddPlayerBank(10);
        seat.SitIn(player, 10);

        // Act
        bool result = seat.IsParticipatingGame;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ForceBet_SubtractsFromStack_AddsToPendingBets()
    {
        // Arrange
        var table = new Table(6, null);
        var seat = new Seat(1, table);
        var player = new Player();
        player.AddPlayerBank(10);
        seat.SitIn(player, 10);
        ulong initialStackValue = seat.StackValue;

        // Act
        seat.ForceBet(5);

        // Assert
        Assert.Equal(initialStackValue - 5, seat.StackValue);
        Assert.Equal(5UL, seat.PendingBets.PotValue);
    }

    [Fact]
    public void SitOut_SetsSitOutTime_DecrementsTablePlayerCount()
    {
        // Arrange
        var table = new Table(6, null);
        var seat = new Seat(1, table);
        var player = new Player();
        player.AddPlayerBank(100);
        seat.SitIn(player,100);
        int initialPlayerCount = table.SeatedPlayersCount;

        // Act
        seat.SitOut();

        // Assert
        Assert.NotNull(seat.SitOutTime);
        Assert.Equal(initialPlayerCount - 1, table.SeatedPlayersCount);
    }

    [Fact]
    public void SitIn_IncreasesTablePlayerCount_SetsSitOutTimeNull()
    {
        // Arrange
        RuleSet rules = new RuleSet(
            GameMode.Cash, 
            buyIn:100,
            BlindToBuyInRatio.OneToTwentyFive,
            maxBuyInRatio:2
            );
        Game game = new Game(rules);
        var table = new Table(6, game);
        var seat = new Seat(1, table);
        var player = new Player();
        player.AddPlayerBank(1000);
        seat.SitIn(player, 100);
        seat.SitOut();
        int initialPlayerCount = table.SeatedPlayersCount;

        // Act
        SitInResult result = seat.SitIn(player, 100); // Assuming player has sufficient bank for buy-in

        // Assert
        Assert.Equal(result, SitInResult.Success);
        Assert.Null(seat.SitOutTime);
        Assert.Equal(initialPlayerCount+1, table.SeatedPlayersCount);
    }

    // Additional tests for Fold, CallForPlayerAction, NotifyPlayer, etc.
}
