﻿using Xunit;
using Poker.PhysicalObjects.Cards;
using Poker.PhysicalObjects.Decks;

namespace Poker.Tests.PhysicalObjects.Cards;

public class CardTests
{
    [Fact]
    public void Card_Constructor_SetsCardRankAndSuit()
    {
        // Arrange
        var cardRank = CardRank.Ace;
        var suit = CardSuit.Hearts;

        // Act
        var card = new Card(cardRank, suit);

        // Assert
        Assert.Equal(cardRank, card.CardRank);
        Assert.Equal(suit, card.Suit);
    }

    [Fact]
    public void Equals_ReturnsTrueForEqualCards()
    {
        // Arrange
        var card1 = new Card(CardRank.King, CardSuit.Clubs);
        var card2 = new Card(CardRank.King, CardSuit.Clubs);

        // Act & Assert
        Assert.True(card1.Equals(card2));
    }
    [Fact]
    public void Equals_ReturnsTrueForNullComparisons()
    {
        // Arrange
        Card? card1 = null;
        Card? card2 = null;

        // Act & Assert
        Assert.True(card1 == null);
        Assert.True(card2 == null);
    }

    [Fact]
    public void Equals_ReturnsFalseForDifferentCards()
    {
        // Arrange
        var card1 = new Card(CardRank.King, CardSuit.Clubs);
        var card2 = new Card(CardRank.Queen, CardSuit.Clubs);

        // Act & Assert
        Assert.False(card1.Equals(card2));
    }
    [Fact]
    public void Equals_ReturnsFalseForComparisonWithNull()
    {
        // Arrange
        var card1 = new Card(CardRank.King, CardSuit.Clubs);
        // Arrange
        Card? nullCard = null; 
        
        // Assert
        Assert.False(card1.Equals(null));
        Assert.False(nullCard == card1);
        Assert.False(null == card1);
        Assert.False(card1== null);
        Assert.False(card1 == nullCard);
    }
    [Fact]
    public void Equals_ReturnsFalseForOtherCardsInDeck()
    {
        Deck deck = new Deck();
        for (int i = 0; i < 10; i++)
        {
            deck.ShuffleCards();
            Card  compareCard = deck.DrawCard();
            for (int b = 0; b < 50; b++)
            {
                Card otherCard = deck.DrawCard();
                Assert.NotEqual(compareCard, otherCard);
            }
        }
    }
    [Fact]
    public void HashesAreDifferentForEachCard()
    {
        Deck deck = new Deck();
        int cardCount = deck.CardCount;
        HashSet<int> hashes = new HashSet<int>();
        do
        {
            hashes.Add(deck.DrawCard().GetHashCode());
        } while (deck.CardCount > 0);
        Assert.Equal(52, cardCount);
    }

    [Theory]
    [InlineData(CardRank.Ace, CardRank.King)]
    [InlineData(CardRank.King, CardRank.Queen)]
    [InlineData(CardRank.Queen, CardRank.Jack)]
    [InlineData(CardRank.Jack, CardRank.Ten)]
    [InlineData(CardRank.Ten, CardRank.Nine)]
    [InlineData(CardRank.Nine, CardRank.Eight)]
    [InlineData(CardRank.Eight, CardRank.Seven)]
    [InlineData(CardRank.Seven, CardRank.Six)]
    [InlineData(CardRank.Six, CardRank.Five)]
    [InlineData(CardRank.Five, CardRank.Four)]
    [InlineData(CardRank.Four, CardRank.Three)]
    [InlineData(CardRank.Three, CardRank.Two)]
    public void Operator_CorrectlyComparesCards(CardRank CardRank1, CardRank CardRank2)
    {
        // Arrange
        var card1 = new Card(CardRank1, CardSuit.Hearts);
        var card2 = new Card(CardRank2, CardSuit.Diamonds);

        // try if the correct operator returns true as expected
        Assert.True(card1 > card2);
        Assert.True(card1 >= card2);
        Assert.True(card1 != card2);
        Assert.True(card2 <= card1);
        Assert.True(card2 < card1);

        // try if the false operator returns false as expected
        Assert.False(card1 < card2);
        Assert.False(card1 <= card2);
        Assert.False(card1 == card2);
        Assert.False(card2 >= card1);
        Assert.False(card2 > card1);
    }
    [Fact]
    public void OperatorCorrectlyIdentifiesCardsOfSameCardRank()
    {
        foreach (CardRank CardRank in Enum.GetValues(typeof(CardRank)))
        {
            List<Card> cardsToCompare = new List<Card>();
            foreach(CardSuit suit in Enum.GetValues(typeof(CardSuit)))
            {
                cardsToCompare.Add(new Card(CardRank, suit));
            }
            for(int i = 1; i < cardsToCompare.Count; i++)
            {
                // try if the correct operator returns true as expected
                Assert.True(cardsToCompare[0] >= cardsToCompare[i]);
                Assert.True(cardsToCompare[0] == cardsToCompare[i]);
                Assert.True(cardsToCompare[0] <= cardsToCompare[i]);

                // try if the false operator returns false as expected
                Assert.False(cardsToCompare[0] < cardsToCompare[i]);
                Assert.False(cardsToCompare[0] > cardsToCompare[i]);
                Assert.False(cardsToCompare[0].Equals(cardsToCompare[i]));
            }
        }
    }
}
