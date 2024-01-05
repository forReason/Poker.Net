using NuGet.Frameworks;
using Xunit;
using Poker.Tests.TestAssets;
using Poker.PhysicalObjects.Cards;
using Poker.PhysicalObjects.Decks;

namespace Poker.Tests.PhysicalObjects.Decks;

public class CardEvaluationTests
{
    [Fact]
    public void ScoreCards_ShouldIdentifyFullHouse()
    {
        var communityCards = new Card[]
        {
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Two, CardSuit.Hearts),
            new Card(CardRank.Queen, CardSuit.Hearts),
            new Card(CardRank.Two, CardSuit.Spades),
            new Card(CardRank.Three, CardSuit.Spades)
        };
        var playerPocketCards = new PocketCards(new Card(CardRank.Three, CardSuit.Hearts), new Card(CardRank.Two, CardSuit.Clubs));

        var handScore = CardEvaluation.ScoreCards(communityCards, playerPocketCards);

        Assert.Equal(HandCardRank.FullHouse, handScore.CardRank);
        // Add more assertions as needed
    }
    [Theory]
    [MemberData(nameof(CardRankTestSamples.GetSampleData), MemberType = typeof(CardRankTestSamples))]
    public void ScoreCards_ShouldCorrectlyIdentifyHand(HandCardRank expectedHandCardRank, Card[] communityCards, Card[] pocketCards)
    {
        // Arrange
        var playerPocketCards = new PocketCards();
        if (pocketCards.Length > 0)
        {
            playerPocketCards = new PocketCards(pocketCards[0], pocketCards[1]);
        }


        // Act
        var handScore = CardEvaluation.ScoreCards(communityCards, playerPocketCards);

        // Assert
        Assert.Equal(expectedHandCardRank, handScore.CardRank);
        // Add more specific assertions if necessary
    }

    [Fact]
    public void TestRoyalFlushesSameCardRank()
    {
        (Card[] CommunityCards, Card[] PocketCards)[] samples = CardRankTestSamples.Samples[HandCardRank.RoyalFlush];
        (Card[] CommunityCards, Card[] PocketCards) sample1 = samples[0];
        var sample1PocketCards = new PocketCards();
        if (sample1.PocketCards.Length > 0)
        {
            sample1PocketCards = new PocketCards(sample1.PocketCards[0], sample1.PocketCards[1]);
        }
        var sample1Score = CardEvaluation.ScoreCards(sample1.CommunityCards, sample1PocketCards);
        foreach (var sample in samples)
        {
            var pocketCards = new PocketCards();
            if (sample.PocketCards.Length > 0)
            {
                pocketCards = new PocketCards(sample.PocketCards[0], sample.PocketCards[1]);
            }
            var sampleScore = CardEvaluation.ScoreCards(sample.CommunityCards, pocketCards);
            Assert.Equal(sample1Score, sampleScore);
        }
    }
    [Fact]
    public void TestRoyalFlushesHigherThanStraightFlushes()
    {
        (Card[] CommunityCards, Card[] PocketCards)[] royalFlushes = CardRankTestSamples.Samples[HandCardRank.RoyalFlush];
        (Card[] CommunityCards, Card[] PocketCards)[] straightFlushes = CardRankTestSamples.Samples[HandCardRank.StraightFlush];
        foreach (var royalFlush in royalFlushes)
        {
            var royalFlushPocketCards = new PocketCards();
            if (royalFlush.PocketCards.Length > 0)
            {
                royalFlushPocketCards = new PocketCards(royalFlush.PocketCards[0], royalFlush.PocketCards[1]);
            }
            var higherScore = CardEvaluation.ScoreCards(royalFlush.CommunityCards, royalFlushPocketCards);
            foreach (var straightFlush in straightFlushes)
            {
                var straightFlushPocketCards = new PocketCards();
                if (straightFlush.PocketCards.Length > 0)
                {
                    straightFlushPocketCards = new PocketCards(straightFlush.PocketCards[0], straightFlush.PocketCards[1]);
                }
                var lowerScore = CardEvaluation.ScoreCards(straightFlush.CommunityCards, straightFlushPocketCards);
                Assert.True(higherScore > lowerScore);
                Assert.True(lowerScore < higherScore);
            }

        }
    }

    [Fact]
    public void TestStraightFlushesHigherThanFourOfAKind()
    {
        (Card[] CommunityCards, Card[] PocketCards)[] fourOfAkinds = CardRankTestSamples.Samples[HandCardRank.FourOfAKind];
        (Card[] CommunityCards, Card[] PocketCards)[] straightFlushes = CardRankTestSamples.Samples[HandCardRank.StraightFlush];
        foreach (var straightFlush in straightFlushes)
        {
            var straightFlushPocketCards = new PocketCards();
            if (straightFlush.PocketCards.Length > 0)
            {
                straightFlushPocketCards = new PocketCards(straightFlush.PocketCards[0], straightFlush.PocketCards[1]);
            }
            var higherScore = CardEvaluation.ScoreCards(straightFlush.CommunityCards, straightFlushPocketCards);
            foreach (var fourOfAkind in fourOfAkinds)
            {
                var fourOfAkindPocketCards = new PocketCards();
                if (fourOfAkind.PocketCards.Length > 0)
                {
                    fourOfAkindPocketCards = new PocketCards(fourOfAkind.PocketCards[0], fourOfAkind.PocketCards[1]);
                }
                var lowerScore = CardEvaluation.ScoreCards(fourOfAkind.CommunityCards, fourOfAkindPocketCards);
                Assert.True(higherScore > lowerScore);
                Assert.True(lowerScore < higherScore);
            }

        }
    }

    [Fact]
    public void TestFourOfAKindHigherThanFullHouse()
    {
        (Card[] CommunityCards, Card[] PocketCards)[] fourOfKinds = CardRankTestSamples.Samples[HandCardRank.FourOfAKind];
        (Card[] CommunityCards, Card[] PocketCards)[] fullHouses = CardRankTestSamples.Samples[HandCardRank.FullHouse];
        foreach (var higherCards in fourOfKinds)
        {
            var higherPocketCards = new PocketCards();
            if (higherCards.PocketCards.Length > 0)
            {
                higherPocketCards = new PocketCards(higherCards.PocketCards[0], higherCards.PocketCards[1]);
            }
            var higherScore = CardEvaluation.ScoreCards(higherCards.CommunityCards, higherPocketCards);
            foreach (var lowerCards in fullHouses)
            {
                var lowerCardsPocketCards = new PocketCards();
                if (lowerCards.PocketCards.Length > 0)
                {
                    lowerCardsPocketCards = new PocketCards(lowerCards.PocketCards[0], lowerCards.PocketCards[1]);
                }
                var lowerScore = CardEvaluation.ScoreCards(lowerCards.CommunityCards, lowerCardsPocketCards);
                Assert.True(higherScore > lowerScore);
                Assert.True(lowerScore < higherScore);
            }
        }
    }

    [Fact]
    public void TestFullHouseHigherThanFlush()
    {
        (Card[] CommunityCards, Card[] PocketCards)[] fullHouse = CardRankTestSamples.Samples[HandCardRank.FullHouse];
        (Card[] CommunityCards, Card[] PocketCards)[] flush = CardRankTestSamples.Samples[HandCardRank.Flush];
        foreach (var higherCards in fullHouse)
        {
            var higherPocketCards = new PocketCards();
            if (higherCards.PocketCards.Length > 0)
            {
                higherPocketCards = new PocketCards(higherCards.PocketCards[0], higherCards.PocketCards[1]);
            }
            var higherScore = CardEvaluation.ScoreCards(higherCards.CommunityCards, higherPocketCards);
            foreach (var lowerCards in flush)
            {
                var lowerCardsPocketCards = new PocketCards();
                if (lowerCards.PocketCards.Length > 0)
                {
                    lowerCardsPocketCards = new PocketCards(lowerCards.PocketCards[0], lowerCards.PocketCards[1]);
                }
                var lowerScore = CardEvaluation.ScoreCards(lowerCards.CommunityCards, lowerCardsPocketCards);
                Assert.True(higherScore > lowerScore);
                Assert.True(lowerScore < higherScore);
            }
        }
    }

    [Fact]
    public void TestFlushHigherThanStraight()
    {
        (Card[] CommunityCards, Card[] PocketCards)[] flush = CardRankTestSamples.Samples[HandCardRank.Flush];
        (Card[] CommunityCards, Card[] PocketCards)[] straight = CardRankTestSamples.Samples[HandCardRank.Straight];
        foreach (var higherCards in flush)
        {
            var higherPocketCards = new PocketCards();
            if (higherCards.PocketCards.Length > 0)
            {
                higherPocketCards = new PocketCards(higherCards.PocketCards[0], higherCards.PocketCards[1]);
            }
            var higherScore = CardEvaluation.ScoreCards(higherCards.CommunityCards, higherPocketCards);
            foreach (var lowerCards in straight)
            {
                var lowerCardsPocketCards = new PocketCards();
                if (lowerCards.PocketCards.Length > 0)
                {
                    lowerCardsPocketCards = new PocketCards(lowerCards.PocketCards[0], lowerCards.PocketCards[1]);
                }
                var lowerScore = CardEvaluation.ScoreCards(lowerCards.CommunityCards, lowerCardsPocketCards);
                Assert.True(higherScore > lowerScore);
                Assert.True(lowerScore < higherScore);
            }
        }
    }

    [Fact]
    public void TestStraightHigherThanTheeOfAKind()
    {
        (Card[] CommunityCards, Card[] PocketCards)[] straight = CardRankTestSamples.Samples[HandCardRank.Straight];
        (Card[] CommunityCards, Card[] PocketCards)[] threeOfaKind = CardRankTestSamples.Samples[HandCardRank.ThreeOfAKind];
        foreach (var higherCards in straight)
        {
            var higherPocketCards = new PocketCards();
            if (higherCards.PocketCards.Length > 0)
            {
                higherPocketCards = new PocketCards(higherCards.PocketCards[0], higherCards.PocketCards[1]);
            }
            var higherScore = CardEvaluation.ScoreCards(higherCards.CommunityCards, higherPocketCards);
            foreach (var lowerCards in threeOfaKind)
            {
                var lowerCardsPocketCards = new PocketCards();
                if (lowerCards.PocketCards.Length > 0)
                {
                    lowerCardsPocketCards = new PocketCards(lowerCards.PocketCards[0], lowerCards.PocketCards[1]);
                }
                var lowerScore = CardEvaluation.ScoreCards(lowerCards.CommunityCards, lowerCardsPocketCards);
                Assert.True(higherScore > lowerScore);
                Assert.True(lowerScore < higherScore);
            }
        }
    }

    [Fact]
    public void TestTheeOfAKindHigherThanTwoPairs()
    {
        (Card[] CommunityCards, Card[] PocketCards)[] threeOfaKind = CardRankTestSamples.Samples[HandCardRank.ThreeOfAKind];
        (Card[] CommunityCards, Card[] PocketCards)[] twoPairs = CardRankTestSamples.Samples[HandCardRank.TwoPairs];
        foreach (var higherCards in threeOfaKind)
        {
            var higherPocketCards = new PocketCards();
            if (higherCards.PocketCards.Length > 0)
            {
                higherPocketCards = new PocketCards(higherCards.PocketCards[0], higherCards.PocketCards[1]);
            }
            var higherScore = CardEvaluation.ScoreCards(higherCards.CommunityCards, higherPocketCards);
            foreach (var lowerCards in twoPairs)
            {
                var lowerCardsPocketCards = new PocketCards();
                if (lowerCards.PocketCards.Length > 0)
                {
                    lowerCardsPocketCards = new PocketCards(lowerCards.PocketCards[0], lowerCards.PocketCards[1]);
                }
                var lowerScore = CardEvaluation.ScoreCards(lowerCards.CommunityCards, lowerCardsPocketCards);
                Assert.True(higherScore > lowerScore);
                Assert.True(lowerScore < higherScore);
            }
        }
    }
    [Fact]
    public void TestTwoPairsHigherThanOnePair()
    {
        (Card[] CommunityCards, Card[] PocketCards)[] twoPairs = CardRankTestSamples.Samples[HandCardRank.TwoPairs];
        (Card[] CommunityCards, Card[] PocketCards)[] onePair = CardRankTestSamples.Samples[HandCardRank.OnePair];
        foreach (var higherCards in twoPairs)
        {
            var higherPocketCards = new PocketCards();
            if (higherCards.PocketCards.Length > 0)
            {
                higherPocketCards = new PocketCards(higherCards.PocketCards[0], higherCards.PocketCards[1]);
            }
            var higherScore = CardEvaluation.ScoreCards(higherCards.CommunityCards, higherPocketCards);
            foreach (var lowerCards in onePair)
            {
                var lowerCardsPocketCards = new PocketCards();
                if (lowerCards.PocketCards.Length > 0)
                {
                    lowerCardsPocketCards = new PocketCards(lowerCards.PocketCards[0], lowerCards.PocketCards[1]);
                }
                var lowerScore = CardEvaluation.ScoreCards(lowerCards.CommunityCards, lowerCardsPocketCards);
                Assert.True(higherScore > lowerScore);
                Assert.True(lowerScore < higherScore);
            }
        }
    }
    [Fact]
    public void TestonePairHigherThanHighCard()
    {
        (Card[] CommunityCards, Card[] PocketCards)[] onePair = CardRankTestSamples.Samples[HandCardRank.OnePair];
        (Card[] CommunityCards, Card[] PocketCards)[] highCard = CardRankTestSamples.Samples[HandCardRank.HighCard];
        foreach (var higherCards in onePair)
        {
            var higherPocketCards = new PocketCards();
            if (higherCards.PocketCards.Length > 0)
            {
                higherPocketCards = new PocketCards(higherCards.PocketCards[0], higherCards.PocketCards[1]);
            }
            var higherScore = CardEvaluation.ScoreCards(higherCards.CommunityCards, higherPocketCards);
            foreach (var lowerCards in highCard)
            {
                var lowerCardsPocketCards = new PocketCards();
                if (lowerCards.PocketCards.Length > 0)
                {
                    lowerCardsPocketCards = new PocketCards(lowerCards.PocketCards[0], lowerCards.PocketCards[1]);
                }
                var lowerScore = CardEvaluation.ScoreCards(lowerCards.CommunityCards, lowerCardsPocketCards);
                Assert.True(higherScore > lowerScore);
                Assert.True(lowerScore < higherScore);
            }
        }
    }

    [Fact]
    public void TestoneHighCardLowerThanRoyalFlush()
    {
        (Card[] CommunityCards, Card[] PocketCards)[] royalFlush = CardRankTestSamples.Samples[HandCardRank.RoyalFlush];
        (Card[] CommunityCards, Card[] PocketCards)[] highCard = CardRankTestSamples.Samples[HandCardRank.HighCard];
        foreach (var higherCards in royalFlush)
        {
            var higherPocketCards = new PocketCards();
            if (higherCards.PocketCards.Length > 0)
            {
                higherPocketCards = new PocketCards(higherCards.PocketCards[0], higherCards.PocketCards[1]);
            }
            var higherScore = CardEvaluation.ScoreCards(higherCards.CommunityCards, higherPocketCards);
            foreach (var lowerCards in highCard)
            {
                var lowerCardsPocketCards = new PocketCards();
                if (lowerCards.PocketCards.Length > 0)
                {
                    lowerCardsPocketCards = new PocketCards(lowerCards.PocketCards[0], lowerCards.PocketCards[1]);
                }
                var lowerScore = CardEvaluation.ScoreCards(lowerCards.CommunityCards, lowerCardsPocketCards);
                Assert.True(lowerScore < higherScore);
                Assert.True(higherScore > lowerScore);
            }
        }
    }

    [Fact]
    public void ScoreCards_ShouldIdentifyHighCard()
    {
        var communityCards = new Card[]
        {
            new Card(CardRank.Two, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Five, CardSuit.Clubs),
            new Card(CardRank.Seven, CardSuit.Spades),
            new Card(CardRank.Nine, CardSuit.Hearts)
        };
        var playerPocketCards = new PocketCards();
        playerPocketCards = new PocketCards(new Card(CardRank.Eight, CardSuit.Hearts), new Card(CardRank.Ace, CardSuit.Diamonds));

        var handScore = CardEvaluation.ScoreCards(communityCards, playerPocketCards);

        Assert.Equal(HandCardRank.HighCard, handScore.CardRank);
        // Assert that the highest card is correctly identified
    }
    [Fact]
    public void StraightFlushCardRanking()
    {
        var higherCommunityCards = new Card[]
        {
            new Card(CardRank.Two, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Hearts),
            new Card(CardRank.Seven, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Hearts)
        };
        var higherPocketCards = new PocketCards();
        higherPocketCards = new PocketCards(new Card(CardRank.Eight, CardSuit.Hearts), new Card(CardRank.Nine, CardSuit.Hearts));

        var higherScore = CardEvaluation.ScoreCards(higherCommunityCards, higherPocketCards);

        var lowerCommunityCards = new Card[]
        {
            new Card(CardRank.Two, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Five, CardSuit.Hearts),
            new Card(CardRank.Seven, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Hearts)
        };
        var lowerPocketCards = new PocketCards();
        lowerPocketCards = new PocketCards(new Card(CardRank.Eight, CardSuit.Hearts), new Card(CardRank.Ace, CardSuit.Diamonds));

        var lowerScore = CardEvaluation.ScoreCards(lowerCommunityCards, lowerPocketCards);

        Assert.True(higherScore > lowerScore);
        Assert.True(lowerScore < higherScore);
        // Assert that the highest card is correctly identified
    }
    [Fact]
    public void FourOfAKindCardRanking1()
    {
        var higherCommunityCards = new Card[]
        {
            new Card(CardRank.Ace, CardSuit.Hearts),
            new Card(CardRank.Ace, CardSuit.Spades),
            new Card(CardRank.Ace, CardSuit.Clubs),
            new Card(CardRank.Ace, CardSuit.Diamonds),
            new Card(CardRank.Six, CardSuit.Hearts)
        };
        var higherPocketCards = new PocketCards();
        higherPocketCards = new PocketCards(new Card(CardRank.Eight, CardSuit.Hearts), new Card(CardRank.King, CardSuit.Hearts));

        var higherScore = CardEvaluation.ScoreCards(higherCommunityCards, higherPocketCards);

        var lowerCommunityCards = new Card[]
        {
            new Card(CardRank.Ace, CardSuit.Hearts),
            new Card(CardRank.Ace, CardSuit.Spades),
            new Card(CardRank.Ace, CardSuit.Clubs),
            new Card(CardRank.Ace, CardSuit.Diamonds),
            new Card(CardRank.Six, CardSuit.Hearts)
        };
        var lowerPocketCards = new PocketCards();
        lowerPocketCards = new PocketCards(new Card(CardRank.Queen, CardSuit.Hearts), new Card(CardRank.Queen, CardSuit.Diamonds));

        var lowerScore = CardEvaluation.ScoreCards(lowerCommunityCards, lowerPocketCards);

        Assert.True(higherScore > lowerScore);
        Assert.True(lowerScore < higherScore);
        // Assert that the highest card is correctly identified
    }
    [Fact]
    public void FourOfAKindCardRanking2()
    {
        var higherCommunityCards = new Card[]
        {
            new Card(CardRank.Ace, CardSuit.Hearts),
            new Card(CardRank.Ace, CardSuit.Spades),
            new Card(CardRank.Ace, CardSuit.Clubs),
            new Card(CardRank.Ace, CardSuit.Diamonds),
            new Card(CardRank.Six, CardSuit.Hearts)
        };
        var higherPocketCards = new PocketCards();
        higherPocketCards = new PocketCards(new Card(CardRank.Eight, CardSuit.Hearts), new Card(CardRank.King, CardSuit.Hearts));

        var higherScore = CardEvaluation.ScoreCards(higherCommunityCards, higherPocketCards);

        var lowerCommunityCards = new Card[]
        {
            new Card(CardRank.Queen, CardSuit.Hearts),
            new Card(CardRank.Queen, CardSuit.Spades),
            new Card(CardRank.Queen, CardSuit.Clubs),
            new Card(CardRank.Queen, CardSuit.Diamonds),
            new Card(CardRank.Six, CardSuit.Hearts)
        };
        var lowerPocketCards = new PocketCards();
        lowerPocketCards = new PocketCards(new Card(CardRank.Eight, CardSuit.Hearts), new Card(CardRank.King, CardSuit.Diamonds));

        var lowerScore = CardEvaluation.ScoreCards(lowerCommunityCards, lowerPocketCards);

        Assert.True(higherScore > lowerScore);
        Assert.True(lowerScore < higherScore);
        // Assert that the highest card is correctly identified
    }
    [Fact]
    public void FullHouseCardRanking()
    {
        var higherCommunityCards = new Card[]
        {
            new Card(CardRank.Ace, CardSuit.Hearts),
            new Card(CardRank.Ace, CardSuit.Spades),
            new Card(CardRank.Ace, CardSuit.Clubs),
            new Card(CardRank.Six, CardSuit.Diamonds),
            new Card(CardRank.Six, CardSuit.Hearts)
        };
        var higherPocketCards = new PocketCards();
        higherPocketCards = new PocketCards(new Card(CardRank.Eight, CardSuit.Hearts), new Card(CardRank.King, CardSuit.Hearts));

        var higherScore = CardEvaluation.ScoreCards(higherCommunityCards, higherPocketCards);

        var lowerCommunityCards = new Card[]
        {
            new Card(CardRank.Six, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Spades),
            new Card(CardRank.Six, CardSuit.Clubs),
            new Card(CardRank.Ace, CardSuit.Diamonds),
            new Card(CardRank.Ace, CardSuit.Hearts)
        };
        var lowerPocketCards = new PocketCards();
        lowerPocketCards = new PocketCards(new Card(CardRank.Eight, CardSuit.Hearts), new Card(CardRank.King, CardSuit.Diamonds));

        var lowerScore = CardEvaluation.ScoreCards(lowerCommunityCards, lowerPocketCards);

        Assert.True(higherScore > lowerScore);
        Assert.True(lowerScore < higherScore);
        // Assert that the highest card is correctly identified
    }
    [Fact]
    public void Flushanking()
    {
        var higherCommunityCards = new Card[]
        {
            new Card(CardRank.Jack, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Spades),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Seven, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Hearts)
        };
        var higherPocketCards = new PocketCards();
        higherPocketCards = new PocketCards(new Card(CardRank.Eight, CardSuit.Hearts), new Card(CardRank.King, CardSuit.Hearts));

        var higherScore = CardEvaluation.ScoreCards(higherCommunityCards, higherPocketCards);

        var lowerCommunityCards = new Card[]
        {
            new Card(CardRank.Jack, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Spades),
            new Card(CardRank.Ace, CardSuit.Clubs),
            new Card(CardRank.Seven, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Hearts)
        };
        var lowerPocketCards = new PocketCards();
        lowerPocketCards = new PocketCards(new Card(CardRank.Eight, CardSuit.Hearts), new Card(CardRank.Queen, CardSuit.Hearts));

        var lowerScore = CardEvaluation.ScoreCards(lowerCommunityCards, lowerPocketCards);

        Assert.True(higherScore > lowerScore);
        Assert.True(lowerScore < higherScore);
        // Assert that the highest card is correctly identified
    }
    [Fact]
    public void StraightCardRanking()
    {
        var higherCommunityCards = new Card[]
        {
            new Card(CardRank.Two, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Hearts),
            new Card(CardRank.Seven, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Hearts)
        };
        var higherPocketCards = new PocketCards();
        higherPocketCards = new PocketCards(new Card(CardRank.Eight, CardSuit.Hearts), new Card(CardRank.Nine, CardSuit.Spades));

        var higherScore = CardEvaluation.ScoreCards(higherCommunityCards, higherPocketCards);

        var lowerCommunityCards = new Card[]
        {
            new Card(CardRank.Two, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Five, CardSuit.Diamonds),
            new Card(CardRank.Seven, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Hearts)
        };
        var lowerPocketCards = new PocketCards();
        lowerPocketCards = new PocketCards(new Card(CardRank.Eight, CardSuit.Hearts), new Card(CardRank.Ace, CardSuit.Diamonds));

        var lowerScore = CardEvaluation.ScoreCards(lowerCommunityCards, lowerPocketCards);

        Assert.True(higherScore > lowerScore);
        Assert.True(lowerScore < higherScore);
        // Assert that the highest card is correctly identified
    }
    [Fact]
    public void ThreeOfAKindCardRanking1()
    {
        var higherCommunityCards = new Card[]
        {
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Seven, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Hearts)
        };
        var higherPocketCards = new PocketCards();
        higherPocketCards = new PocketCards(new Card(CardRank.Eight, CardSuit.Hearts), new Card(CardRank.Nine, CardSuit.Spades));

        var higherScore = CardEvaluation.ScoreCards(higherCommunityCards, higherPocketCards);

        var lowerCommunityCards = new Card[]
        {
            new Card(CardRank.Two, CardSuit.Hearts),
            new Card(CardRank.Two, CardSuit.Clubs),
            new Card(CardRank.Two, CardSuit.Diamonds),
            new Card(CardRank.Seven, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Hearts)
        };
        var lowerPocketCards = new PocketCards();
        lowerPocketCards = new PocketCards(new Card(CardRank.Eight, CardSuit.Hearts), new Card(CardRank.Ace, CardSuit.Diamonds));

        var lowerScore = CardEvaluation.ScoreCards(lowerCommunityCards, lowerPocketCards);

        Assert.True(higherScore > lowerScore);
        Assert.True(lowerScore < higherScore);
        // Assert that the highest card is correctly identified
    }
    [Fact]
    public void ThreeOfAKindCardRanking2()
    {
        var higherCommunityCards = new Card[]
        {
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Seven, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Hearts)
        };
        var higherPocketCards = new PocketCards();
        higherPocketCards = new PocketCards(new Card(CardRank.King, CardSuit.Hearts), new Card(CardRank.Nine, CardSuit.Spades));

        var higherScore = CardEvaluation.ScoreCards(higherCommunityCards, higherPocketCards);

        var lowerCommunityCards = new Card[]
        {
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Seven, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Hearts)
        };
        var lowerPocketCards = new PocketCards();
        lowerPocketCards = new PocketCards(new Card(CardRank.Eight, CardSuit.Hearts), new Card(CardRank.King, CardSuit.Diamonds));

        var lowerScore = CardEvaluation.ScoreCards(lowerCommunityCards, lowerPocketCards);

        Assert.True(higherScore > lowerScore);
        Assert.True(lowerScore < higherScore);
        // Assert that the highest card is correctly identified
    }
    [Fact]
    public void TwoPairCardRanking1()
    {
        var higherCommunityCards = new Card[]
        {
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Two, CardSuit.Hearts),
            new Card(CardRank.Two, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Hearts)
        };
        var higherPocketCards = new PocketCards();
        higherPocketCards = new PocketCards(new Card(CardRank.King, CardSuit.Hearts), new Card(CardRank.Eight, CardSuit.Spades));

        var higherScore = CardEvaluation.ScoreCards(higherCommunityCards, higherPocketCards);

        var lowerCommunityCards = new Card[]
        {
            new Card(CardRank.Three, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Two, CardSuit.Hearts),
            new Card(CardRank.Two, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Hearts)
        };
        var lowerPocketCards = new PocketCards();
        lowerPocketCards = new PocketCards(new Card(CardRank.Eight, CardSuit.Hearts), new Card(CardRank.King, CardSuit.Diamonds));

        var lowerScore = CardEvaluation.ScoreCards(lowerCommunityCards, lowerPocketCards);

        Assert.True(higherScore > lowerScore);
        Assert.True(lowerScore < higherScore);
        // Assert that the highest card is correctly identified
    }
    [Fact]
    public void TwoPairCardRanking2()
    {
        var higherCommunityCards = new Card[]
        {
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Hearts)
        };
        var higherPocketCards = new PocketCards();
        higherPocketCards = new PocketCards(new Card(CardRank.King, CardSuit.Hearts), new Card(CardRank.Eight, CardSuit.Spades));

        var higherScore = CardEvaluation.ScoreCards(higherCommunityCards, higherPocketCards);

        var lowerCommunityCards = new Card[]
        {
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Two, CardSuit.Hearts),
            new Card(CardRank.Two, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Hearts)
        };
        var lowerPocketCards = new PocketCards();
        lowerPocketCards = new PocketCards(new Card(CardRank.Eight, CardSuit.Hearts), new Card(CardRank.King, CardSuit.Diamonds));

        var lowerScore = CardEvaluation.ScoreCards(lowerCommunityCards, lowerPocketCards);

        Assert.True(higherScore > lowerScore);
        Assert.True(lowerScore < higherScore);
        // Assert that the highest card is correctly identified
    }
    [Fact]
    public void TwoPairCardRanking3()
    {
        var higherCommunityCards = new Card[]
        {
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Hearts)
        };
        var higherPocketCards = new PocketCards();
        higherPocketCards = new PocketCards(new Card(CardRank.King, CardSuit.Hearts), new Card(CardRank.Nine, CardSuit.Spades));

        var higherScore = CardEvaluation.ScoreCards(higherCommunityCards, higherPocketCards);

        var lowerCommunityCards = new Card[]
        {
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Hearts)
        };
        var lowerPocketCards = new PocketCards();
        lowerPocketCards = new PocketCards(new Card(CardRank.Eight, CardSuit.Hearts), new Card(CardRank.Queen, CardSuit.Diamonds));

        var lowerScore = CardEvaluation.ScoreCards(lowerCommunityCards, lowerPocketCards);

        Assert.True(higherScore > lowerScore);
        Assert.True(lowerScore < higherScore);
        // Assert that the highest card is correctly identified
    }
    [Fact]
    public void OnePairCardRanking1()
    {
        var higherCommunityCards = new Card[]
        {
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Two, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Hearts)
        };
        var higherPocketCards = new PocketCards();
        higherPocketCards = new PocketCards(new Card(CardRank.King, CardSuit.Hearts), new Card(CardRank.Eight, CardSuit.Spades));

        var higherScore = CardEvaluation.ScoreCards(higherCommunityCards, higherPocketCards);

        var lowerCommunityCards = new Card[]
        {
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Three, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Two, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Hearts)
        };
        var lowerPocketCards = new PocketCards();
        lowerPocketCards = new PocketCards(new Card(CardRank.Eight, CardSuit.Hearts), new Card(CardRank.King, CardSuit.Diamonds));

        var lowerScore = CardEvaluation.ScoreCards(lowerCommunityCards, lowerPocketCards);

        Assert.True(higherScore > lowerScore);
        Assert.True(lowerScore < higherScore);
        // Assert that the highest card is correctly identified
    }
    [Fact]
    public void OnePairCardRanking2()
    {
        var higherCommunityCards = new Card[]
        {
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Two, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Hearts)
        };
        var higherPocketCards = new PocketCards();
        higherPocketCards = new PocketCards(new Card(CardRank.King, CardSuit.Hearts), new Card(CardRank.Nine, CardSuit.Spades));

        var higherScore = CardEvaluation.ScoreCards(higherCommunityCards, higherPocketCards);

        var lowerCommunityCards = new Card[]
        {
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Four, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.Two, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Hearts)
        };
        var lowerPocketCards = new PocketCards();
        lowerPocketCards = new PocketCards(new Card(CardRank.Eight, CardSuit.Hearts), new Card(CardRank.Queen, CardSuit.Diamonds));

        var lowerScore = CardEvaluation.ScoreCards(lowerCommunityCards, lowerPocketCards);

        Assert.True(higherScore > lowerScore);
        Assert.True(lowerScore < higherScore);
        // Assert that the highest card is correctly identified
    }
    [Fact]
    public void HighCardRanking()
    {
        var higherCommunityCards = new Card[]
        {
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Ten, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.King, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Hearts)
        };
        var higherPocketCards = new PocketCards();
        higherPocketCards = new PocketCards(new Card(CardRank.Two, CardSuit.Hearts), new Card(CardRank.Nine, CardSuit.Spades));

        var higherScore = CardEvaluation.ScoreCards(higherCommunityCards, higherPocketCards);

        var lowerCommunityCards = new Card[]
        {
            new Card(CardRank.Four, CardSuit.Diamonds),
            new Card(CardRank.Ten, CardSuit.Clubs),
            new Card(CardRank.Three, CardSuit.Hearts),
            new Card(CardRank.King, CardSuit.Hearts),
            new Card(CardRank.Six, CardSuit.Hearts)
        };
        var lowerPocketCards = new PocketCards();
        lowerPocketCards = new PocketCards(new Card(CardRank.Two, CardSuit.Hearts), new Card(CardRank.Seven, CardSuit.Diamonds));

        var lowerScore = CardEvaluation.ScoreCards(lowerCommunityCards, lowerPocketCards);

        Assert.True(higherScore > lowerScore);
        Assert.True(lowerScore < higherScore);
        // Assert that the highest card is correctly identified
    }
}