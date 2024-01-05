using Poker.PhysicalObjects.Cards;
using Poker.PhysicalObjects.Decks;

namespace Poker.Tests.PhysicalObjects.Decks
{
    public class HandScoreTests
    {
        [Fact]
        public void CompareHand_HigherRank_Returns1()
        {
            var hand1 = new HandScore ( HandCardRank.Flush, new[] { CardRank.Ten } );
            var hand2 = new HandScore ( HandCardRank.Straight, new[] { CardRank.Jack } );

            var result = hand1.CompareHand(hand2);

            Assert.Equal(1, result);
        }

        [Fact]
        public void CompareHand_LowerRank_ReturnsMinus1()
        {
            var hand1 = new HandScore( HandCardRank.OnePair,  new[] { CardRank.Two } );
            var hand2 = new HandScore ( HandCardRank.TwoPairs,  new[] { CardRank.Three } );

            var result = hand1.CompareHand(hand2);

            Assert.Equal(-1, result);
        }

        [Fact]
        public void CompareHand_SameRankHigherScore_Returns1()
        {
            var hand1 = new HandScore (HandCardRank.OnePair, new[] { CardRank.Ace } );            var hand2 = new HandScore (HandCardRank.OnePair, new[] { CardRank.King });

            var result = hand1.CompareHand(hand2);

            Assert.Equal(1, result);
        }

        [Fact]
        public void CompareHand_SameRankLowerScore_ReturnsMinus1()
        {
            var hand1 = new HandScore (HandCardRank.OnePair, new[] { CardRank.Two } );
            var hand2 = new HandScore (HandCardRank.OnePair, new[] { CardRank.Three } );

            var result = hand1.CompareHand(hand2);

            Assert.Equal(-1, result);
        }

        [Fact]
        public void CompareHand_SameRankSameScore_Returns0()
        {
            var hand1 = new HandScore (HandCardRank.OnePair,  new[] { CardRank.Two } );
            var hand2 = new HandScore (HandCardRank.OnePair,  new[] { CardRank.Two } );

            var result = hand1.CompareHand(hand2);

            Assert.Equal(0, result);
        }
    }
}
