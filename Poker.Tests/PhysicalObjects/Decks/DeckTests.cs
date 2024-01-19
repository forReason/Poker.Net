
using System.Security.Cryptography;
using Poker.Net.PhysicalObjects.Cards;
using Poker.Net.PhysicalObjects.Decks;

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
        public void RNG_TestGenerator()
        {
            int testSize = 100000; // Number of random values to generate
            int deckSize = 52; // Simulating a deck of cards
            var frequencyCount = new int[deckSize];
        
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] randomBytes = new byte[4];

            for (int i = 0; i < testSize; i++)
            {
                rng.GetBytes(randomBytes);
                uint randomIndex = BitConverter.ToUInt32(randomBytes, 0) % (uint)deckSize;
                frequencyCount[randomIndex]++;
            }

            rng.Dispose();

            // Output the frequency of each number
            for (int i = 0; i < deckSize; i++)
            {
                Console.WriteLine($"Number {i + 1}: Frequency = {frequencyCount[i]}");
            }
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
        [Fact]
        public void ShuffleCards_ShouldChangeOrderOfCards()
        {
            var deck = new Deck();
            var originalOrder = deck.GetDeckSnapshot(); // This method needs to be implemented in Deck class to return a copy of the current state of _shuffledCards

            // Act
            deck.ShuffleCards();
            var newOrder = deck.GetDeckSnapshot();

            // Assert
            Assert.False(originalOrder.SequenceEqual(newOrder), "The order of the cards should be different after washing the deck.");
            Assert.True(deck.ConfirmDeckIntegrity());
            double displacement = deck.CalculateAverageDisplacement();
            Assert.True(displacement > 10);
        }
        [Fact]
        public void WashDeck_ShouldChangeOrderOfCards()
        {
            // Arrange
            var deck = new Deck();
            var originalOrder = deck.GetDeckSnapshot(); // This method needs to be implemented in Deck class to return a copy of the current state of _shuffledCards

            // Act
            deck.WashDeck();
            var newOrder = deck.GetDeckSnapshot();

            // Assert
            Assert.False(originalOrder.SequenceEqual(newOrder), "The order of the cards should be different after washing the deck.");
            Assert.True(deck.ConfirmDeckIntegrity());
            double displacement = deck.CalculateAverageDisplacement();
            Assert.True(displacement > 10);
        }
        [Fact]
        public void RiffleShuffle_ChangesOrderOfCards_MaintainsDeckIntegrity()
        {
            // Arrange
            var deck = new Deck();
            var originalOrder = deck.GetDeckSnapshot();

            // Act
            deck.RiffleShuffle();
            var newOrder = deck.GetDeckSnapshot();

            // Assert
            Assert.False(originalOrder.SequenceEqual(newOrder), "The order of the cards should be different after a riffle shuffle.");
            Assert.True(deck.ConfirmDeckIntegrity(), "The deck should maintain integrity after a riffle shuffle.");
            double displacement = deck.CalculateAverageDisplacement();
            Assert.True(displacement > 10);
        }
        [Fact]
        public void StripShuffle_ChangesOrderOfCards_MaintainsDeckIntegrity()
        {
            // Arrange
            var deck = new Deck();
            var originalOrder = deck.GetDeckSnapshot();

            // Act
            deck.StripShuffle();
            var newOrder = deck.GetDeckSnapshot();

            // Assert
            Assert.False(originalOrder.SequenceEqual(newOrder), "The order of the cards should be different after a strip shuffle.");
            Assert.True(deck.ConfirmDeckIntegrity(), "The deck should maintain integrity after a strip shuffle.");
            double displacement = deck.CalculateAverageDisplacement();
            Assert.True(displacement > 20);
            
        }
        [Fact]
        public void Cut_ChangesDeckOrder_MaintainsDeckIntegrity()
        {
            // Arrange
            var deck = new Deck();
            var originalOrder = deck.GetDeckSnapshot();

            // Act
            deck.Cut();
            var newOrder = deck.GetDeckSnapshot();

            // Assert
            Assert.False(originalOrder.SequenceEqual(newOrder), "The order of the cards should be different after a cut.");
            Assert.True(deck.ConfirmDeckIntegrity(), "The deck should maintain integrity after a cut.");
            double displacement = deck.CalculateAverageDisplacement();
            Assert.True(displacement > 10);
        }

    }
}