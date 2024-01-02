using NuGet.Frameworks;
using Xunit;
using Poker.Cards;
using Poker.Decks;
using Poker.Tests.TestAssets;

namespace Poker.Tests.Decks;
    
public class CardEvaluationTests
{
    [Fact]
    public void ScoreCards_ShouldIdentifyFullHouse()
    {
        var communityCards = new Card[]
        {
            new Card(Rank.Three, Suit.Diamonds),
            new Card(Rank.Two, Suit.Hearts),
            new Card(Rank.Queen, Suit.Hearts),
            new Card(Rank.Two, Suit.Spades),
            new Card(Rank.Three, Suit.Spades)
        };
        var playerPocketCards = new PocketCards();
        playerPocketCards.SetHand(new Card(Rank.Three, Suit.Hearts), new Card(Rank.Two, Suit.Clubs));

        var handScore = CardEvaluation.ScoreCards(communityCards, playerPocketCards);

        Assert.Equal(HandRank.FullHouse, handScore.Rank);
        // Add more assertions as needed
    }
    [Theory]
    [MemberData(nameof(CardRankTestSamples.GetSampleData), MemberType = typeof(CardRankTestSamples))]
    public void ScoreCards_ShouldCorrectlyIdentifyHand(HandRank expectedHandRank, Card[] communityCards, Card[] pocketCards)
    {
        // Arrange
        var playerPocketCards = new PocketCards();
        if (pocketCards.Length > 0)
        {
            playerPocketCards.SetHand(pocketCards[0], pocketCards[1]);
        }
        

        // Act
        var handScore = CardEvaluation.ScoreCards(communityCards, playerPocketCards);

        // Assert
        Assert.Equal(expectedHandRank, handScore.Rank);
        // Add more specific assertions if necessary
    }

    [Fact]
    public void TestRoyalFlushesSameRank()
    {
        (Card[] CommunityCards, Card[] PocketCards)[] samples = CardRankTestSamples.Samples[HandRank.RoyalFlush];
        (Card[] CommunityCards, Card[] PocketCards) sample1 = samples[0];
        var sample1PocketCards = new PocketCards();
        if (sample1.PocketCards.Length > 0)
        {
            sample1PocketCards.SetHand(sample1.PocketCards[0], sample1.PocketCards[1]);
        }
        var sample1Score = CardEvaluation.ScoreCards(sample1.CommunityCards, sample1PocketCards);
        foreach (var sample in samples)
        {
            var pocketCards = new PocketCards();
            if (sample.PocketCards.Length > 0)
            {
                pocketCards.SetHand(sample.PocketCards[0], sample.PocketCards[1]);
            }
            var sampleScore = CardEvaluation.ScoreCards(sample.CommunityCards, pocketCards);
            Assert.Equal(sample1Score, sampleScore);
        }
    }
    [Fact]
    public void TestRoyalFlushesHigherThanStraightFlushes()
    {
        (Card[] CommunityCards, Card[] PocketCards)[] royalFlushes = CardRankTestSamples.Samples[HandRank.RoyalFlush];
        (Card[] CommunityCards, Card[] PocketCards)[] straightFlushes = CardRankTestSamples.Samples[HandRank.StraightFlush];
        foreach (var royalFlush in royalFlushes)
        {
            var royalFlushPocketCards = new PocketCards();
            if (royalFlush.PocketCards.Length > 0)
            {
                royalFlushPocketCards.SetHand(royalFlush.PocketCards[0], royalFlush.PocketCards[1]);
            }
            var higherScore = CardEvaluation.ScoreCards(royalFlush.CommunityCards, royalFlushPocketCards);
            foreach (var straightFlush in straightFlushes)
            {
                var straightFlushPocketCards = new PocketCards();
                if (straightFlush.PocketCards.Length > 0)
                {
                    straightFlushPocketCards.SetHand(straightFlush.PocketCards[0], straightFlush.PocketCards[1]);
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
        (Card[] CommunityCards, Card[] PocketCards)[] fourOfAkinds = CardRankTestSamples.Samples[HandRank.FourOfAKind];
        (Card[] CommunityCards, Card[] PocketCards)[] straightFlushes = CardRankTestSamples.Samples[HandRank.StraightFlush];
        foreach (var straightFlush in straightFlushes)
        {
            var straightFlushPocketCards = new PocketCards();
            if (straightFlush.PocketCards.Length > 0)
            {
                straightFlushPocketCards.SetHand(straightFlush.PocketCards[0], straightFlush.PocketCards[1]);
            }
            var higherScore = CardEvaluation.ScoreCards(straightFlush.CommunityCards, straightFlushPocketCards);
            foreach (var fourOfAkind in fourOfAkinds)
            {
                var fourOfAkindPocketCards = new PocketCards();
                if (fourOfAkind.PocketCards.Length > 0)
                {
                    fourOfAkindPocketCards.SetHand(fourOfAkind.PocketCards[0], fourOfAkind.PocketCards[1]);
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
        (Card[] CommunityCards, Card[] PocketCards)[] fourOfKinds = CardRankTestSamples.Samples[HandRank.FourOfAKind];
        (Card[] CommunityCards, Card[] PocketCards)[] fullHouses = CardRankTestSamples.Samples[HandRank.FullHouse];
        foreach (var higherCards in fourOfKinds)
        {
            var higherPocketCards = new PocketCards();
            if (higherCards.PocketCards.Length > 0)
            {
                higherPocketCards.SetHand(higherCards.PocketCards[0], higherCards.PocketCards[1]);
            }
            var higherScore = CardEvaluation.ScoreCards(higherCards.CommunityCards, higherPocketCards);
            foreach (var lowerCards in fullHouses)
            {
                var lowerCardsPocketCards = new PocketCards();
                if (lowerCards.PocketCards.Length > 0)
                {
                    lowerCardsPocketCards.SetHand(lowerCards.PocketCards[0], lowerCards.PocketCards[1]);
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
        (Card[] CommunityCards, Card[] PocketCards)[] fullHouse = CardRankTestSamples.Samples[HandRank.FullHouse];
        (Card[] CommunityCards, Card[] PocketCards)[] flush = CardRankTestSamples.Samples[HandRank.Flush];
        foreach (var higherCards in fullHouse)
        {
            var higherPocketCards = new PocketCards();
            if (higherCards.PocketCards.Length > 0)
            {
                higherPocketCards.SetHand(higherCards.PocketCards[0], higherCards.PocketCards[1]);
            }
            var higherScore = CardEvaluation.ScoreCards(higherCards.CommunityCards, higherPocketCards);
            foreach (var lowerCards in flush)
            {
                var lowerCardsPocketCards = new PocketCards();
                if (lowerCards.PocketCards.Length > 0)
                {
                    lowerCardsPocketCards.SetHand(lowerCards.PocketCards[0], lowerCards.PocketCards[1]);
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
        (Card[] CommunityCards, Card[] PocketCards)[] flush = CardRankTestSamples.Samples[HandRank.Flush];
        (Card[] CommunityCards, Card[] PocketCards)[] straight = CardRankTestSamples.Samples[HandRank.Straight];
        foreach (var higherCards in flush)
        {
            var higherPocketCards = new PocketCards();
            if (higherCards.PocketCards.Length > 0)
            {
                higherPocketCards.SetHand(higherCards.PocketCards[0], higherCards.PocketCards[1]);
            }
            var higherScore = CardEvaluation.ScoreCards(higherCards.CommunityCards, higherPocketCards);
            foreach (var lowerCards in straight)
            {
                var lowerCardsPocketCards = new PocketCards();
                if (lowerCards.PocketCards.Length > 0)
                {
                    lowerCardsPocketCards.SetHand(lowerCards.PocketCards[0], lowerCards.PocketCards[1]);
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
        (Card[] CommunityCards, Card[] PocketCards)[] straight = CardRankTestSamples.Samples[HandRank.Straight];
        (Card[] CommunityCards, Card[] PocketCards)[] threeOfaKind = CardRankTestSamples.Samples[HandRank.ThreeOfAKind];
        foreach (var higherCards in straight)
        {
            var higherPocketCards = new PocketCards();
            if (higherCards.PocketCards.Length > 0)
            {
                higherPocketCards.SetHand(higherCards.PocketCards[0], higherCards.PocketCards[1]);
            }
            var higherScore = CardEvaluation.ScoreCards(higherCards.CommunityCards, higherPocketCards);
            foreach (var lowerCards in threeOfaKind)
            {
                var lowerCardsPocketCards = new PocketCards();
                if (lowerCards.PocketCards.Length > 0)
                {
                    lowerCardsPocketCards.SetHand(lowerCards.PocketCards[0], lowerCards.PocketCards[1]);
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
        (Card[] CommunityCards, Card[] PocketCards)[] threeOfaKind = CardRankTestSamples.Samples[HandRank.ThreeOfAKind];
        (Card[] CommunityCards, Card[] PocketCards)[] twoPairs = CardRankTestSamples.Samples[HandRank.TwoPairs];
        foreach (var higherCards in threeOfaKind)
        {
            var higherPocketCards = new PocketCards();
            if (higherCards.PocketCards.Length > 0)
            {
                higherPocketCards.SetHand(higherCards.PocketCards[0], higherCards.PocketCards[1]);
            }
            var higherScore = CardEvaluation.ScoreCards(higherCards.CommunityCards, higherPocketCards);
            foreach (var lowerCards in twoPairs)
            {
                var lowerCardsPocketCards = new PocketCards();
                if (lowerCards.PocketCards.Length > 0)
                {
                    lowerCardsPocketCards.SetHand(lowerCards.PocketCards[0], lowerCards.PocketCards[1]);
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
        (Card[] CommunityCards, Card[] PocketCards)[] twoPairs = CardRankTestSamples.Samples[HandRank.TwoPairs];
        (Card[] CommunityCards, Card[] PocketCards)[] onePair = CardRankTestSamples.Samples[HandRank.OnePair];
        foreach (var higherCards in twoPairs)
        {
            var higherPocketCards = new PocketCards();
            if (higherCards.PocketCards.Length > 0)
            {
                higherPocketCards.SetHand(higherCards.PocketCards[0], higherCards.PocketCards[1]);
            }
            var higherScore = CardEvaluation.ScoreCards(higherCards.CommunityCards, higherPocketCards);
            foreach (var lowerCards in onePair)
            {
                var lowerCardsPocketCards = new PocketCards();
                if (lowerCards.PocketCards.Length > 0)
                {
                    lowerCardsPocketCards.SetHand(lowerCards.PocketCards[0], lowerCards.PocketCards[1]);
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
        (Card[] CommunityCards, Card[] PocketCards)[] onePair = CardRankTestSamples.Samples[HandRank.OnePair];
        (Card[] CommunityCards, Card[] PocketCards)[] highCard = CardRankTestSamples.Samples[HandRank.HighCard];
        foreach (var higherCards in onePair)
        {
            var higherPocketCards = new PocketCards();
            if (higherCards.PocketCards.Length > 0)
            {
                higherPocketCards.SetHand(higherCards.PocketCards[0], higherCards.PocketCards[1]);
            }
            var higherScore = CardEvaluation.ScoreCards(higherCards.CommunityCards, higherPocketCards);
            foreach (var lowerCards in highCard)
            {
                var lowerCardsPocketCards = new PocketCards();
                if (lowerCards.PocketCards.Length > 0)
                {
                    lowerCardsPocketCards.SetHand(lowerCards.PocketCards[0], lowerCards.PocketCards[1]);
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
        (Card[] CommunityCards, Card[] PocketCards)[] royalFlush = CardRankTestSamples.Samples[HandRank.RoyalFlush];
        (Card[] CommunityCards, Card[] PocketCards)[] highCard = CardRankTestSamples.Samples[HandRank.HighCard];
        foreach (var higherCards in royalFlush)
        {
            var higherPocketCards = new PocketCards();
            if (higherCards.PocketCards.Length > 0)
            {
                higherPocketCards.SetHand(higherCards.PocketCards[0], higherCards.PocketCards[1]);
            }
            var higherScore = CardEvaluation.ScoreCards(higherCards.CommunityCards, higherPocketCards);
            foreach (var lowerCards in highCard)
            {
                var lowerCardsPocketCards = new PocketCards();
                if (lowerCards.PocketCards.Length > 0)
                {
                    lowerCardsPocketCards.SetHand(lowerCards.PocketCards[0], lowerCards.PocketCards[1]);
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
            new Card(Rank.Two, Suit.Hearts),
            new Card(Rank.Three, Suit.Diamonds),
            new Card(Rank.Five, Suit.Clubs),
            new Card(Rank.Seven, Suit.Spades),
            new Card(Rank.Nine, Suit.Hearts)
        };
        var playerPocketCards = new PocketCards();
        playerPocketCards.SetHand(new Card(Rank.Eight, Suit.Hearts), new Card(Rank.Ace, Suit.Diamonds));

        var handScore = CardEvaluation.ScoreCards(communityCards, playerPocketCards);

        Assert.Equal(HandRank.HighCard, handScore.Rank);
        // Assert that the highest card is correctly identified
    }
    [Fact]
    public void StraightFlushRanking()
    {
        var higherCommunityCards = new Card[]
        {
            new Card(Rank.Two, Suit.Hearts),
            new Card(Rank.Three, Suit.Hearts),
            new Card(Rank.Five, Suit.Hearts),
            new Card(Rank.Seven, Suit.Hearts),
            new Card(Rank.Six, Suit.Hearts)
        };
        var higherPocketCards = new PocketCards();
        higherPocketCards.SetHand(new Card(Rank.Eight, Suit.Hearts), new Card(Rank.Nine, Suit.Hearts));

        var higherScore = CardEvaluation.ScoreCards(higherCommunityCards, higherPocketCards);
        
        var lowerCommunityCards = new Card[]
        {
            new Card(Rank.Two, Suit.Hearts),
            new Card(Rank.Three, Suit.Hearts),
            new Card(Rank.Five, Suit.Hearts),
            new Card(Rank.Seven, Suit.Hearts),
            new Card(Rank.Six, Suit.Hearts)
        };
        var lowerPocketCards = new PocketCards();
        lowerPocketCards.SetHand(new Card(Rank.Eight, Suit.Hearts), new Card(Rank.Ace, Suit.Diamonds));

        var lowerScore = CardEvaluation.ScoreCards(lowerCommunityCards, lowerPocketCards);

        Assert.True(higherScore > lowerScore);
        Assert.True(lowerScore < higherScore);
        // Assert that the highest card is correctly identified
    }
    [Fact]
    public void FourOfAKindRanking1()
    {
        var higherCommunityCards = new Card[]
        {
            new Card(Rank.Ace, Suit.Hearts),
            new Card(Rank.Ace, Suit.Spades),
            new Card(Rank.Ace, Suit.Clubs),
            new Card(Rank.Ace, Suit.Diamonds),
            new Card(Rank.Six, Suit.Hearts)
        };
        var higherPocketCards = new PocketCards();
        higherPocketCards.SetHand(new Card(Rank.Eight, Suit.Hearts), new Card(Rank.King, Suit.Hearts));

        var higherScore = CardEvaluation.ScoreCards(higherCommunityCards, higherPocketCards);
        
        var lowerCommunityCards = new Card[]
        {
            new Card(Rank.Ace, Suit.Hearts),
            new Card(Rank.Ace, Suit.Spades),
            new Card(Rank.Ace, Suit.Clubs),
            new Card(Rank.Ace, Suit.Diamonds),
            new Card(Rank.Six, Suit.Hearts)
        };
        var lowerPocketCards = new PocketCards();
        lowerPocketCards.SetHand(new Card(Rank.Queen, Suit.Hearts), new Card(Rank.Queen, Suit.Diamonds));

        var lowerScore = CardEvaluation.ScoreCards(lowerCommunityCards, lowerPocketCards);

        Assert.True(higherScore > lowerScore);
        Assert.True(lowerScore < higherScore);
        // Assert that the highest card is correctly identified
    }
    [Fact]
    public void FourOfAKindRanking2()
    {
        var higherCommunityCards = new Card[]
        {
            new Card(Rank.Ace, Suit.Hearts),
            new Card(Rank.Ace, Suit.Spades),
            new Card(Rank.Ace, Suit.Clubs),
            new Card(Rank.Ace, Suit.Diamonds),
            new Card(Rank.Six, Suit.Hearts)
        };
        var higherPocketCards = new PocketCards();
        higherPocketCards.SetHand(new Card(Rank.Eight, Suit.Hearts), new Card(Rank.King, Suit.Hearts));

        var higherScore = CardEvaluation.ScoreCards(higherCommunityCards, higherPocketCards);
        
        var lowerCommunityCards = new Card[]
        {
            new Card(Rank.Queen, Suit.Hearts),
            new Card(Rank.Queen, Suit.Spades),
            new Card(Rank.Queen, Suit.Clubs),
            new Card(Rank.Queen, Suit.Diamonds),
            new Card(Rank.Six, Suit.Hearts)
        };
        var lowerPocketCards = new PocketCards();
        lowerPocketCards.SetHand(new Card(Rank.Eight, Suit.Hearts), new Card(Rank.King, Suit.Diamonds));

        var lowerScore = CardEvaluation.ScoreCards(lowerCommunityCards, lowerPocketCards);

        Assert.True(higherScore > lowerScore);
        Assert.True(lowerScore < higherScore);
        // Assert that the highest card is correctly identified
    }
    [Fact]
    public void FullHouseRanking()
    {
        var higherCommunityCards = new Card[]
        {
            new Card(Rank.Ace, Suit.Hearts),
            new Card(Rank.Ace, Suit.Spades),
            new Card(Rank.Ace, Suit.Clubs),
            new Card(Rank.Six, Suit.Diamonds),
            new Card(Rank.Six, Suit.Hearts)
        };
        var higherPocketCards = new PocketCards();
        higherPocketCards.SetHand(new Card(Rank.Eight, Suit.Hearts), new Card(Rank.King, Suit.Hearts));

        var higherScore = CardEvaluation.ScoreCards(higherCommunityCards, higherPocketCards);
        
        var lowerCommunityCards = new Card[]
        {
            new Card(Rank.Six, Suit.Hearts),
            new Card(Rank.Six, Suit.Spades),
            new Card(Rank.Six, Suit.Clubs),
            new Card(Rank.Ace, Suit.Diamonds),
            new Card(Rank.Ace, Suit.Hearts)
        };
        var lowerPocketCards = new PocketCards();
        lowerPocketCards.SetHand(new Card(Rank.Eight, Suit.Hearts), new Card(Rank.King, Suit.Diamonds));

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
            new Card(Rank.Jack, Suit.Hearts),
            new Card(Rank.Three, Suit.Spades),
            new Card(Rank.Four, Suit.Clubs),
            new Card(Rank.Seven, Suit.Hearts),
            new Card(Rank.Six, Suit.Hearts)
        };
        var higherPocketCards = new PocketCards();
        higherPocketCards.SetHand(new Card(Rank.Eight, Suit.Hearts), new Card(Rank.King, Suit.Hearts));

        var higherScore = CardEvaluation.ScoreCards(higherCommunityCards, higherPocketCards);
        
        var lowerCommunityCards = new Card[]
        {
            new Card(Rank.Jack, Suit.Hearts),
            new Card(Rank.Three, Suit.Spades),
            new Card(Rank.Ace, Suit.Clubs),
            new Card(Rank.Seven, Suit.Hearts),
            new Card(Rank.Six, Suit.Hearts)
        };
        var lowerPocketCards = new PocketCards();
        lowerPocketCards.SetHand(new Card(Rank.Eight, Suit.Hearts), new Card(Rank.Queen, Suit.Hearts));

        var lowerScore = CardEvaluation.ScoreCards(lowerCommunityCards, lowerPocketCards);

        Assert.True(higherScore > lowerScore);
        Assert.True(lowerScore < higherScore);
        // Assert that the highest card is correctly identified
    }
    [Fact]
    public void StraightRanking()
    {
        var higherCommunityCards = new Card[]
        {
            new Card(Rank.Two, Suit.Diamonds),
            new Card(Rank.Three, Suit.Clubs),
            new Card(Rank.Five, Suit.Hearts),
            new Card(Rank.Seven, Suit.Hearts),
            new Card(Rank.Six, Suit.Hearts)
        };
        var higherPocketCards = new PocketCards();
        higherPocketCards.SetHand(new Card(Rank.Eight, Suit.Hearts), new Card(Rank.Nine, Suit.Spades));

        var higherScore = CardEvaluation.ScoreCards(higherCommunityCards, higherPocketCards);
        
        var lowerCommunityCards = new Card[]
        {
            new Card(Rank.Two, Suit.Hearts),
            new Card(Rank.Three, Suit.Clubs),
            new Card(Rank.Five, Suit.Diamonds),
            new Card(Rank.Seven, Suit.Hearts),
            new Card(Rank.Six, Suit.Hearts)
        };
        var lowerPocketCards = new PocketCards();
        lowerPocketCards.SetHand(new Card(Rank.Eight, Suit.Hearts), new Card(Rank.Ace, Suit.Diamonds));

        var lowerScore = CardEvaluation.ScoreCards(lowerCommunityCards, lowerPocketCards);

        Assert.True(higherScore > lowerScore);
        Assert.True(lowerScore < higherScore);
        // Assert that the highest card is correctly identified
    }
    [Fact]
    public void ThreeOfAKindRanking1()
    {
        var higherCommunityCards = new Card[]
        {
            new Card(Rank.Three, Suit.Diamonds),
            new Card(Rank.Three, Suit.Clubs),
            new Card(Rank.Three, Suit.Hearts),
            new Card(Rank.Seven, Suit.Hearts),
            new Card(Rank.Six, Suit.Hearts)
        };
        var higherPocketCards = new PocketCards();
        higherPocketCards.SetHand(new Card(Rank.Eight, Suit.Hearts), new Card(Rank.Nine, Suit.Spades));

        var higherScore = CardEvaluation.ScoreCards(higherCommunityCards, higherPocketCards);
        
        var lowerCommunityCards = new Card[]
        {
            new Card(Rank.Two, Suit.Hearts),
            new Card(Rank.Two, Suit.Clubs),
            new Card(Rank.Two, Suit.Diamonds),
            new Card(Rank.Seven, Suit.Hearts),
            new Card(Rank.Six, Suit.Hearts)
        };
        var lowerPocketCards = new PocketCards();
        lowerPocketCards.SetHand(new Card(Rank.Eight, Suit.Hearts), new Card(Rank.Ace, Suit.Diamonds));

        var lowerScore = CardEvaluation.ScoreCards(lowerCommunityCards, lowerPocketCards);

        Assert.True(higherScore > lowerScore);
        Assert.True(lowerScore < higherScore);
        // Assert that the highest card is correctly identified
    }
    [Fact]
    public void ThreeOfAKindRanking2()
    {
        var higherCommunityCards = new Card[]
        {
            new Card(Rank.Three, Suit.Diamonds),
            new Card(Rank.Three, Suit.Clubs),
            new Card(Rank.Three, Suit.Hearts),
            new Card(Rank.Seven, Suit.Hearts),
            new Card(Rank.Six, Suit.Hearts)
        };
        var higherPocketCards = new PocketCards();
        higherPocketCards.SetHand(new Card(Rank.King, Suit.Hearts), new Card(Rank.Nine, Suit.Spades));

        var higherScore = CardEvaluation.ScoreCards(higherCommunityCards, higherPocketCards);
        
        var lowerCommunityCards = new Card[]
        {
            new Card(Rank.Three, Suit.Diamonds),
            new Card(Rank.Three, Suit.Clubs),
            new Card(Rank.Three, Suit.Hearts),
            new Card(Rank.Seven, Suit.Hearts),
            new Card(Rank.Six, Suit.Hearts)
        };
        var lowerPocketCards = new PocketCards();
        lowerPocketCards.SetHand(new Card(Rank.Eight, Suit.Hearts), new Card(Rank.King, Suit.Diamonds));

        var lowerScore = CardEvaluation.ScoreCards(lowerCommunityCards, lowerPocketCards);

        Assert.True(higherScore > lowerScore);
        Assert.True(lowerScore < higherScore);
        // Assert that the highest card is correctly identified
    }
    [Fact]
    public void TwoPairRanking1()
    {
        var higherCommunityCards = new Card[]
        {
            new Card(Rank.Four, Suit.Diamonds),
            new Card(Rank.Four, Suit.Clubs),
            new Card(Rank.Two, Suit.Hearts),
            new Card(Rank.Two, Suit.Hearts),
            new Card(Rank.Six, Suit.Hearts)
        };
        var higherPocketCards = new PocketCards();
        higherPocketCards.SetHand(new Card(Rank.King, Suit.Hearts), new Card(Rank.Eight, Suit.Spades));

        var higherScore = CardEvaluation.ScoreCards(higherCommunityCards, higherPocketCards);
        
        var lowerCommunityCards = new Card[]
        {
            new Card(Rank.Three, Suit.Diamonds),
            new Card(Rank.Three, Suit.Clubs),
            new Card(Rank.Two, Suit.Hearts),
            new Card(Rank.Two, Suit.Hearts),
            new Card(Rank.Six, Suit.Hearts)
        };
        var lowerPocketCards = new PocketCards();
        lowerPocketCards.SetHand(new Card(Rank.Eight, Suit.Hearts), new Card(Rank.King, Suit.Diamonds));

        var lowerScore = CardEvaluation.ScoreCards(lowerCommunityCards, lowerPocketCards);

        Assert.True(higherScore > lowerScore);
        Assert.True(lowerScore < higherScore);
        // Assert that the highest card is correctly identified
    }
    [Fact]
    public void TwoPairRanking2()
    {
        var higherCommunityCards = new Card[]
        {
            new Card(Rank.Four, Suit.Diamonds),
            new Card(Rank.Four, Suit.Clubs),
            new Card(Rank.Three, Suit.Hearts),
            new Card(Rank.Three, Suit.Hearts),
            new Card(Rank.Six, Suit.Hearts)
        };
        var higherPocketCards = new PocketCards();
        higherPocketCards.SetHand(new Card(Rank.King, Suit.Hearts), new Card(Rank.Eight, Suit.Spades));

        var higherScore = CardEvaluation.ScoreCards(higherCommunityCards, higherPocketCards);
        
        var lowerCommunityCards = new Card[]
        {
            new Card(Rank.Four, Suit.Diamonds),
            new Card(Rank.Four, Suit.Clubs),
            new Card(Rank.Two, Suit.Hearts),
            new Card(Rank.Two, Suit.Hearts),
            new Card(Rank.Six, Suit.Hearts)
        };
        var lowerPocketCards = new PocketCards();
        lowerPocketCards.SetHand(new Card(Rank.Eight, Suit.Hearts), new Card(Rank.King, Suit.Diamonds));

        var lowerScore = CardEvaluation.ScoreCards(lowerCommunityCards, lowerPocketCards);

        Assert.True(higherScore > lowerScore);
        Assert.True(lowerScore < higherScore);
        // Assert that the highest card is correctly identified
    }
    [Fact]
    public void TwoPairRanking3()
    {
        var higherCommunityCards = new Card[]
        {
            new Card(Rank.Four, Suit.Diamonds),
            new Card(Rank.Four, Suit.Clubs),
            new Card(Rank.Three, Suit.Hearts),
            new Card(Rank.Three, Suit.Hearts),
            new Card(Rank.Six, Suit.Hearts)
        };
        var higherPocketCards = new PocketCards();
        higherPocketCards.SetHand(new Card(Rank.King, Suit.Hearts), new Card(Rank.Nine, Suit.Spades));

        var higherScore = CardEvaluation.ScoreCards(higherCommunityCards, higherPocketCards);
        
        var lowerCommunityCards = new Card[]
        {
            new Card(Rank.Four, Suit.Diamonds),
            new Card(Rank.Four, Suit.Clubs),
            new Card(Rank.Three, Suit.Hearts),
            new Card(Rank.Three, Suit.Hearts),
            new Card(Rank.Six, Suit.Hearts)
        };
        var lowerPocketCards = new PocketCards();
        lowerPocketCards.SetHand(new Card(Rank.Eight, Suit.Hearts), new Card(Rank.Queen, Suit.Diamonds));

        var lowerScore = CardEvaluation.ScoreCards(lowerCommunityCards, lowerPocketCards);

        Assert.True(higherScore > lowerScore);
        Assert.True(lowerScore < higherScore);
        // Assert that the highest card is correctly identified
    }
    [Fact]
    public void OnePairRanking1()
    {
        var higherCommunityCards = new Card[]
        {
            new Card(Rank.Four, Suit.Diamonds),
            new Card(Rank.Four, Suit.Clubs),
            new Card(Rank.Three, Suit.Hearts),
            new Card(Rank.Two, Suit.Hearts),
            new Card(Rank.Six, Suit.Hearts)
        };
        var higherPocketCards = new PocketCards();
        higherPocketCards.SetHand(new Card(Rank.King, Suit.Hearts), new Card(Rank.Eight, Suit.Spades));

        var higherScore = CardEvaluation.ScoreCards(higherCommunityCards, higherPocketCards);
        
        var lowerCommunityCards = new Card[]
        {
            new Card(Rank.Four, Suit.Diamonds),
            new Card(Rank.Three, Suit.Clubs),
            new Card(Rank.Three, Suit.Hearts),
            new Card(Rank.Two, Suit.Hearts),
            new Card(Rank.Six, Suit.Hearts)
        };
        var lowerPocketCards = new PocketCards();
        lowerPocketCards.SetHand(new Card(Rank.Eight, Suit.Hearts), new Card(Rank.King, Suit.Diamonds));

        var lowerScore = CardEvaluation.ScoreCards(lowerCommunityCards, lowerPocketCards);

        Assert.True(higherScore > lowerScore);
        Assert.True(lowerScore < higherScore);
        // Assert that the highest card is correctly identified
    }
    [Fact]
    public void OnePairRanking2()
    {
        var higherCommunityCards = new Card[]
        {
            new Card(Rank.Four, Suit.Diamonds),
            new Card(Rank.Four, Suit.Clubs),
            new Card(Rank.Three, Suit.Hearts),
            new Card(Rank.Two, Suit.Hearts),
            new Card(Rank.Six, Suit.Hearts)
        };
        var higherPocketCards = new PocketCards();
        higherPocketCards.SetHand(new Card(Rank.King, Suit.Hearts), new Card(Rank.Nine, Suit.Spades));

        var higherScore = CardEvaluation.ScoreCards(higherCommunityCards, higherPocketCards);
        
        var lowerCommunityCards = new Card[]
        {
            new Card(Rank.Four, Suit.Diamonds),
            new Card(Rank.Four, Suit.Clubs),
            new Card(Rank.Three, Suit.Hearts),
            new Card(Rank.Two, Suit.Hearts),
            new Card(Rank.Six, Suit.Hearts)
        };
        var lowerPocketCards = new PocketCards();
        lowerPocketCards.SetHand(new Card(Rank.Eight, Suit.Hearts), new Card(Rank.Queen, Suit.Diamonds));

        var lowerScore = CardEvaluation.ScoreCards(lowerCommunityCards, lowerPocketCards);

        Assert.True(higherScore > lowerScore);
        Assert.True(lowerScore < higherScore);
        // Assert that the highest card is correctly identified
    }
    [Fact]
    public void HighCardranking()
    {
        var higherCommunityCards = new Card[]
        {
            new Card(Rank.Four, Suit.Diamonds),
            new Card(Rank.Ten, Suit.Clubs),
            new Card(Rank.Three, Suit.Hearts),
            new Card(Rank.King, Suit.Hearts),
            new Card(Rank.Six, Suit.Hearts)
        };
        var higherPocketCards = new PocketCards();
        higherPocketCards.SetHand(new Card(Rank.Two, Suit.Hearts), new Card(Rank.Nine, Suit.Spades));

        var higherScore = CardEvaluation.ScoreCards(higherCommunityCards, higherPocketCards);
        
        var lowerCommunityCards = new Card[]
        {
            new Card(Rank.Four, Suit.Diamonds),
            new Card(Rank.Ten, Suit.Clubs),
            new Card(Rank.Three, Suit.Hearts),
            new Card(Rank.King, Suit.Hearts),
            new Card(Rank.Six, Suit.Hearts)
        };
        var lowerPocketCards = new PocketCards();
        lowerPocketCards.SetHand(new Card(Rank.Two, Suit.Hearts), new Card(Rank.Seven, Suit.Diamonds));

        var lowerScore = CardEvaluation.ScoreCards(lowerCommunityCards, lowerPocketCards);

        Assert.True(higherScore > lowerScore);
        Assert.True(lowerScore < higherScore);
        // Assert that the highest card is correctly identified
    }
}