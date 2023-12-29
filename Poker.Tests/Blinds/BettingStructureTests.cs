using System;
using Xunit;
using Poker.Blinds;
using System.Linq;
using Poker.Tables;


namespace Poker.Tests.Blinds;

public class BettingStructureTests
{
    [Fact]
    public void BlindStructure_CalculatesCorrectLevels()
    {
        // Arrange
        var blindStructure = new BettingStructure(
            TableRuleSet.Tournament,
            buyIn: 1000,
            blindtoByinRatio: BlindToBuyInRatio.OneToEighty,
            maxBuyInRatio: 1,
            ante: AnteToBigBlindRatio.OneToFive);

        // Act
        var blindLevels = blindStructure.BlindStructure;

        // Assert
        Assert.NotNull(blindLevels);
        Assert.True(blindLevels.Count > 0, "Blind levels should be more than zero.");

        // Check the first level
        var firstLevel = blindLevels.First();
        Assert.Equal(1, firstLevel.Level);
        Assert.Equal(20UL, firstLevel.SmallBlind); // BuyIn / BlindRatio
        Assert.Equal(40UL, firstLevel.BigBlind);   // SmallBlind * 2
        Assert.Equal(0UL, firstLevel.Ante);        // Ante is introduced in the second half

        // Check the introduction of ante (assuming more than 2 levels)
        if (blindLevels.Count > 2)
        {
            var anteIntroductionLevel = blindLevels[blindLevels.Count / 2];
            Assert.True(anteIntroductionLevel.Ante > 0, "Ante should be introduced in the second half.");
        }
    }
}