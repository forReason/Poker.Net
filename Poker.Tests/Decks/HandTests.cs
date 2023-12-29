using Xunit;
using Poker.Decks;
using Poker.Cards;
using System;

namespace Poker.Tests
{
    public class HandTests
    {

        [Fact]
        public void DrawCard_IncreasesCardCount()
        {
            var deck = new Deck();
            var hand = new Hand();
            hand.DealCard(deck);

            Assert.Equal(1, hand.CardCount);
        }

        [Fact]
        public void DrawCard_Twice_FillsSlots()
        {
            var deck = new Deck();
            var hand = new Hand();
            hand.DealCard(deck);
            hand.DealCard(deck);

            Assert.Equal(2, hand.CardCount);
            Assert.NotNull(hand.Slots[0]);
            Assert.NotNull(hand.Slots[1]);
        }

        [Fact]
        public void DrawCard_ThrowsException_WhenDrawingMoreThanTwoCards()
        {
            var deck = new Deck();
            var hand = new Hand();
            hand.DealCard(deck);
            hand.DealCard(deck);

            Assert.Throws<InvalidOperationException>(() => hand.DealCard(deck));
        }

        [Fact]
        public void Clear_ResetsHand()
        {
            var deck = new Deck();
            var hand = new Hand();
            hand.DealCard(deck);
            hand.DealCard(deck);
            hand.Clear();

            Assert.Equal(0, hand.CardCount);
            Assert.All(hand.Slots, slot => Assert.Null(slot));
        }

        // Additional tests can be added here
    }
}