using Poker.PhysicalObjects.Cards;
using Poker.PhysicalObjects.Decks;

namespace Poker.Tests.PhysicalObjects.Decks
{
    public class SharedCardsTests
    {
        [Fact]
        public void OpenNextStage_IncreasesStage()
        {
            var deck = new Deck();
            var sharedCards = new CommunityCards();
            sharedCards.OpenNextStage(deck);

            Assert.Equal(1, (int)sharedCards.Stage);
        }

        [Fact]
        public void RevealAll_SetsStageToThree()
        {
            var deck = new Deck();
            var sharedCards = new CommunityCards();
            sharedCards.RevealAll(deck);

            Assert.Equal(3, (int)sharedCards.Stage);
        }

        [Fact]
        public void Clear_ResetsStageAndSlots()
        {
            var deck = new Deck();
            var sharedCards = new CommunityCards();
            sharedCards.RevealAll(deck);
            sharedCards.Clear();

            Assert.Equal(0, (int)sharedCards.Stage);
            Assert.All(sharedCards.TableCards, slot => Assert.Null(slot));
        }

        [Fact]
        public void OpenNextStage_ThrowsException_WhenStageIsThree()
        {
            var deck = new Deck();
            var sharedCards = new CommunityCards();
            sharedCards.RevealAll(deck);

            Assert.Throws<InvalidOperationException>(() => sharedCards.OpenNextStage(deck));
        }

        [Fact]
        public void Slots_ReturnsCopy_NotOriginalArray()
        {
            var deck = new Deck();
            var sharedCards = new CommunityCards();
            sharedCards.OpenNextStage(deck);

            var slotsCopy = sharedCards.TableCards;
            slotsCopy[0] = new Card(); // Modify the copy

            Assert.NotEqual(slotsCopy[0], sharedCards.TableCards[0]);
        }

        // Additional tests can be added here
    }
}
