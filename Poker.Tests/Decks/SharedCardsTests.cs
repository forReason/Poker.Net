using Xunit;
using Poker.Decks;
using Poker.Cards;
using System;

namespace Poker.Tests
{
    public class SharedCardsTests
    {
        [Fact]
        public void OpenNextStage_IncreasesStage()
        {
            var deck = new Deck();
            var sharedCards = new SharedCards();
            sharedCards.OpenNextStage(deck);

            Assert.Equal(1, sharedCards.Stage);
        }

        [Fact]
        public void RevealAll_SetsStageToThree()
        {
            var deck = new Deck();
            var sharedCards = new SharedCards();
            sharedCards.RevealAll(deck);

            Assert.Equal(3, sharedCards.Stage);
        }

        [Fact]
        public void Clear_ResetsStageAndSlots()
        {
            var deck = new Deck();
            var sharedCards = new SharedCards();
            sharedCards.RevealAll(deck);
            sharedCards.Clear();

            Assert.Equal(0, sharedCards.Stage);
            Assert.All(sharedCards.Slots, slot => Assert.Null(slot));
        }

        [Fact]
        public void OpenNextStage_ThrowsException_WhenStageIsThree()
        {
            var deck = new Deck();
            var sharedCards = new SharedCards();
            sharedCards.RevealAll(deck);

            Assert.Throws<InvalidOperationException>(() => sharedCards.OpenNextStage(deck));
        }

        [Fact]
        public void Slots_ReturnsCopy_NotOriginalArray()
        {
            var deck = new Deck();
            var sharedCards = new SharedCards();
            sharedCards.OpenNextStage(deck);

            var slotsCopy = sharedCards.Slots;
            slotsCopy[0] = new Card(); // Modify the copy

            Assert.NotEqual(slotsCopy[0], sharedCards.Slots[0]);
        }

        // Additional tests can be added here
    }
}
