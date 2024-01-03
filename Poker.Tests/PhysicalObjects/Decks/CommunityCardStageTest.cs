using Poker.PhysicalObjects.Decks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Tests.PhysicalObjects.Decks
{
    public class CommunityCardStageTest
    {
        [Fact]
        public void CommunityCardStage_ProgressesCorrectly()
        {
            // Arrange
            var communityCards = new CommunityCards();
            var deck = new Deck(); // Assuming a Deck class exists

            // Act & Assert
            // Initially, it should be PreFlop
            Assert.Equal(CommunityCardStage.PreFlop, communityCards.Stage);

            // Open next stage to Flop
            communityCards.OpenNextStage(deck);
            Assert.Equal(CommunityCardStage.Flop, communityCards.Stage);
            Assert.NotNull(communityCards.TableCards[0]);
            Assert.NotNull(communityCards.TableCards[1]);
            Assert.NotNull(communityCards.TableCards[2]);

            // Open next stage to Turn
            communityCards.OpenNextStage(deck);
            Assert.Equal(CommunityCardStage.Turn, communityCards.Stage);
            Assert.NotNull(communityCards.TableCards[3]);

            // Open next stage to River
            communityCards.OpenNextStage(deck);
            Assert.Equal(CommunityCardStage.River, communityCards.Stage);
            Assert.NotNull(communityCards.TableCards[4]);
        }

    }
}
