using Poker.Chips;
using Poker.Players;

namespace Poker.Tests.Chips;
public class PotTests
{
    [Fact]
    public void AddChips_IncreasesTotalValue()
    {
        var pot = new Pot();
        var player = new Player(); // Assuming a Player class exists
        var chipsToAdd = new Dictionary<PokerChip, ulong> { { PokerChip.Blue, 10 } };

        pot.AddChips(chipsToAdd, player);

        Assert.Equal(500UL, pot.PotValue); // Blue chips are worth 50 each
    }

    [Fact]
    public void RemoveValue_DecreasesTotalValue()
    {
        var pot = new Pot();
        var player = new Player();
        pot.AddChips(new Dictionary<PokerChip, ulong> { { PokerChip.Blue, 10 } }, player);

        pot.RemoveValue(76);

        Assert.Equal(424UL, pot.PotValue); // 509 - 76
    }

    [Fact]
    public void RemoveValue_ReturnsCorrectChips()
    {
        var pot = new Pot();
        var player = new Player();
        pot.AddChips(new Dictionary<PokerChip, ulong> { { PokerChip.Blue, 10 } }, player);

        var removedChips = pot.RemoveValue(100);

        Assert.True(removedChips.ContainsKey(PokerChip.Blue) && removedChips[PokerChip.Blue] == 2);
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
