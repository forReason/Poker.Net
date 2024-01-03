using Poker.Chips;

namespace Poker.Tests.PhysicalObjects.Chips;
public class BankTests
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

    [Fact]
    public void TestExchangeChipsForSmallerDenominations()
    {
        var result = Bank.ExchangeChipsForSmallerDenominations(PokerChip.Black, 1); // $100

        Assert.Equal(2UL, result[PokerChip.Blue]);  // $50
        Assert.DoesNotContain(PokerChip.Black, result.Keys);
    }
}