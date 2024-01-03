
using Poker.PhysicalObjects.Chips;

namespace Poker.Tests.PhysicalObjects.Chips;
public class BankChipValueConversionTests
{
    [Fact]
    public void TestConvert0Value()
    {
        var result = Bank.ConvertValueToChips(0);
        Assert.Empty(result);
    }

    [Fact]
    public void TestConvertChipsToValue()
    {
        var chips = new Dictionary<PokerChip, ulong>
        {
            { PokerChip.Red, 2 },    // $10
            { PokerChip.Brown, 1 }    // $10
        };

        ulong value = Bank.ConvertChipsToValue(chips);

        Assert.Equal(20UL, value);  // Expect $20 total
    }

    [Fact]
    public void TestConvertValueToChips()
    {
        ulong value = 55; // $55

        var chips = Bank.ConvertValueToChips(value);

        Assert.Equal(1UL, chips[PokerChip.Blue]);   // $25
        Assert.Equal(1UL, chips[PokerChip.Red]);    // $30
        Assert.DoesNotContain(PokerChip.Black, chips.Keys);
    }
    [Theory]
    [InlineData(150, new[] { PokerChip.Black, PokerChip.Blue, PokerChip.White })]
    [InlineData(75, new[] { PokerChip.Green, PokerChip.Red })]
    public void DistributeValueForUse_DistributesCorrectly(ulong value, PokerChip[] expectedChips)
    {
        // Act
        var result = Bank.DistributeValueForUse(value);

        // Assert
        Assert.Equal(expectedChips.Length, result.Count);
        foreach (var chip in expectedChips)
        {
            Assert.True(result.ContainsKey(chip));
            Assert.True(result[chip] > 0);
        }
    }
}