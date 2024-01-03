using Poker.PhysicalObjects.Cards;

public class CardRankTests
{
    [Fact]
    public void CardRanks_AreUnique()
    {
        var ranks = Enum.GetValues(typeof(CardRank)).Cast<CardRank>().ToList();
        var distinctRanks = ranks.Distinct().ToList();
        Assert.Equal(distinctRanks.Count, ranks.Count);
    }

    [Theory]
    [InlineData(CardRank.Two, CardRank.Three)]
    [InlineData(CardRank.Four, CardRank.Five)]
    [InlineData(CardRank.Five, CardRank.Six)]
    [InlineData(CardRank.Six, CardRank.Seven)]
    [InlineData(CardRank.Seven, CardRank.Eight)]
    [InlineData(CardRank.Eight, CardRank.Nine)]
    [InlineData(CardRank.Nine, CardRank.Ten)]
    [InlineData(CardRank.Ten, CardRank.Jack)]
    [InlineData(CardRank.Jack, CardRank.Queen)]
    [InlineData(CardRank.Queen, CardRank.King)]
    [InlineData(CardRank.King, CardRank.Ace)]
    public void CardRanks_FollowExpectedOrder(CardRank lower, CardRank higher)
    {
        Assert.True((int)lower < (int)higher);
    }
}
