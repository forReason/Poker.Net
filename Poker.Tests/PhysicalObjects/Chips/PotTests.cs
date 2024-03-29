
using Poker.Net.PhysicalObjects.Chips;
using Poker.Net.PhysicalObjects.Players;
using System.Linq;

namespace Poker.Tests.PhysicalObjects.Chips;
public class PotTests
{
    [Fact]
    public void GetChips_ReturnsChips()
    {
        // Arrange
        var pot = new Pot();
        var player = new Player(); // Assuming a Player class exists
        var chipsToAdd = new Dictionary<PokerChip, ulong> { { PokerChip.Blue, 10 }, { PokerChip.Red, 5 } };

        // Act
        pot.AddChips(chipsToAdd, player);
        var chipsInPot = pot.GetChips();

        // Assert
        Assert.Equal(chipsToAdd.Count, chipsInPot.Count); // Check if count matches
        foreach (var chip in chipsToAdd)
        {
            Assert.True(chipsInPot.ContainsKey(chip.Key)); // Check if each chip type is present
            Assert.Equal(chip.Value, chipsInPot[chip.Key]); // Check if each chip count is correct
        }
        Assert.Equal(Bank.ConvertChipsToValue(chipsToAdd), Bank.ConvertChipsToValue(chipsInPot)); // Check if the pot value is correct                                                                                                                      // Check if the pot value is correct
        Assert.Contains(player, pot.Players); // Check if the player is registered in the pot
    }

    [Fact]
    public void GetSortedChips_ReturnsChipsInDescendingOrder()
    {
        // Arrange
        var pot = new Pot();
        var chipsToAdd = new Dictionary<PokerChip, ulong>
        {
            { PokerChip.Blue, 10 },
            { PokerChip.Red, 5 },
            { PokerChip.Green, 15 }
        };
        pot.AddChips(chipsToAdd, new Player());

        // Act
        var sortedChips = pot.GetSortedChips().ToList();

        // Assert
        Assert.Equal(3, sortedChips.Count);
        Assert.True(sortedChips[0].Key > sortedChips[1].Key && sortedChips[1].Key > sortedChips[2].Key);
    }

    [Fact]
    public void GetSortedChips_ReturnsChipsInAscendingOrder()
    {
        // Arrange
        var pot = new Pot();
        var chipsToAdd = new Dictionary<PokerChip, ulong>
        {
            { PokerChip.Blue, 10 },
            { PokerChip.Red, 5 },
            { PokerChip.Green, 15 }
        };
        pot.AddChips(chipsToAdd, new Player());

        // Act
        var sortedChips = pot.GetSortedChips(reverse: true).ToList();

        // Assert
        Assert.Equal(3, sortedChips.Count);
        Assert.True(sortedChips[0].Key < sortedChips[1].Key && sortedChips[1].Key < sortedChips[2].Key);
    }

    [Fact]
    public void AddChips_IncreasesTotalValue()
    {
        var pot = new Pot();
        var player = new Player(); // Assuming a Player class exists
        var chipsToAdd = new Dictionary<PokerChip, ulong> { { PokerChip.Blue, 10 } };

        pot.AddChips(chipsToAdd, player);

        Assert.Equal(500UL, pot.StackValue); // Blue chips are worth 50 each
    }

    [Fact]
    public void RemoveValue_DecreasesTotalValue()
    {
        var pot = new Pot();
        var player = new Player();
        pot.AddChips(new Dictionary<PokerChip, ulong> { { PokerChip.Blue, 10 } }, player);

        pot.RemoveValue(76);

        Assert.Equal(424UL, pot.StackValue); // 509 - 76
    }

    [Fact]
    public void RemoveValue_ReturnsCorrectChips()
    {
        var pot = new Pot();
        var player = new Player();
        pot.AddChips(new Dictionary<PokerChip, ulong> { { PokerChip.Blue, 10 } }, player);

        var removedChips = pot.RemoveValue(100);

        Assert.True(removedChips.GetChips().ContainsKey(PokerChip.Blue) && removedChips.GetChips()[PokerChip.Blue] == 2);
    }

    [Fact]
    public void RemoveAllChips_ClearsPotAndReturnsAllChips()
    {
        // Arrange
        var pot = new Pot();
        var player = new Player(); // Assuming a Player class exists
        var chipsToAdd = new Dictionary<PokerChip, ulong> { { PokerChip.Blue, 10 }, { PokerChip.Red, 5 } };
        pot.AddChips(chipsToAdd, player);

        // Act
        var removedChips = pot.RemoveAllChips();

        // Assert
        Assert.Equal(chipsToAdd, removedChips.GetChips()); // Check if all chips were correctly returned
        Assert.Empty(pot.GetChips()); // Ensure the pot is empty
        Assert.Equal(0UL, pot.StackValue); // Ensure the pot value is zero
    }

    [Fact]
    public void MoveAllChips_MovesAllChipsToTargetPot()
    {
        // Arrange
        var sourcePot = new Pot();
        var targetPot = new Pot();
        var player = new Player();
        var chipsToAdd = new Dictionary<PokerChip, ulong> { { PokerChip.Blue, 10 }, { PokerChip.Red, 5 } };
        sourcePot.AddChips(chipsToAdd, player);

        // Act
        sourcePot.MoveAllChips(targetPot, player);

        // Assert
        Assert.Empty(sourcePot.GetChips());
        Assert.Equal(chipsToAdd, targetPot.GetChips());
    }

    [Fact]
    public void MoveValue_MovesSpecifiedValueToTargetPot()
    {
        // Arrange
        var sourcePot = new Pot();
        var targetPot = new Pot();
        var player = new Player();
        sourcePot.AddChips(new Dictionary<PokerChip, ulong> { { PokerChip.Brown, 1 } }, player); // 10
        ulong valueToMove = 5; // Assuming Blue chip is worth 1 each

        // Act
        bool result = sourcePot.MoveValue(targetPot, valueToMove, player);

        // Assert
        Assert.True(result);
        Assert.Equal(5UL, sourcePot.StackValue); // Check remaining value in source
        Assert.Equal(valueToMove, targetPot.StackValue); // Check value moved to target
    }

    [Fact]
    public void Recolorize_AdjustsChipDenominations()
    {
        // Arrange
        var pot = new Pot();
        var player = new Player();
        pot.AddChips(new Dictionary<PokerChip, ulong> { { PokerChip.Blue, 10 } }, player); // 10 cips * 50 = 500

        ulong valueBefore = pot.StackValue;
        // Act
        pot.Recolorize();
        var chipsAfterRecolor = pot.GetChips();

        // Assert
        Assert.Equal(valueBefore, pot.StackValue);
        Assert.True(pot.GetChips().Count > 1);

    }

    [Fact]
    public void RemoveChips_RemovesSpecifiedChipsFromPot()
    {
        // Arrange
        var pot = new Pot();
        pot.AddChips(new Dictionary<PokerChip, ulong> { { PokerChip.Blue, 10 } }, new Player());
        var chipsToRemove = new Dictionary<PokerChip, ulong> { { PokerChip.Blue, 5 } };

        // Act
        var removedValue = pot.RemoveChips(chipsToRemove);

        // Assert
        Assert.Equal(250UL, removedValue.StackValue); // Blue chip is worth 50 each
        Assert.Equal(250UL, pot.StackValue); // Remaining value in the pot
    }



    [Fact]
    public void Clear_ResetsPot()
    {
        var pot = new Pot();
        var player = new Player();
        pot.AddChips(new Dictionary<PokerChip, ulong> { { PokerChip.Blue, 10 } }, player);

        ulong clearedValue = pot.Clear();

        Assert.Equal(500UL, clearedValue);
        Assert.Empty(pot.GetChips());
    }

    [Fact]
    public void Players_AreCorrectlyAdded()
    {
        var pot = new Pot();
        var player1 = new Player();
        var player2 = new Player();

        pot.AddChips(new Dictionary<PokerChip, ulong> { { PokerChip.Red, 3 } }, player1);
        pot.AddChips(new Dictionary<PokerChip, ulong> { { PokerChip.Blue, 2 } }, player2);

        Assert.Contains(player1, pot.Players);
        Assert.Contains(player2, pot.Players);
    }

    // Additional tests can be added for thread safety, boundary cases, etc.
}
