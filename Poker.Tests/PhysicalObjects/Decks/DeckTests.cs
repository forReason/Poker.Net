
using Poker.PhysicalObjects.Cards;
using Poker.PhysicalObjects.Decks;

namespace Poker.Tests.PhysicalObjects.Decks
{
    public class DeckTests
    {
        [Fact]
        public void Deck_InitializesWith52Cards()
        {
            var deck = new Deck();
            Assert.Equal(52, deck.CardCount);
        }

        [Fact]
        public void DrawCard_DecreasesCardCount()
        {
            var deck = new Deck();
            deck.DrawCard();
            Assert.Equal(51, deck.CardCount);
        }

        [Fact]
        public void DrawCard_ThrowsException_WhenNoCardsLeft()
        {
            var deck = new Deck();
            // Draw all cards
            for (int i = 0; i < 52; i++)
            {
                deck.DrawCard();
            }

            Assert.Throws<InvalidOperationException>(() => deck.DrawCard());
        }

        [Fact]
        public void ShuffleCards_MaintainsCardCount()
        {
            var deck = new Deck();
            deck.ShuffleCards();
            Assert.Equal(52, deck.CardCount);
        }

        [Fact]
        public void ShuffleCards_CardsChange()
        {
            var deck = new Deck();
            deck.ShuffleCards();
            Card firstCard = deck.DrawCard();
            deck.ShuffleCards();
            Card secondCard = deck.DrawCard();
            Assert.NotEqual(firstCard, secondCard);
        }
    }
}