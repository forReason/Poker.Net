using Xunit;
using Poker.PhysicalObjects.Cards;
using Poker.PhysicalObjects.Decks;

namespace Poker.Tests.PhysicalObjects.Decks
{
    public class CommunityCardsTests
    {
        [Fact]
        public void Set_CommunityCards_SetsCorrectStageAndCards()
        {
            // Arrange
            
            var cards = new Card[] { new Card(CardRank.Ace, CardSuit.Hearts), new Card(CardRank.King, CardSuit.Diamonds), new Card(CardRank.Queen, CardSuit.Clubs) };
            var communityCards = new CommunityCards(cards);


            // Assert
            Assert.Equal(CommunityCardStage.Flop, communityCards.Stage);
            Assert.Equal(cards, communityCards.TableCards.Take(cards.Length).Cast<Card>());
        }

        [Fact]
        public void OpenNextStage_AdvancesStageAndBurnsOneCard()
        {
            // Arrange
            var communityCards = new CommunityCards();
            var deck = new Deck(); // Assuming Deck class is implemented

            // Act
            communityCards.OpenNextStage(deck);

            // Assert
            Assert.Equal(CommunityCardStage.Flop, communityCards.Stage);
            Assert.NotNull(communityCards.BurnCards[0]);
            Assert.NotNull(communityCards.TableCards[0]);
            Assert.NotNull(communityCards.TableCards[1]);
            Assert.NotNull(communityCards.TableCards[2]);
        }

        [Fact]
        public void RevealAll_RevealsAllCommunityCards()
        {
            // Arrange
            var communityCards = new CommunityCards();
            var deck = new Deck();

            // Act
            communityCards.RevealAll(deck);

            // Assert
            Assert.Equal(CommunityCardStage.River, communityCards.Stage);
            Assert.NotNull(communityCards.TableCards[0]);
            Assert.NotNull(communityCards.TableCards[1]);
            Assert.NotNull(communityCards.TableCards[2]);
            Assert.NotNull(communityCards.TableCards[3]);
            Assert.NotNull(communityCards.TableCards[4]);
        }

        [Fact]
        public void RevealAllMidGame_RevealsAllCommunityCards()
        {
            // Arrange
            var communityCards = new CommunityCards();
            var deck = new Deck();

            // Act
            communityCards.OpenNextStage(deck);
            communityCards.RevealAll(deck);

            // Assert
            Assert.Equal(CommunityCardStage.River, communityCards.Stage);
            Assert.NotNull(communityCards.TableCards[0]);
            Assert.NotNull(communityCards.TableCards[1]);
            Assert.NotNull(communityCards.TableCards[2]);
            Assert.NotNull(communityCards.TableCards[3]);
            Assert.NotNull(communityCards.TableCards[4]);
        }

        [Fact]
        public void Clear_ResetsCommunityCards()
        {
            // Arrange
            var communityCards = new CommunityCards();
            var deck = new Deck();
            communityCards.OpenNextStage(deck);

            // Act
            communityCards.Clear();

            // Assert
            Assert.Equal(CommunityCardStage.PreFlop, communityCards.Stage);
            Assert.All(communityCards.TableCards, card => Assert.Null(card));
            Assert.All(communityCards.BurnCards, card => Assert.Null(card));
        }

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
    }
}
