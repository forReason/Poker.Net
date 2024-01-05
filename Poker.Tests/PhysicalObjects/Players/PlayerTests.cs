using Xunit;
using Poker.PhysicalObjects.Players;

public class PlayerTests
{
    [Fact]
    public void AddPlayerBank_IncreasesBankAmount()
    {
        // Arrange
        var player = new Player();
        decimal initialBank = player.Bank;
        decimal amountToAdd = 100m;

        // Act
        player.AddPlayerBank(amountToAdd);

        // Assert
        Assert.Equal(initialBank + amountToAdd, player.Bank);
    }

    [Fact]
    public void TryRemovePlayerBank_ReturnsTrueAndDecreasesBank_WhenSufficientFunds()
    {
        // Arrange
        var player = new Player();
        player.AddPlayerBank(100m);
        decimal amountToRemove = 50m;

        // Act
        bool result = player.TryRemovePlayerBank(amountToRemove);

        // Assert
        Assert.True(result);
        Assert.Equal(50m, player.Bank);
    }

    [Fact]
    public void TryRemovePlayerBank_ReturnsFalse_WhenInsufficientFunds()
    {
        // Arrange
        var player = new Player();
        player.AddPlayerBank(30m);
        decimal amountToRemove = 50m;

        // Act
        bool result = player.TryRemovePlayerBank(amountToRemove);

        // Assert
        Assert.False(result);
        Assert.Equal(30m, player.Bank); // Bank should remain unchanged
    }
}