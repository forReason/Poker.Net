using Xunit;
using Poker.Decks;
using Poker.Cards;
using System;

namespace Poker.Tests.PhysicalObjects.Decks
{
    public class PocketCardsTests
    {

        [Fact]
        public void DrawCard_IncreasesCardCount()
        {
            var deck = new Deck();
            var hand = new PocketCards();
            hand.DealCard(deck);

            Assert.Equal(1, hand.CardCount);
        }

        [Fact]
        public void DrawCard_Twice_FillsSlots()
        {
            var deck = new Deck();
            var hand = new PocketCards();
            hand.DealCard(deck);
            hand.DealCard(deck);

            Assert.Equal(2, hand.CardCount);
            Assert.NotNull(hand.Cards[0]);
            Assert.NotNull(hand.Cards[1]);
        }

        [Fact]
        public void DrawCard_ThrowsException_WhenDrawingMoreThanTwoCards()
        {
            var deck = new Deck();
            var hand = new PocketCards();
            hand.DealCard(deck);
            hand.DealCard(deck);

            Assert.Throws<InvalidOperationException>(() => hand.DealCard(deck));
        }

        [Fact]
        public void Clear_ResetsHand()
        {
            var deck = new Deck();
            var hand = new PocketCards();
            hand.DealCard(deck);
            hand.DealCard(deck);
            hand.Clear();

            Assert.Equal(0, hand.CardCount);
            Assert.All(hand.Cards, slot => Assert.Null(slot));
        }

        // Additional tests can be added here
    }
}