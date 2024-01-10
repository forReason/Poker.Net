
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
        IReadOnlyDictionary<PokerChip, ulong> dict = chips;
        ulong recoloredValue = Bank.ConvertChipsToValue(dict);
        Assert.Equal(originalValue, recoloredValue);  // Expect to get the same amount back
        Assert.True(result.GetChips().Count > 5);
    }

    [Fact]
    public void TestExchangeChipsForSmallerDenominations()
    {
        var result = Bank.ExchangeChipsForSmallerDenominations(PokerChip.Black, 1); // $100

        Assert.Equal(2UL, result.GetChips()[PokerChip.Blue]);  // $50
        Assert.DoesNotContain(PokerChip.Black, result.GetChips().Keys);
    }
    [Fact]
    public void MergeStacks_CombinesTwoStacksCorrectly()
    {
        // Arrange
        ChipStack mainStack = new ChipStack(new Dictionary<PokerChip, ulong>
        {
            { PokerChip.Red, 10 },
            { PokerChip.Green, 5 }
        });
        ChipStack valuesToAdd = new ChipStack(new Dictionary<PokerChip, ulong>
        {
            { PokerChip.Red, 5 },
            { PokerChip.Blue, 3 }
        });
        ChipStack expectedStack = new ChipStack(new Dictionary<PokerChip, ulong>
        {
            { PokerChip.Red, 15 },
            { PokerChip.Green, 5 },
            { PokerChip.Blue, 3 }
        });

        // Act
        mainStack.Merge(valuesToAdd);

        // Assert
        Assert.Equal(expectedStack.GetChips(), mainStack.GetChips());
    }

    [Fact]
    public void MergeStacks_AddsNewValuesIfNotPresent()
    {
        // Arrange
        ChipStack mainStack = new ChipStack(new Dictionary<PokerChip, ulong> { { PokerChip.Red, 10 } });
        ChipStack valuesToAdd = new ChipStack(new Dictionary<PokerChip, ulong> { { PokerChip.Blue, 2 } });
        ChipStack expected = new ChipStack(new Dictionary<PokerChip, ulong> { { PokerChip.Red, 10 }, { PokerChip.Blue, 2 } });

        // Act
        mainStack.Merge(valuesToAdd);

        // Assert
        Assert.Equal(expected.GetChips(), mainStack.GetChips());
    }

    [Fact]
    public void MergeStacks_DoesNotChangeMainStackIfValuesToAddIsEmpty()
    {
        // Arrange
        ChipStack mainStack = new ChipStack(new Dictionary<PokerChip, ulong> { { PokerChip.Red, 10 } });
        ChipStack valuesToAdd = new ChipStack(new Dictionary<PokerChip, ulong>());

        // Act
        mainStack.Merge(valuesToAdd);

        // Assert
        Assert.Single(mainStack.GetChips());
        Assert.Contains(KeyValuePair.Create(PokerChip.Red, 10UL), mainStack.GetChips());
    }
}