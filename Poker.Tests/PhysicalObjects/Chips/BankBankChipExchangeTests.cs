
using Poker.PhysicalObjects.Chips;

namespace Poker.Tests.PhysicalObjects.Chips;
public class BankBankChipExchangeTests
{
    [Fact]
    public void TestRecolorize()
    {
        var chips = new Dictionary<PokerChip, ulong>
        {
            { PokerChip.White, 10 },  // $10
            { PokerChip.Red, 3 },      // $15
            { PokerChip.Pink , 1} // 1000
        };
        ulong originalValue = Bank.ConvertChipsToValue(chips);
        var result = Bank.Recolorize(chips);
        ulong recoloredValue = Bank.ConvertChipsToValue(result);
        Assert.Equal(originalValue, recoloredValue);  // Expect to get the same amount back
        Assert.True(result.Count > 5);
    }

    [Fact]
    public void TestExchangeChipsForSmallerDenominations()
    {
        var result = Bank.ExchangeChipsForSmallerDenominations(PokerChip.Black, 1); // $100

        Assert.Equal(2UL, result[PokerChip.Blue]);  // $50
        Assert.DoesNotContain(PokerChip.Black, result.Keys);
    }
    [Fact]
    public void MergeStacks_CombinesTwoStacksCorrectly()
    {
        // Arrange
        var mainStack = new Dictionary<PokerChip, ulong>
        {
            { PokerChip.Red, 10 },
            { PokerChip.Green, 5 }
        };
        var valuesToAdd = new Dictionary<PokerChip, ulong>
        {
            { PokerChip.Red, 5 },
            { PokerChip.Blue, 3 }
        };
        var expectedStack = new Dictionary<PokerChip, ulong>
        {
            { PokerChip.Red, 15 },
            { PokerChip.Green, 5 },
            { PokerChip.Blue, 3 }
        };

        // Act
        Bank.MergeStacks(mainStack, valuesToAdd);

        // Assert
        Assert.Equal(expectedStack, mainStack);
    }

    [Fact]
    public void MergeStacks_AddsNewValuesIfNotPresent()
    {
        // Arrange
        var mainStack = new Dictionary<PokerChip, ulong> { { PokerChip.Red, 10 } };
        var valuesToAdd = new Dictionary<PokerChip, ulong> { { PokerChip.Blue, 2 } };
        var expected = new Dictionary<PokerChip, ulong> { { PokerChip.Red, 10 }, { PokerChip.Blue, 2 } };

        // Act
        Bank.MergeStacks(mainStack, valuesToAdd);

        // Assert
        Assert.Equal(expected, mainStack);
    }

    [Fact]
    public void MergeStacks_DoesNotChangeMainStackIfValuesToAddIsEmpty()
    {
        // Arrange
        var mainStack = new Dictionary<PokerChip, ulong> { { PokerChip.Red, 10 } };
        var valuesToAdd = new Dictionary<PokerChip, ulong>();

        // Act
        Bank.MergeStacks(mainStack, valuesToAdd);

        // Assert
        Assert.Single(mainStack);
        Assert.Contains(KeyValuePair.Create(PokerChip.Red, 10UL), mainStack);
    }
}