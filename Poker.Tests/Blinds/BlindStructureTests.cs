using System;
using Xunit;
using Poker.Blinds;
using System.Linq;


namespace Poker.Tests.Blinds;

public class BlindStructureTests
{
    [Fact]
    public void BlindStructure_CalculatesCorrectLevels()
    {
        // Arrange
        var blindStructure = new BlindStructure
        {
            BuyIn = 1000, // Example buy-in
            Ante = AnteToBigBlindRatio.OneToFive
        };

        // Act
        var blindLevels = blindStructure.CalculateBlindStructure();

        // Assert
        Assert.NotNull(blindLevels);
        Assert.True(blindLevels.Count > 0, "Blind levels should be more than zero.");

        // Check the first level
        var firstLevel = blindLevels.First();
        Assert.Equal(1, firstLevel.Level);
        Assert.Equal(50UL, firstLevel.SmallBlind); // BuyIn / BlindRatio
        Assert.Equal(100UL, firstLevel.BigBlind);   // SmallBlind * 2
        Assert.Equal(0UL, firstLevel.Ante);        // Ante is introduced in the second half

        // Check the introduction of ante (assuming more than 2 levels)
        if (blindLevels.Count > 2)
        {
            var anteIntroductionLevel = blindLevels[blindLevels.Count / 2];
            Assert.True(anteIntroductionLevel.Ante > 0, "Ante should be introduced in the second half.");
        }
    }
}