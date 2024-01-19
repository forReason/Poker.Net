using Poker.Net.PhysicalObjects.Cards;

namespace Poker.Tests.PhysicalObjects.Cards
{
    public class CardSuitTests
    {
        [Fact]
        public void CardSuits_AreUnique()
        {
            var suits = Enum.GetValues(typeof(CardSuit)).Cast<CardSuit>().ToList();
            var distinctSuits = suits.Distinct().ToList();
            Assert.Equal(distinctSuits.Count, suits.Count);
        }
        [Fact]
        public void CardSuits_AreFour()
        {
            var suits = Enum.GetValues(typeof(CardSuit)).Cast<CardSuit>().ToList();
            Assert.Equal(4, suits.Count);
        }
    }
}
