using Poker.PhysicalObjects.Cards;
using Poker.PhysicalObjects.Decks;
using System.Collections.Generic;

namespace Poker.Tests.TestAssets;
public class CardRankTestSamples
{
    public static IEnumerable<object[]> GetSampleData()
    {
        foreach (var sample in Samples)
        {
            foreach (var hand in sample.Value)
            {
                yield return new object[] { sample.Key, hand.CommunityCards, hand.PocketCards };
            }
        }
    }
    public static Dictionary<HandCardRank, (Card[] CommunityCards, Card[] PocketCards)[]> Samples =
        new Dictionary<HandCardRank, (Card[] CommunityCards, Card[] PocketCards)[]>
        {
            {
                HandCardRank.RoyalFlush,
                [
                    // clean royal flush on table
                    ([Hearts(CardRank.Ten), Hearts(CardRank.Jack), Hearts(CardRank.Queen), Hearts(CardRank.King), Hearts(CardRank.Ace)], 
                        [Hearts(CardRank.Three), Hearts(CardRank.Two)]),
                    // 2 cards with different suit
                    ([Hearts(CardRank.Ten), Hearts(CardRank.Jack), Hearts(CardRank.Queen), Hearts(CardRank.King), Hearts(CardRank.Ace)], 
                        [Hearts(CardRank.Ace), Hearts(CardRank.King)]),
                    // 2 card with different suit and CardRank (one on hand
                    ([Hearts(CardRank.Ten), Hearts(CardRank.Jack), Hearts(CardRank.Queen), Hearts(CardRank.King), Diamonds(CardRank.Nine)], 
                        [Hearts(CardRank.Ace), Clubs(CardRank.Three)]),
                    // flop with royal flush
                    ([Hearts(CardRank.Ten), Hearts(CardRank.Jack), Hearts(CardRank.Queen)], 
                        [Hearts(CardRank.King), Hearts(CardRank.Ace)]),
                    // turn with royal flush
                    ([Hearts(CardRank.Ten), Hearts(CardRank.Jack), Hearts(CardRank.Queen),Hearts(CardRank.King)], 
                        [Spades(CardRank.Nine), Hearts(CardRank.Ace)]),
                    // Just Community Cards with royal flush
                    ([Hearts(CardRank.Ten), Hearts(CardRank.Jack), Hearts(CardRank.Queen), Hearts(CardRank.King), Hearts(CardRank.Ace)], 
                        []),
                    // Existing cases...
                    // Additional cases
                    // 1. Royal flush with one card in hand
                    ([Hearts(CardRank.Ten), Hearts(CardRank.Jack), Hearts(CardRank.Queen), Hearts(CardRank.King)],
                        [Hearts(CardRank.Ace), Diamonds(CardRank.Nine)]),
                    // 2. Royal flush with two cards in hand
                    ([Hearts(CardRank.Ten), Hearts(CardRank.Jack), Hearts(CardRank.Queen)],
                        [Hearts(CardRank.King), Hearts(CardRank.Ace)]),
                    // 3. Royal flush with mixed suits on the table
                    ([Diamonds(CardRank.Ten), Diamonds(CardRank.Jack), Diamonds(CardRank.Queen), Diamonds(CardRank.King), Spades(CardRank.Ace)],
                        [Diamonds(CardRank.Ace), Clubs(CardRank.King)]),
                    // 4. Royal flush with other high cards
                    ([Hearts(CardRank.Ten), Hearts(CardRank.Jack), Hearts(CardRank.Queen), Hearts(CardRank.King), Hearts(CardRank.Ace)],
                        [Diamonds(CardRank.King), Spades(CardRank.Queen)]),
                    // 5. Royal flush on turn
                    ([Hearts(CardRank.Ten), Hearts(CardRank.Jack), Hearts(CardRank.Queen), Hearts(CardRank.King)],
                        [Hearts(CardRank.Ace), Clubs(CardRank.Two)]),
                    // 6. Royal flush on flop, with high cards in hand
                    ([Hearts(CardRank.Ten), Hearts(CardRank.Jack), Hearts(CardRank.Queen)],
                        [Hearts(CardRank.King), Hearts(CardRank.Ace)]),
                    // 7. Royal flush with all community cards
                    ([Hearts(CardRank.Ten), Hearts(CardRank.Jack), Hearts(CardRank.Queen), Hearts(CardRank.King), Hearts(CardRank.Ace)],
                        [Diamonds(CardRank.Two), Clubs(CardRank.Three)]),
                    // 8. Royal flush with one pocket card
                    ([Hearts(CardRank.Ten), Hearts(CardRank.Jack), Hearts(CardRank.Queen), Hearts(CardRank.King)],
                        [Hearts(CardRank.Ace), Spades(CardRank.Four)]),
                    // 9. Mixed suits, but royal flush sequence
                    ([Diamonds(CardRank.Ten), Diamonds(CardRank.Jack), Diamonds(CardRank.Queen), Diamonds(CardRank.King), Diamonds(CardRank.Ace)],
                        [Hearts(CardRank.Three), Clubs(CardRank.Four)]),
                    // 10. Royal flush, other high cards in hand
                    ([Hearts(CardRank.Ten), Hearts(CardRank.Jack), Hearts(CardRank.Queen), Hearts(CardRank.King), Hearts(CardRank.Ace)],
                        [Diamonds(CardRank.King), Spades(CardRank.Queen)]),
                    // 11. Royal flush on turn, mixed suits
                    ([Diamonds(CardRank.Ten), Diamonds(CardRank.Jack), Diamonds(CardRank.Queen), Diamonds(CardRank.King)],
                        [Diamonds(CardRank.Ace), Clubs(CardRank.Two)]),
                    // 12. Royal flush with other suited cards
                    ([Hearts(CardRank.Ten), Hearts(CardRank.Jack), Hearts(CardRank.Queen), Hearts(CardRank.King), Hearts(CardRank.Ace)],
                        [Hearts(CardRank.Nine), Hearts(CardRank.Eight)]),
                    // 13. Royal flush, one card on hand, other suits
                    ([Clubs(CardRank.Ten), Clubs(CardRank.Jack), Clubs(CardRank.Queen), Clubs(CardRank.King)],
                        [Clubs(CardRank.Ace), Diamonds(CardRank.Seven)]),
                    // 14. Royal flush with high cards outside the sequence
                    ([Hearts(CardRank.Ten), Hearts(CardRank.Jack), Hearts(CardRank.Queen), Hearts(CardRank.King), Hearts(CardRank.Ace)],
                        [Diamonds(CardRank.Queen), Spades(CardRank.King)]),
                    // 15. Royal flush on flop, turn, and river
                    ([Clubs(CardRank.Ten), Clubs(CardRank.Jack), Clubs(CardRank.Queen)],
                        [Clubs(CardRank.King), Clubs(CardRank.Ace)]),
                    // 16. Mixed CardRanks with a royal flush sequence
                    ([Hearts(CardRank.Ten), Hearts(CardRank.Jack), Hearts(CardRank.Queen), Hearts(CardRank.King), Hearts(CardRank.Ace)],
                        [Clubs(CardRank.Nine), Diamonds(CardRank.Eight)]),
                    // 17. Royal flush, high cards in hand
                    ([Hearts(CardRank.Ten), Hearts(CardRank.Jack), Hearts(CardRank.Queen), Hearts(CardRank.King)],
                        [Hearts(CardRank.Ace), Diamonds(CardRank.King)]),
                    // 18. Royal flush with mixed suits, other high cards
                    ([Diamonds(CardRank.Ten), Diamonds(CardRank.Jack), Diamonds(CardRank.Queen), Diamonds(CardRank.King), Diamonds(CardRank.Ace)],
                        [Hearts(CardRank.Nine), Clubs(CardRank.Eight)]),
                    // 19. Royal flush with one card in hand, mixed suits
                    ([Hearts(CardRank.Ten), Hearts(CardRank.Jack), Hearts(CardRank.Queen), Hearts(CardRank.King)],
                        [Hearts(CardRank.Ace), Spades(CardRank.Four)]),
                    // 20. Royal flush, lowest CardRanks outside sequence
                    ([Hearts(CardRank.Ten), Hearts(CardRank.Jack), Hearts(CardRank.Queen), Hearts(CardRank.King), Hearts(CardRank.Ace)],
                        [Diamonds(CardRank.Two), Clubs(CardRank.Three)]),
                ]
            },
            {
                HandCardRank.StraightFlush,
                [
                    // highest straight flush on table
                    ([Hearts(CardRank.Ten), Hearts(CardRank.Jack), Hearts(CardRank.Queen), Hearts(CardRank.King), Hearts(CardRank.Nine)], 
                    [Hearts(CardRank.Three), Hearts(CardRank.Two)]),
                    // lowest straight flush without wheel
                    ([Hearts(CardRank.Ten), Hearts(CardRank.Four), Hearts(CardRank.Six), Hearts(CardRank.Five), Hearts(CardRank.Nine)], 
                        [Hearts(CardRank.Three), Hearts(CardRank.Two)]),
                    // lowest straight flush with wheel
                    ([Hearts(CardRank.Ten), Hearts(CardRank.Four), Hearts(CardRank.Ace), Hearts(CardRank.Five), Hearts(CardRank.Six)], 
                        [Hearts(CardRank.Three), Hearts(CardRank.Two)]),
                    // straight flush with other cards mixed in
                    ([Clubs(CardRank.Ten), Hearts(CardRank.Four), Hearts(CardRank.Ace), Hearts(CardRank.Five), Spades(CardRank.Six)], 
                        [Hearts(CardRank.Three), Hearts(CardRank.Two)]),
                    // flop
                    ([Hearts(CardRank.Ten), Hearts(CardRank.Jack), Hearts(CardRank.Queen)], 
                        [Hearts(CardRank.King), Hearts(CardRank.Nine)]),
                    // turn
                    ([Hearts(CardRank.Ten), Hearts(CardRank.Four), Hearts(CardRank.Five), Hearts(CardRank.Six)], 
                        [Hearts(CardRank.Three), Hearts(CardRank.Two)]),
                    // Existing cases...
                    // Additional cases
                    // 1. Straight flush ending at King
                    ([Diamonds(CardRank.Nine), Diamonds(CardRank.Ten), Diamonds(CardRank.Jack), Diamonds(CardRank.Queen), Diamonds(CardRank.King)],
                        [Diamonds(CardRank.Four), Diamonds(CardRank.Two)]),
                    // 2. Straight flush starting from Ace (wheel)
                    ([Clubs(CardRank.Ace), Clubs(CardRank.Two), Clubs(CardRank.Three), Clubs(CardRank.Four), Clubs(CardRank.Five)],
                        [Clubs(CardRank.Six), Clubs(CardRank.Seven)]),
                    // 3. Straight flush in the middle of the deck
                    ([Hearts(CardRank.Six), Hearts(CardRank.Seven), Hearts(CardRank.Eight), Hearts(CardRank.Nine), Hearts(CardRank.Ten)],
                        [Hearts(CardRank.Jack), Hearts(CardRank.Queen)]),
                    // 4. Straight flush with mixed suits but same CardRank
                    ([Spades(CardRank.Eight), Spades(CardRank.Nine), Spades(CardRank.Ten), Spades(CardRank.Jack), Spades(CardRank.Queen)],
                        [Diamonds(CardRank.King), Hearts(CardRank.Seven)]),
                    // 5. Straight flush on flop, turn, and river
                    ([Diamonds(CardRank.Seven), Diamonds(CardRank.Eight), Diamonds(CardRank.Nine)],
                        [Diamonds(CardRank.Ten), Diamonds(CardRank.Jack)]),
                    // 6. Lowest straight flush with wheel, other suits
                    ([Clubs(CardRank.Ace), Clubs(CardRank.Two), Clubs(CardRank.Three), Clubs(CardRank.Four), Clubs(CardRank.Five)],
                        [Hearts(CardRank.Six), Spades(CardRank.Seven)]),
                    // 7. Straight flush with other high cards
                    ([Spades(CardRank.Jack), Spades(CardRank.Queen), Spades(CardRank.King), Spades(CardRank.Three), Spades(CardRank.Ten)],
                        [Spades(CardRank.Nine), Spades(CardRank.Ten)]),
                    // 8. Straight flush on turn, other CardRanks
                    ([Hearts(CardRank.Four), Hearts(CardRank.Five), Hearts(CardRank.Six), Hearts(CardRank.Seven)],
                        [Hearts(CardRank.Three), Hearts(CardRank.Eight)]),
                    // 9. Mixed suits with a straight flush sequence
                    ([Diamonds(CardRank.Seven), Diamonds(CardRank.Eight), Diamonds(CardRank.Nine), Diamonds(CardRank.Ten), Diamonds(CardRank.Jack)],
                        [Clubs(CardRank.Queen), Spades(CardRank.King)]),
                    // 10. Straight flush, Ace high
                    ([Spades(CardRank.Four), Hearts(CardRank.King), Spades(CardRank.Queen), Spades(CardRank.Jack), Spades(CardRank.Ten)],
                        [Spades(CardRank.Nine), Spades(CardRank.Eight)]),
                    // 11. Straight flush, other suits
                    ([Clubs(CardRank.Nine), Clubs(CardRank.Eight), Clubs(CardRank.Seven), Clubs(CardRank.Six), Clubs(CardRank.Five)],
                        [Hearts(CardRank.Four), Diamonds(CardRank.Three)]),
                    // 12. Mixed CardRanks with a straight flush
                    ([Hearts(CardRank.Three), Hearts(CardRank.Four), Hearts(CardRank.Five), Hearts(CardRank.Six), Hearts(CardRank.Seven)],
                        [Spades(CardRank.Eight), Diamonds(CardRank.Nine)]),
                    // 13. Straight flush, low to high
                    ([Diamonds(CardRank.Two), Diamonds(CardRank.Three), Diamonds(CardRank.Four), Diamonds(CardRank.Five), Diamonds(CardRank.Six)],
                        [Clubs(CardRank.Seven), Spades(CardRank.Eight)]),
                    // 14. Straight flush with high card outside the sequence
                    ([Clubs(CardRank.Ten), Clubs(CardRank.Jack), Clubs(CardRank.Queen), Clubs(CardRank.King), Clubs(CardRank.Nine)],
                        [Diamonds(CardRank.Nine), Hearts(CardRank.Eight)]),
                    // 15. Straight flush, middle CardRanks
                    ([Spades(CardRank.Six), Spades(CardRank.Seven), Spades(CardRank.Eight), Spades(CardRank.Nine), Spades(CardRank.Ten)],
                        [Hearts(CardRank.Jack), Diamonds(CardRank.Queen)]),
                    // 16. Straight flush with mixed high cards
                    ([Hearts(CardRank.King), Hearts(CardRank.Queen), Hearts(CardRank.Jack), Hearts(CardRank.Ten), Hearts(CardRank.Nine)],
                        [Clubs(CardRank.Eight), Diamonds(CardRank.Seven)]),
                    // 17. Straight flush, low CardRanks
                    ([Diamonds(CardRank.Three), Diamonds(CardRank.Four), Diamonds(CardRank.Five), Diamonds(CardRank.Six), Diamonds(CardRank.Seven)],
                        [Hearts(CardRank.Two), Spades(CardRank.Ace)]),
                    // 18. Straight flush on flop, turn, river with mixed suits
                    ([Clubs(CardRank.Jack), Clubs(CardRank.Queen), Clubs(CardRank.King), Spades(CardRank.Ace), Clubs(CardRank.Ten)],
                        [Clubs(CardRank.Nine), Spades(CardRank.Eight)]),
                    // 19. Straight flush, other high cards
                    ([Spades(CardRank.Five), Spades(CardRank.Six), Spades(CardRank.Seven), Spades(CardRank.Eight), Spades(CardRank.Nine)],
                        [Diamonds(CardRank.Ten), Clubs(CardRank.Jack)]),
                    // 20. Mixed suits, straight flush sequence
                    ([Hearts(CardRank.Two), Hearts(CardRank.Three), Hearts(CardRank.Four), Hearts(CardRank.Five), Hearts(CardRank.Six)],
                        [Diamonds(CardRank.Seven), Clubs(CardRank.Eight)]),
                ]
            },
            {
                HandCardRank.FourOfAKind,
                [
                    // highest four of a kind
                    ([Hearts(CardRank.Ten), Hearts(CardRank.Ace), Hearts(CardRank.Queen), Spades(CardRank.Ace), Diamonds(CardRank.Ace)], 
                        [Hearts(CardRank.Three), Clubs(CardRank.Ace)]),
                    // lowest four of a kind
                    ([Clubs(CardRank.Two), Hearts(CardRank.Four), Spades(CardRank.Two), Hearts(CardRank.Five), Diamonds(CardRank.Two)], 
                        [Hearts(CardRank.Three), Hearts(CardRank.Two)]),
                    // flop
                    ([Hearts(CardRank.Ace), Spades(CardRank.Ace), Diamonds(CardRank.Ace)], 
                        [Hearts(CardRank.Three), Clubs(CardRank.Ace)]),
                    // turn
                    ([Clubs(CardRank.Two), Hearts(CardRank.Four), Spades(CardRank.Two), Diamonds(CardRank.Two)], 
                        [Hearts(CardRank.Three), Hearts(CardRank.Two)]),
                        // Existing cases...
                    // Additional cases
                    // 1. Four of a kind with highest kicker
                    ([Hearts(CardRank.King), Diamonds(CardRank.King), Clubs(CardRank.King), Spades(CardRank.King), Hearts(CardRank.Ace)], 
                        [Diamonds(CardRank.Queen), Spades(CardRank.Jack)]),
                    // 2. Four of a kind with lowest kicker
                    ([Clubs(CardRank.Three), Spades(CardRank.Three), Diamonds(CardRank.Three), Hearts(CardRank.Three), Spades(CardRank.Two)], 
                        [Hearts(CardRank.Four), Clubs(CardRank.Five)]),
                    // 3. Four of a kind on the flop
                    ([Hearts(CardRank.Jack), Diamonds(CardRank.Jack), Clubs(CardRank.Jack)], 
                        [Spades(CardRank.Jack), Hearts(CardRank.King)]),
                    // 4. Four of a kind on the turn
                    ([Clubs(CardRank.Six), Spades(CardRank.Six), Diamonds(CardRank.Six), Hearts(CardRank.Six)], 
                        [Hearts(CardRank.Seven), Spades(CardRank.Eight)]),
                    // 5. Four of a kind with one card in hand
                    ([Clubs(CardRank.Nine), Spades(CardRank.Nine), Diamonds(CardRank.Nine), Hearts(CardRank.Ten), Spades(CardRank.Queen)], 
                        [Hearts(CardRank.Nine), Diamonds(CardRank.King)]),
                    // 6. Four of a kind with both cards in hand
                    ([Hearts(CardRank.Seven), Diamonds(CardRank.Five), Clubs(CardRank.Ten), Spades(CardRank.Five)], 
                        [Hearts(CardRank.Five), Diamonds(CardRank.Five)]),
                    // 7. Four of a kind with mixed suits
                    ([Clubs(CardRank.Four), Spades(CardRank.Four), Diamonds(CardRank.Four), Hearts(CardRank.Six), Spades(CardRank.Seven)], 
                        [Hearts(CardRank.Four), Diamonds(CardRank.Eight)]),
                    // 8. Four of a kind with one card on the river
                    ([Hearts(CardRank.Ten), Diamonds(CardRank.Ace), Clubs(CardRank.Queen), Spades(CardRank.Ace)], 
                        [Hearts(CardRank.Ace), Diamonds(CardRank.Ace)]),
                    // 9. Four of a kind with scattered CardRanks
                    ([Clubs(CardRank.Eight), Spades(CardRank.Nine), Diamonds(CardRank.Ten), Hearts(CardRank.Jack), Spades(CardRank.Ten)], 
                        [Hearts(CardRank.Ten), Diamonds(CardRank.Ten)]),
                    // 10. Four of a kind with high and low cards
                    ([Hearts(CardRank.Two), Diamonds(CardRank.Ten), Clubs(CardRank.Six), Spades(CardRank.Eight), Hearts(CardRank.Ten)], 
                        [Diamonds(CardRank.Ten), Spades(CardRank.Ten)]),
                    // 11. Four of a kind with different CardRanks on the table
                    ([Clubs(CardRank.Queen), Spades(CardRank.King), Diamonds(CardRank.Ace), Hearts(CardRank.Ace), Spades(CardRank.Ace)], 
                        [Hearts(CardRank.Ace), Diamonds(CardRank.Jack)]),
                    // 12. Four of a kind with one pair on the flop
                    ([Hearts(CardRank.Seven), Diamonds(CardRank.Seven), Clubs(CardRank.Seven)], 
                        [Spades(CardRank.Seven), Clubs(CardRank.Eight)]),
                    // 13. Four of a kind with one card in hand and one on the river
                    ([Clubs(CardRank.Two), Spades(CardRank.Two), Diamonds(CardRank.Five), Hearts(CardRank.Six)], 
                        [Hearts(CardRank.Two), Diamonds(CardRank.Two)]),
                    // 14. Four of a kind with mixed high and low cards
                    ([Hearts(CardRank.Nine), Diamonds(CardRank.Jack), Clubs(CardRank.Queen), Spades(CardRank.King), Hearts(CardRank.Queen)], 
                        [Diamonds(CardRank.Queen), Spades(CardRank.Queen)]),
                    // 15. Four of a kind with scattered suits
                    ([Clubs(CardRank.Five), Spades(CardRank.Six), Diamonds(CardRank.Seven), Hearts(CardRank.Eight), Spades(CardRank.Five)], 
                        [Hearts(CardRank.Five), Diamonds(CardRank.Five)]),
                    // 16. Four of a kind with one pair in hand and one on the table
                    ([Hearts(CardRank.Two), Diamonds(CardRank.Four), Clubs(CardRank.Two)], 
                        [Spades(CardRank.Two), Hearts(CardRank.Two)]),
                    // 17. Four of a kind with low and high kickers
                    ([Clubs(CardRank.Jack), Spades(CardRank.Jack), Diamonds(CardRank.Jack), Hearts(CardRank.Two), Spades(CardRank.Three)], 
                        [Hearts(CardRank.Jack), Diamonds(CardRank.Four)]),
                    // 18. Four of a kind with one card on the flop and one on the turn
                    ([Hearts(CardRank.Seven), Diamonds(CardRank.Eight), Clubs(CardRank.Seven)], 
                        [Spades(CardRank.Seven), Hearts(CardRank.Seven)]),
                    // 19. Four of a kind with scattered high cards
                    ([Clubs(CardRank.Ten), Spades(CardRank.Jack), Diamonds(CardRank.Queen), Hearts(CardRank.King), Spades(CardRank.Ten)], 
                        [Hearts(CardRank.Ten), Diamonds(CardRank.Ten)]),
                    // 20. Four of a kind with various suits and CardRanks
                    ([Hearts(CardRank.Three), Diamonds(CardRank.Five), Clubs(CardRank.Seven), Spades(CardRank.Nine), Hearts(CardRank.Five)], 
                        [Diamonds(CardRank.Five), Spades(CardRank.Five)]),
                ]
            },
            {
                HandCardRank.FullHouse,
                [
                    // highest FullHouse
                    ([Hearts(CardRank.King), Hearts(CardRank.Ace), Hearts(CardRank.Queen), Spades(CardRank.Ace), Spades(CardRank.King)], 
                        [Hearts(CardRank.Three), Clubs(CardRank.Ace)]),
                    // lowest FullHouse
                    ([Diamonds(CardRank.Three), Hearts(CardRank.Two), Hearts(CardRank.Queen), Spades(CardRank.Two), Spades(CardRank.Three)], 
                        [Hearts(CardRank.Three), Clubs(CardRank.Two)]),
                    // Existing cases...
                    // Additional cases
                    // 1. Full House with highest three-of-a-kind and highest pair
                    ([Hearts(CardRank.Ace), Diamonds(CardRank.Ace), Clubs(CardRank.Ace), Spades(CardRank.King), Hearts(CardRank.King)], 
                        [Diamonds(CardRank.Queen), Spades(CardRank.Jack)]),
                    // 2. Full House with lowest three-of-a-kind and lowest pair
                    ([Clubs(CardRank.Two), Spades(CardRank.Two), Diamonds(CardRank.Two), Hearts(CardRank.Three), Spades(CardRank.Three)], 
                        [Hearts(CardRank.Four), Clubs(CardRank.Five)]),
                    // 3. Full House formed on the flop
                    ([Hearts(CardRank.Ace), Diamonds(CardRank.Ace), Clubs(CardRank.Ace)], 
                        [Spades(CardRank.King), Hearts(CardRank.King)]),
                    // 4. Full House formed on the turn
                    ([Clubs(CardRank.Two), Spades(CardRank.Two), Diamonds(CardRank.Two), Hearts(CardRank.Three)], 
                        [Spades(CardRank.Three), Clubs(CardRank.Four)]),
                    // 5. Full House with mixed CardRanks
                    ([Hearts(CardRank.Six), Diamonds(CardRank.Six), Clubs(CardRank.Six), Spades(CardRank.Seven), Hearts(CardRank.Seven)], 
                        [Diamonds(CardRank.Eight), Spades(CardRank.Nine)]),
                    // 6. Full House with three-of-a-kind on table
                    ([Hearts(CardRank.Jack), Diamonds(CardRank.Jack), Clubs(CardRank.Jack), Spades(CardRank.Ten), Hearts(CardRank.Nine)], 
                        [Diamonds(CardRank.Ten), Spades(CardRank.Ten)]),
                    // 7. Full House with pair in hand
                    ([Clubs(CardRank.Four), Spades(CardRank.Five), Diamonds(CardRank.Five), Hearts(CardRank.Six), Spades(CardRank.Seven)], 
                        [Hearts(CardRank.Four), Diamonds(CardRank.Four)]),
                    // 8. Full House with different suits
                    ([Hearts(CardRank.Eight), Diamonds(CardRank.Eight), Clubs(CardRank.Eight), Spades(CardRank.Nine), Hearts(CardRank.Ten)], 
                        [Diamonds(CardRank.Nine), Spades(CardRank.Nine)]),
                    // 9. Full House with one pair on the river
                    ([Clubs(CardRank.Queen), Spades(CardRank.Queen), Diamonds(CardRank.King)], 
                        [Hearts(CardRank.Queen), Spades(CardRank.King)]),
                    // 10. Full House with Ace pocket
                    ([Hearts(CardRank.Two), Diamonds(CardRank.Two), Clubs(CardRank.Four), Spades(CardRank.Five), Clubs(CardRank.Ace)], 
                        [Hearts(CardRank.Ace), Diamonds(CardRank.Ace)]),
                    // 11. Full House with high pair on table
                    ([Clubs(CardRank.Jack), Spades(CardRank.Jack), Diamonds(CardRank.Ten), Hearts(CardRank.Ten), Spades(CardRank.Nine)], 
                        [Hearts(CardRank.Jack), Diamonds(CardRank.Seven)]),
                    // 12. Full House with low pair in hand
                    ([Hearts(CardRank.Five), Diamonds(CardRank.Six), Clubs(CardRank.Seven), Spades(CardRank.Six)], 
                        [Hearts(CardRank.Five), Diamonds(CardRank.Five)]),
                    // 13. Full House with scattered CardRanks
                    ([Clubs(CardRank.Nine), Spades(CardRank.Eight), Diamonds(CardRank.Seven), Hearts(CardRank.Nine), Spades(CardRank.Five)], 
                        [Hearts(CardRank.Seven), Diamonds(CardRank.Seven)]),
                    // 14. Full House with three-of-a-kind on the flop
                    ([Hearts(CardRank.King), Diamonds(CardRank.King), Clubs(CardRank.King)], 
                        [Spades(CardRank.Queen), Hearts(CardRank.Queen)]),
                    // 15. Full House with pair on the turn
                    ([Clubs(CardRank.Three), Spades(CardRank.Three), Diamonds(CardRank.Three), Hearts(CardRank.Four)], 
                        [Spades(CardRank.Two), Clubs(CardRank.Two)]),
                    // 16. Full House with highest three-of-a-kind and lowest pair
                    ([Hearts(CardRank.Ace), Diamonds(CardRank.Ace), Clubs(CardRank.Ace), Spades(CardRank.Two), Hearts(CardRank.Two)], 
                        [Diamonds(CardRank.Three), Spades(CardRank.Four)]),
                    // 17. Full House with lowest three-of-a-kind and highest pair
                    ([Clubs(CardRank.Two), Spades(CardRank.Two), Diamonds(CardRank.Two), Hearts(CardRank.King), Spades(CardRank.King)], 
                        [Hearts(CardRank.Queen), Clubs(CardRank.Jack)]),
                    // 18. Full House with three-of-a-kind and pair of different CardRanks
                    ([Hearts(CardRank.Seven), Diamonds(CardRank.Seven), Clubs(CardRank.Seven), Spades(CardRank.Eight), Hearts(CardRank.Eight)], 
                        [Diamonds(CardRank.Nine), Spades(CardRank.Ten)]),
                    // 19. Full House with mixed high and low cards
                    ([Hearts(CardRank.Two), Diamonds(CardRank.Four), Clubs(CardRank.Six), Spades(CardRank.Six), Hearts(CardRank.Six)], 
                        [Diamonds(CardRank.Two), Spades(CardRank.Two)]),
                    // 20. Full House with one pair in hand and one on table
                    ([Hearts(CardRank.Three), Diamonds(CardRank.Seven), Clubs(CardRank.Four), Spades(CardRank.Four)], 
                        [Hearts(CardRank.Three), Diamonds(CardRank.Three)]),
                ]
            },
            {
                HandCardRank.Flush,
                [
                    // highest Flush
                    ([Hearts(CardRank.King), Hearts(CardRank.Ace), Hearts(CardRank.Queen), Spades(CardRank.Ace), Spades(CardRank.King)], 
                        [Hearts(CardRank.Jack), Hearts(CardRank.Nine)]),
                    // lowest Flush
                    ([Hearts(CardRank.Three), Hearts(CardRank.Two), Hearts(CardRank.Seven), Hearts(CardRank.Five), Spades(CardRank.Three)], 
                        [Hearts(CardRank.Four), Clubs(CardRank.Two)]),
                    // flop
                    ([Hearts(CardRank.King), Hearts(CardRank.Ace), Hearts(CardRank.Queen)], 
                    [Hearts(CardRank.Jack), Hearts(CardRank.Nine)]),
                    // turn
                    ([Hearts(CardRank.Three), Hearts(CardRank.Two), Hearts(CardRank.Seven), Hearts(CardRank.Five)], 
                        [Hearts(CardRank.Four), Clubs(CardRank.Two)]),
                        // Existing cases...
                    // Additional cases
                    // 1. Flush with mixed high and low cards
                    ([Hearts(CardRank.King), Hearts(CardRank.Nine), Hearts(CardRank.Three), Hearts(CardRank.Five), Hearts(CardRank.Two)], 
                        [Hearts(CardRank.Six), Clubs(CardRank.Seven)]),
                    // 2. Flush with highest card in hand
                    ([Hearts(CardRank.Queen), Hearts(CardRank.Four), Hearts(CardRank.Ten), Hearts(CardRank.Nine), Spades(CardRank.King)], 
                        [Hearts(CardRank.Ace), Diamonds(CardRank.Two)]),
                    // 3. Flush formed on the flop
                    ([Hearts(CardRank.Ace), Hearts(CardRank.King), Hearts(CardRank.Queen)], 
                        [Hearts(CardRank.Jack), Hearts(CardRank.Nine)]),
                    // 4. Flush formed on the turn
                    ([Hearts(CardRank.Four), Hearts(CardRank.Seven), Hearts(CardRank.Eight), Hearts(CardRank.Nine)], 
                        [Hearts(CardRank.Ten), Clubs(CardRank.Two)]),
                    // 5. Flush with one card on the river
                    ([Hearts(CardRank.Five), Hearts(CardRank.Six), Hearts(CardRank.Seven), Hearts(CardRank.Seven)], 
                        [Hearts(CardRank.Nine), Spades(CardRank.King)]),
                    // 6. Flush with scattered CardRanks
                    ([Hearts(CardRank.Three), Hearts(CardRank.Six), Hearts(CardRank.Ten), Hearts(CardRank.Queen), Spades(CardRank.Ace)], 
                        [Hearts(CardRank.King), Diamonds(CardRank.Two)]),
                    // 7. Flush with one pair in hand
                    ([Hearts(CardRank.Four), Hearts(CardRank.Eight), Hearts(CardRank.Nine), Hearts(CardRank.Ten)], 
                        [Hearts(CardRank.Ten), Hearts(CardRank.Queen)]),
                    // 8. Flush with high cards on table
                    ([Hearts(CardRank.Ace), Hearts(CardRank.King), Hearts(CardRank.Queen), Hearts(CardRank.Nine), Diamonds(CardRank.Ten)], 
                        [Hearts(CardRank.Nine), Clubs(CardRank.Eight)]),
                    // 9. Flush with mixed suits on table
                    ([Hearts(CardRank.Seven), Hearts(CardRank.Eight), Hearts(CardRank.Eight), Spades(CardRank.Jack), Hearts(CardRank.Queen)], 
                        [Hearts(CardRank.Ten), Clubs(CardRank.Ace)]),
                    // 10. Flush with lowest card in hand
                    ([Hearts(CardRank.Two), Hearts(CardRank.Three), Hearts(CardRank.Four), Hearts(CardRank.Five)], 
                        [Hearts(CardRank.Five), Diamonds(CardRank.Seven)]),
                    // 11. Flush with high community cards
                    ([Hearts(CardRank.Jack), Hearts(CardRank.Queen), Hearts(CardRank.Seven), Hearts(CardRank.Ace), Spades(CardRank.Ten)], 
                        [Hearts(CardRank.Nine), Clubs(CardRank.Eight)]),
                    // 12. Flush with low community cards
                    ([Hearts(CardRank.Two), Hearts(CardRank.Three), Hearts(CardRank.Four), Hearts(CardRank.Nine), Spades(CardRank.Six)], 
                        [Hearts(CardRank.Seven), Diamonds(CardRank.Eight)]),
                    // 13. Flush with one kicker card
                    ([Hearts(CardRank.Nine), Hearts(CardRank.Ten), Hearts(CardRank.Four), Hearts(CardRank.Queen), Diamonds(CardRank.King)], 
                        [Hearts(CardRank.Ace), Clubs(CardRank.Two)]),
                    // 14. Flush with all community cards
                    ([Hearts(CardRank.Ace), Hearts(CardRank.King), Hearts(CardRank.Seven), Hearts(CardRank.Jack), Hearts(CardRank.Ten)], 
                        [Diamonds(CardRank.Three), Clubs(CardRank.Four)]),
                    // 15. Flush with mixed CardRanks in hand
                    ([Hearts(CardRank.Five), Hearts(CardRank.Six), Hearts(CardRank.Two), Hearts(CardRank.Eight)], 
                        [Hearts(CardRank.Nine), Hearts(CardRank.Ten)]),
                    // 16. Flush with one card on the flop
                    ([Hearts(CardRank.Ace), Hearts(CardRank.King), Hearts(CardRank.Queen)], 
                        [Hearts(CardRank.Jack), Hearts(CardRank.Nine)]),
                    // 17. Flush with one card on the turn
                    ([Hearts(CardRank.Two), Hearts(CardRank.Three), Hearts(CardRank.Eight), Hearts(CardRank.Five)], 
                        [Hearts(CardRank.Six), Clubs(CardRank.Seven)]),
                    // 18. Flush with one card on the river
                    ([Hearts(CardRank.Five), Hearts(CardRank.Six), Hearts(CardRank.Seven), Hearts(CardRank.Eight)], 
                        [Hearts(CardRank.Jack), Spades(CardRank.Ten)]),
                    // 19. Flush with two cards in hand
                    ([Hearts(CardRank.Seven), Hearts(CardRank.Eight), Hearts(CardRank.Nine), Spades(CardRank.Ten)], 
                        [Hearts(CardRank.Jack), Hearts(CardRank.Queen)]),
                    // 20. Flush with mixed high and low cards on table
                    ([Hearts(CardRank.Two), Hearts(CardRank.Four), Hearts(CardRank.Six), Hearts(CardRank.Eight), Hearts(CardRank.Ten)], 
                        [Hearts(CardRank.Ace), Clubs(CardRank.Three)]),
                ]
            },
            {
                HandCardRank.Straight,
                [
                    // highest Straight
                    ([Hearts(CardRank.Ten), Spades(CardRank.Jack), Hearts(CardRank.Queen), Clubs(CardRank.King), Hearts(CardRank.Ace)], 
                        [Diamonds(CardRank.Three), Hearts(CardRank.Two)]),
                    // lowest Straight without wheel
                    ([Hearts(CardRank.Ten), Diamonds(CardRank.Four), Hearts(CardRank.Six), Clubs(CardRank.Five), Diamonds(CardRank.Nine)], 
                        [Hearts(CardRank.Three), Spades(CardRank.Two)]),
                    // lowest Straight with wheel
                    ([Hearts(CardRank.Ten), Diamonds(CardRank.Jack), Hearts(CardRank.Queen), Clubs(CardRank.King), Diamonds(CardRank.Ace)], 
                        [Hearts(CardRank.Three), Hearts(CardRank.Two)]),
                    // straight with other cards mixed in
                    ([Clubs(CardRank.Ten), Diamonds(CardRank.Four), Hearts(CardRank.Ace), Hearts(CardRank.Five), Spades(CardRank.Ten)], 
                        [Hearts(CardRank.Three), Hearts(CardRank.Two)]),
                    // flop
                    ([Hearts(CardRank.Ten), Spades(CardRank.Jack), Hearts(CardRank.Queen)], 
                        [Hearts(CardRank.King), Hearts(CardRank.Ace)]),
                    // turn
                    ([Clubs(CardRank.Ten), Diamonds(CardRank.Four),  Hearts(CardRank.Five), Spades(CardRank.Six)], 
                        [Hearts(CardRank.Three), Hearts(CardRank.Two)]),
                    // Existing cases...CardRank
                    // Additional cases
                    // 1. Straight in sequential order
                    ([Hearts(CardRank.Seven), Diamonds(CardRank.Eight), Clubs(CardRank.Nine), Spades(CardRank.Ten), Hearts(CardRank.Jack)], 
                        [Diamonds(CardRank.Queen), Spades(CardRank.King)]),
                    // 2. Straight with mixed suits
                    ([Hearts(CardRank.Five), Diamonds(CardRank.Six), Clubs(CardRank.Seven), Spades(CardRank.Eight), Hearts(CardRank.Nine)], 
                        [Clubs(CardRank.Ten), Diamonds(CardRank.Jack)]),
                    // 3. Straight with low-high order
                    ([Hearts(CardRank.Two), Diamonds(CardRank.Three), Clubs(CardRank.Four), Spades(CardRank.Five), Hearts(CardRank.Six)], 
                        [Diamonds(CardRank.Seven), Spades(CardRank.Eight)]),
                    // 4. Straight with gaps in the middle
                    ([Hearts(CardRank.Ten), Diamonds(CardRank.Jack), Clubs(CardRank.Queen), Spades(CardRank.King), Diamonds(CardRank.Ace)], 
                        [Hearts(CardRank.Nine), Clubs(CardRank.Eight)]),
                    // 5. Straight formed on the flop
                    ([Hearts(CardRank.Nine), Diamonds(CardRank.Ten), Clubs(CardRank.Jack)], 
                        [Spades(CardRank.Queen), Hearts(CardRank.King)]),
                    // 6. Straight formed on the turn
                    ([Hearts(CardRank.Eight), Diamonds(CardRank.Nine), Clubs(CardRank.Ten), Spades(CardRank.Jack)], 
                        [Diamonds(CardRank.Queen), Hearts(CardRank.King)]),
                    // 7. Straight with highest card in hand
                    ([Clubs(CardRank.Nine), Diamonds(CardRank.Ten), Hearts(CardRank.Jack), Spades(CardRank.Queen)], 
                        [Hearts(CardRank.King), Diamonds(CardRank.Ace)]),
                    // 8. Straight with one card on the river
                    ([Hearts(CardRank.Seven), Diamonds(CardRank.Eight), Clubs(CardRank.Nine), Spades(CardRank.Ten)], 
                        [Diamonds(CardRank.Jack), Hearts(CardRank.Queen)]),
                    // 9. Straight with mixed CardRanks on table
                    ([Clubs(CardRank.Four), Diamonds(CardRank.Five), Hearts(CardRank.Six), Spades(CardRank.Seven), Diamonds(CardRank.Eight)], 
                        [Hearts(CardRank.Ten), Spades(CardRank.Jack)]),
                    // 10. Straight with both hole cards used
                    ([Clubs(CardRank.Three), Diamonds(CardRank.Four), Hearts(CardRank.Six)], 
                        [Spades(CardRank.Five), Diamonds(CardRank.Seven)]),
                    // 11. Straight with all community cards
                    ([Hearts(CardRank.Two), Diamonds(CardRank.Three), Clubs(CardRank.Four), Spades(CardRank.Five), Diamonds(CardRank.Six)], 
                        [Hearts(CardRank.Eight), Spades(CardRank.Ten)]),
                    // 12. Straight with low cards on table
                    ([Hearts(CardRank.Ace), Diamonds(CardRank.Two), Clubs(CardRank.Three), Spades(CardRank.Four), Diamonds(CardRank.Five)], 
                        [Hearts(CardRank.Seven), Spades(CardRank.Eight)]),
                    // 13. Straight with one kicker card
                    ([Clubs(CardRank.Jack), Diamonds(CardRank.Queen), Hearts(CardRank.King), Spades(CardRank.Ace), Diamonds(CardRank.Two)], 
                        [Hearts(CardRank.Ten), Spades(CardRank.Nine)]),
                    // 14. Straight with high card in hand
                    ([Clubs(CardRank.Six), Diamonds(CardRank.Seven), Hearts(CardRank.Eight), Spades(CardRank.Nine)], 
                        [Diamonds(CardRank.Ten), Spades(CardRank.Jack)]),
                    // 15. Straight with scattered cards
                    ([Clubs(CardRank.Five), Diamonds(CardRank.Seven), Hearts(CardRank.Nine), Spades(CardRank.Jack), Diamonds(CardRank.King)], 
                        [Hearts(CardRank.Ten), Spades(CardRank.Eight)]),
                    // 16. Straight with one pair on the flop
                    ([Clubs(CardRank.Four), Diamonds(CardRank.Five), Spades(CardRank.Six)], 
                        [Hearts(CardRank.Seven), Spades(CardRank.Eight)]),
                    // 17. Straight with one card on the turn
                    ([Clubs(CardRank.Three), Diamonds(CardRank.Four), Spades(CardRank.Five), Hearts(CardRank.Six)], 
                        [Diamonds(CardRank.Seven), Spades(CardRank.Eight)]),
                    // 18. Straight with one card on the river
                    ([Hearts(CardRank.Two), Diamonds(CardRank.Three), Clubs(CardRank.Four), Spades(CardRank.Five), Diamonds(CardRank.Six)], 
                        [Hearts(CardRank.Seven), Spades(CardRank.Eight)]),
                    // 19. Straight with all community cards
                    ([Clubs(CardRank.Seven), Diamonds(CardRank.Eight), Hearts(CardRank.Nine), Spades(CardRank.Ten), Diamonds(CardRank.Jack)], 
                        [Hearts(CardRank.Queen), Spades(CardRank.King)]),
                    // 20. Straight with low community cards
                    ([Hearts(CardRank.Two), Diamonds(CardRank.Three), Clubs(CardRank.Four), Spades(CardRank.Five), Diamonds(CardRank.Six)], 
                        [Hearts(CardRank.Seven), Spades(CardRank.Eight)]),
                ]
            },
            {
                HandCardRank.ThreeOfAKind,
                [
                    // highest Three of a kind
                    ([Clubs(CardRank.Ace), Spades(CardRank.Jack), Hearts(CardRank.Queen), Hearts(CardRank.King), Hearts(CardRank.Ace)], 
                        [Hearts(CardRank.Nine), Spades(CardRank.Ace)]),
                    // Lowest Three of a kind
                    ([Clubs(CardRank.Two), Spades(CardRank.Three), Hearts(CardRank.Four), Hearts(CardRank.Five), Hearts(CardRank.Two)], 
                        [Hearts(CardRank.Seven), Spades(CardRank.Two)]),
                    // flop
                    ([Clubs(CardRank.Ace), Spades(CardRank.Jack), Hearts(CardRank.Ace)], 
                        [Hearts(CardRank.Nine), Spades(CardRank.Ace)]),
                    // turn
                    ([Clubs(CardRank.Two), Spades(CardRank.Three), Hearts(CardRank.Four), Hearts(CardRank.Two)], 
                        [Hearts(CardRank.Seven), Spades(CardRank.Two)]),
                        // Existing cases...
                    // Additional cases
                    // 1. Three of a kind with high kickers
                    ([Hearts(CardRank.Ace), Diamonds(CardRank.King), Spades(CardRank.Ace), Clubs(CardRank.Jack), Hearts(CardRank.Nine)], 
                        [Clubs(CardRank.Ace), Diamonds(CardRank.Queen)]),
                    // 2. Three of a kind with low kickers
                    ([Clubs(CardRank.Four), Diamonds(CardRank.Four), Hearts(CardRank.Two), Spades(CardRank.Five), Diamonds(CardRank.Six)], 
                        [Hearts(CardRank.Four), Spades(CardRank.Seven)]),
                    // 3. Three of a kind with one kicker in hand
                    ([Hearts(CardRank.Jack), Diamonds(CardRank.Jack), Spades(CardRank.Three), Clubs(CardRank.Five), Hearts(CardRank.Seven)], 
                        [Clubs(CardRank.Jack), Diamonds(CardRank.Ten)]),
                    // 4. Three of a kind formed on the flop
                    ([Hearts(CardRank.Nine), Diamonds(CardRank.Nine), Spades(CardRank.Nine)], 
                        [Clubs(CardRank.Eight), Diamonds(CardRank.Seven)]),
                    // 5. Three of a kind formed on the turn
                    ([Clubs(CardRank.Six), Diamonds(CardRank.Six), Spades(CardRank.Four), Hearts(CardRank.Six)], 
                        [Diamonds(CardRank.Queen), Spades(CardRank.Jack)]),
                    // 6. Three of a kind with mixed suits
                    ([Clubs(CardRank.Ten), Diamonds(CardRank.Ten), Hearts(CardRank.Ace), Spades(CardRank.Ten), Diamonds(CardRank.Three)], 
                        [Hearts(CardRank.King), Spades(CardRank.Two)]),
                    // 7. Three of a kind with one kicker
                    ([Clubs(CardRank.Eight), Diamonds(CardRank.Eight), Spades(CardRank.Seven), Hearts(CardRank.Eight), Diamonds(CardRank.Five)], 
                        [Hearts(CardRank.Four), Spades(CardRank.Three)]),
                    // 8. Three of a kind with both kickers in hand
                    ([Clubs(CardRank.Five), Diamonds(CardRank.Two), Hearts(CardRank.Three)], 
                        [Spades(CardRank.Five), Diamonds(CardRank.Five)]),
                    // 9. Three of a kind with low cards on the table
                    ([Hearts(CardRank.Three), Diamonds(CardRank.Three), Clubs(CardRank.Two), Spades(CardRank.Three), Diamonds(CardRank.Four)], 
                        [Hearts(CardRank.Ace), Spades(CardRank.King)]),
                    // 10. Three of a kind with a high card in hand
                    ([Clubs(CardRank.Seven), Diamonds(CardRank.Seven), Hearts(CardRank.Six), Spades(CardRank.Seven), Diamonds(CardRank.Nine)], 
                        [Hearts(CardRank.Ace), Spades(CardRank.Jack)]),
                    // 11. Three of a kind formed with pocket cards
                    ([Hearts(CardRank.Four), Diamonds(CardRank.Six), Clubs(CardRank.Eight), Spades(CardRank.Jack), Diamonds(CardRank.Queen)], 
                        [Clubs(CardRank.Four), Spades(CardRank.Four)]),
                    // 12. Three of a kind with scattered kickers
                    ([Clubs(CardRank.Nine), Diamonds(CardRank.Nine), Hearts(CardRank.Ten), Spades(CardRank.Nine), Diamonds(CardRank.Ace)], 
                        [Hearts(CardRank.Queen), Spades(CardRank.King)]),
                    // 13. Three of a kind with one low kicker
                    ([Hearts(CardRank.Two), Diamonds(CardRank.Two), Clubs(CardRank.Five), Spades(CardRank.Two), Diamonds(CardRank.Seven)], 
                        [Hearts(CardRank.Nine), Spades(CardRank.Eight)]),
                    // 14. Three of a kind with one high kicker
                    ([Clubs(CardRank.Jack), Diamonds(CardRank.Jack), Hearts(CardRank.Ace), Spades(CardRank.Jack), Diamonds(CardRank.King)], 
                        [Hearts(CardRank.Queen), Spades(CardRank.Nine)]),
                    // 15. Three of a kind with mixed CardRanks in hand
                    ([Hearts(CardRank.Six), Diamonds(CardRank.Eight), Clubs(CardRank.Ten), Spades(CardRank.Six), Diamonds(CardRank.Queen)], 
                        [Clubs(CardRank.Six), Spades(CardRank.Four)]),
                    // 16. Three of a kind with one pair on the flop
                    ([Clubs(CardRank.Seven), Diamonds(CardRank.Seven), Spades(CardRank.Seven)], 
                        [Hearts(CardRank.Two), Spades(CardRank.Five)]),
                    // 17. Three of a kind with one card on the turn
                    ([Clubs(CardRank.Eight), Diamonds(CardRank.Eight), Spades(CardRank.Two), Hearts(CardRank.Eight)], 
                        [Diamonds(CardRank.Six), Spades(CardRank.Three)]),
                    // 18. Three of a kind with one card on the river
                    ([Hearts(CardRank.Five), Diamonds(CardRank.Five), Clubs(CardRank.Nine), Spades(CardRank.Five), Diamonds(CardRank.Ten)], 
                        [Hearts(CardRank.Jack), Spades(CardRank.Four)]),
                    // 19. Three of a kind with all community cards
                    ([Clubs(CardRank.Three), Diamonds(CardRank.Three), Hearts(CardRank.Three), Spades(CardRank.Two), Diamonds(CardRank.Jack)], 
                        [Hearts(CardRank.King), Spades(CardRank.Queen)]),
                    // 20. Three of a kind with low community cards
                    ([Hearts(CardRank.Two), Diamonds(CardRank.Two), Clubs(CardRank.Two), Spades(CardRank.Four), Diamonds(CardRank.Five)], 
                        [Hearts(CardRank.Seven), Spades(CardRank.Eight)]),
                ]
            },
            {
                HandCardRank.TwoPairs,
                [
                    // highest Two Pairs
                    ([Clubs(CardRank.Ace), Spades(CardRank.Jack), Hearts(CardRank.Queen), Hearts(CardRank.King), Hearts(CardRank.Ace)], 
                        [Hearts(CardRank.Nine), Spades(CardRank.King)]),
                    // Lowest two pairs
                    ([Clubs(CardRank.Two), Spades(CardRank.Three), Hearts(CardRank.Four), Hearts(CardRank.Five), Hearts(CardRank.Three)], 
                        [Hearts(CardRank.Seven), Spades(CardRank.Two)]),
                    // flop
                    ([Clubs(CardRank.Ace), Hearts(CardRank.King), Hearts(CardRank.Ace)], 
                        [Hearts(CardRank.Nine), Spades(CardRank.King)]),
                    // turn
                    ([Clubs(CardRank.Two), Spades(CardRank.Three), Hearts(CardRank.Five), Hearts(CardRank.Three)], 
                        [Hearts(CardRank.Seven), Spades(CardRank.Two)]),
                        // Existing cases...
                    // Additional cases
                    // 1. Two pairs with high kickers
                    ([Clubs(CardRank.Ace), Diamonds(CardRank.Jack), Hearts(CardRank.Queen), Spades(CardRank.Ace), Hearts(CardRank.Jack)], 
                        [Hearts(CardRank.King), Spades(CardRank.Nine)]),
                    // 2. Two pairs with low kickers
                    ([Clubs(CardRank.Two), Diamonds(CardRank.Four), Spades(CardRank.Two), Hearts(CardRank.Four), Hearts(CardRank.Seven)], 
                        [Hearts(CardRank.Three), Spades(CardRank.Six)]),
                    // 3. Two pairs with one pair in hand
                    ([Clubs(CardRank.Ace), Diamonds(CardRank.Jack), Hearts(CardRank.Queen), Spades(CardRank.Three), Hearts(CardRank.Six)], 
                        [Diamonds(CardRank.Ace), Spades(CardRank.Jack)]),
                    // 4. Two pairs with mixed suits
                    ([Clubs(CardRank.Seven), Diamonds(CardRank.Nine), Spades(CardRank.Seven), Hearts(CardRank.Ten), Diamonds(CardRank.Ten)], 
                        [Hearts(CardRank.Four), Spades(CardRank.Two)]),
                    // 5. Two pairs with one high pair and one low pair
                    ([Clubs(CardRank.Ace), Diamonds(CardRank.King), Hearts(CardRank.Three), Spades(CardRank.Ace), Hearts(CardRank.Three)], 
                        [Diamonds(CardRank.Queen), Spades(CardRank.Jack)]),
                    // 6. Two pairs with one pair on the flop
                    ([Clubs(CardRank.Five), Diamonds(CardRank.Eight), Spades(CardRank.Eight)], 
                        [Hearts(CardRank.Five), Spades(CardRank.King)]),
                    // 7. Two pairs with one pair on the turn
                    ([Clubs(CardRank.Nine), Diamonds(CardRank.Six), Spades(CardRank.Six), Hearts(CardRank.Nine)], 
                        [Diamonds(CardRank.Queen), Spades(CardRank.Jack)]),
                    // 8. Two pairs with both pairs in hand
                    ([Clubs(CardRank.Ace), Diamonds(CardRank.Two), Hearts(CardRank.Three)], 
                        [Spades(CardRank.Ace), Diamonds(CardRank.Three)]),
                    // 9. Two pairs with no high card possibility
                    ([Clubs(CardRank.Four), Diamonds(CardRank.Six), Hearts(CardRank.Eight), Spades(CardRank.Four), Diamonds(CardRank.Eight)], 
                        [Hearts(CardRank.Two), Spades(CardRank.Three)]),
                    // 10. Two pairs with one kicker
                    ([Clubs(CardRank.Ten), Diamonds(CardRank.Jack), Spades(CardRank.Queen), Hearts(CardRank.Jack), Diamonds(CardRank.Queen)], 
                        [Hearts(CardRank.Three), Spades(CardRank.Ace)]),
                    // 11. Two pairs with scattered kickers
                    ([Clubs(CardRank.Seven), Diamonds(CardRank.Eight), Hearts(CardRank.Nine), Spades(CardRank.Seven), Diamonds(CardRank.Nine)], 
                        [Hearts(CardRank.Five), Spades(CardRank.Three)]),
                    // 12. Two pairs with one low pair and one mid-range pair
                    ([Clubs(CardRank.Two), Diamonds(CardRank.Six), Hearts(CardRank.Nine), Spades(CardRank.Two), Diamonds(CardRank.Nine)], 
                        [Hearts(CardRank.Ace), Spades(CardRank.King)]),
                    // 13. Two pairs with one pair in the middle of the community cards
                    ([Hearts(CardRank.Ace), Diamonds(CardRank.King), Clubs(CardRank.Queen), Spades(CardRank.Jack), Hearts(CardRank.Queen)], 
                        [Diamonds(CardRank.Jack), Clubs(CardRank.Nine)]),
                    // 14. Two pairs with high card in hand
                    ([Clubs(CardRank.Two), Diamonds(CardRank.Four), Spades(CardRank.Six), Hearts(CardRank.Eight), Diamonds(CardRank.Ten)], 
                        [Hearts(CardRank.Ten), Spades(CardRank.Eight)]),
                    // 15. Two pairs with low card in hand
                    ([Hearts(CardRank.Ace), Diamonds(CardRank.King), Clubs(CardRank.Queen), Spades(CardRank.Jack), Hearts(CardRank.Nine)], 
                        [Diamonds(CardRank.Nine), Clubs(CardRank.Jack)]),
                    // 16. Two pairs with a mixed suit in the community cards
                    ([Hearts(CardRank.Ten), Diamonds(CardRank.Jack), Clubs(CardRank.Queen), Spades(CardRank.King), Hearts(CardRank.Jack)], 
                        [Diamonds(CardRank.Queen), Clubs(CardRank.Ten)]),
                    // 17. Two pairs with different suits in hand and table
                    ([Clubs(CardRank.Ace), Diamonds(CardRank.Jack), Hearts(CardRank.Ten), Spades(CardRank.Ace), Hearts(CardRank.Jack)], 
                        [Diamonds(CardRank.Ten), Clubs(CardRank.King)]),
                    // 18. Two pairs with one kicker in hand
                    ([Clubs(CardRank.Five), Diamonds(CardRank.Seven), Hearts(CardRank.Nine), Spades(CardRank.Five), Diamonds(CardRank.Nine)], 
                        [Hearts(CardRank.King), Spades(CardRank.Seven)]),
                    // 19. Two pairs with one kicker in hand
                    ([Clubs(CardRank.Three), Diamonds(CardRank.Five), Hearts(CardRank.Seven), Spades(CardRank.Three), Diamonds(CardRank.Seven)], 
                        [Hearts(CardRank.Ace), Spades(CardRank.King)]),
                    // 20. Two pairs with both pairs scattered in the community cards
                    ([Hearts(CardRank.Two), Diamonds(CardRank.Four), Clubs(CardRank.Six), Spades(CardRank.Eight), Hearts(CardRank.Ten)], 
                        [Diamonds(CardRank.Six), Clubs(CardRank.Four)]),
                ]
            },
            {
                HandCardRank.OnePair,
                [
                    // highest pair
                    ([Clubs(CardRank.Ace), Spades(CardRank.Jack), Hearts(CardRank.Queen), Hearts(CardRank.King), Hearts(CardRank.Ace)], 
                        [Hearts(CardRank.Nine), Spades(CardRank.Eight)]),
                    // Lowest pair
                    ([Clubs(CardRank.Two), Spades(CardRank.Three), Hearts(CardRank.Four), Hearts(CardRank.Five), Hearts(CardRank.Seven)], 
                        [Hearts(CardRank.Nine), Spades(CardRank.Two)]),
                    // just the pre flop
                    ([], 
                        [Hearts(CardRank.Ace), Spades(CardRank.Ace)]),
                    // flop
                    ([Clubs(CardRank.Ace), Spades(CardRank.Jack), Hearts(CardRank.Ace)], 
                        [Hearts(CardRank.Nine), Spades(CardRank.Eight)]),
                    // turn
                    ([Clubs(CardRank.Two), Spades(CardRank.Three), Hearts(CardRank.Four), Hearts(CardRank.Seven)], 
                        [Hearts(CardRank.Six), Spades(CardRank.Two)]),
                    // 1. Pair with highest possible kicker
                    ([Diamonds(CardRank.Ace), Spades(CardRank.Jack), Hearts(CardRank.Six), Clubs(CardRank.King), Spades(CardRank.Ace)], 
                        [Diamonds(CardRank.Ten), Hearts(CardRank.Nine)]),
                    // 2. Pair with lowest possible kicker
                    ([Clubs(CardRank.Two), Diamonds(CardRank.Four), Spades(CardRank.King), Hearts(CardRank.Seven), Diamonds(CardRank.Two)], 
                        [Clubs(CardRank.Three), Hearts(CardRank.Six)]),
                    // 3. Pair in the middle of the table
                    ([Diamonds(CardRank.Three), Spades(CardRank.Five), Hearts(CardRank.Seven), Clubs(CardRank.Seven), Spades(CardRank.Nine)], 
                        [Diamonds(CardRank.Two), Hearts(CardRank.Four)]),
                    // 4. Pair with all other higher cards
                    ([Clubs(CardRank.Jack), Diamonds(CardRank.Queen), Spades(CardRank.Six), Hearts(CardRank.Ace), Diamonds(CardRank.Jack)], 
                        [Clubs(CardRank.Two), Hearts(CardRank.Three)]),
                    // 5. Pair with one card on the table
                    ([Diamonds(CardRank.Ace), Spades(CardRank.Jack), Hearts(CardRank.Six), Clubs(CardRank.King), Spades(CardRank.Ten)], 
                        [Diamonds(CardRank.Ace), Hearts(CardRank.Nine)]),
                    // 6. Pair with one card in hand
                    ([Diamonds(CardRank.Three), Spades(CardRank.Four), Hearts(CardRank.Five), Clubs(CardRank.Eight), Spades(CardRank.Seven)], 
                        [Spades(CardRank.Three), Hearts(CardRank.Two)]),
                    // 7. Pair with two high kickers
                    ([Diamonds(CardRank.Ace), Spades(CardRank.King), Hearts(CardRank.Queen), Clubs(CardRank.Ten), Spades(CardRank.Ace)], 
                        [Diamonds(CardRank.Nine), Hearts(CardRank.Eight)]),
                    // 8. Pair with two low kickers
                    ([Clubs(CardRank.Two), Diamonds(CardRank.Four), Spades(CardRank.Eight), Hearts(CardRank.Seven), Diamonds(CardRank.Two)], 
                        [Clubs(CardRank.Three), Hearts(CardRank.Six)]),
                    // 9. Pair with scattered kickers
                    ([Clubs(CardRank.Two), Diamonds(CardRank.Six), Spades(CardRank.Ten), Hearts(CardRank.Queen), Diamonds(CardRank.Two)], 
                        [Clubs(CardRank.Ace), Hearts(CardRank.King)]),
                    // 10. Pair with one kicker same as one of the community cards
                    ([Diamonds(CardRank.Ace), Spades(CardRank.Seven), Hearts(CardRank.Queen), Clubs(CardRank.King), Spades(CardRank.Jack)], 
                        [Diamonds(CardRank.Nine), Hearts(CardRank.King)]),
                    // 11. Pair with one high card and one low card in hand
                    ([Diamonds(CardRank.Six), Spades(CardRank.Four), Hearts(CardRank.Five), Clubs(CardRank.Six), Spades(CardRank.Seven)], 
                        [Diamonds(CardRank.Ace), Hearts(CardRank.Two)]),
                    // 12. Pair with one high card on the table
                    ([Hearts(CardRank.Ace), Diamonds(CardRank.Four), Clubs(CardRank.Queen), Spades(CardRank.Jack), Hearts(CardRank.Ten)], 
                        [Diamonds(CardRank.Ten), Clubs(CardRank.Nine)]),
                    // 13. Pair with mixed suits
                    ([Hearts(CardRank.Four), Diamonds(CardRank.Seven), Clubs(CardRank.Nine), Spades(CardRank.Jack), Hearts(CardRank.Nine)], 
                        [Diamonds(CardRank.Three), Clubs(CardRank.Five)]),
                    // 14. Pair with no straight or flush potential
                    ([Hearts(CardRank.Two), Diamonds(CardRank.Six), Clubs(CardRank.Eight), Spades(CardRank.Ten), Hearts(CardRank.Jack)], 
                        [Diamonds(CardRank.Jack), Clubs(CardRank.Four)]),
                    // 15. Pair with one high card and one mid-range card in hand
                    ([Hearts(CardRank.Two), Diamonds(CardRank.Five), Clubs(CardRank.Eight), Spades(CardRank.Ten), Hearts(CardRank.Queen)], 
                        [Diamonds(CardRank.Queen), Clubs(CardRank.Seven)]),
                    // 16. Pair with one card in the middle of the community cards
                    ([Hearts(CardRank.Two), Diamonds(CardRank.Four), Clubs(CardRank.Six), Spades(CardRank.Eight), Hearts(CardRank.Ten)], 
                        [Diamonds(CardRank.Six), Clubs(CardRank.Ace)]),
                    // 17. Pair on the flop
                    ([Hearts(CardRank.Two), Diamonds(CardRank.Four), Clubs(CardRank.Six)], 
                        [Spades(CardRank.Six), Hearts(CardRank.Ace)]),
                    // 18. Pair on the turn
                    ([Hearts(CardRank.Two), Diamonds(CardRank.Four), Clubs(CardRank.Six), Spades(CardRank.Eight)], 
                        [Hearts(CardRank.Six), Diamonds(CardRank.Ace)]),
                    // 19. Pair with high card in hand and low card on the table
                    ([Clubs(CardRank.Two), Diamonds(CardRank.Four), Spades(CardRank.Six), Hearts(CardRank.Eight), Diamonds(CardRank.Ten)], 
                        [Hearts(CardRank.Ten), Spades(CardRank.Ace)]),
                    // 20. Pair with low card in hand and high card on the table
                    ([Hearts(CardRank.Ace), Diamonds(CardRank.King), Clubs(CardRank.Queen), Spades(CardRank.Jack), Hearts(CardRank.Nine)], 
                        [Diamonds(CardRank.Nine), Clubs(CardRank.Two)]),
                ]
            },
            {
                HandCardRank.HighCard,
                [
                    // highest High card
                    ([Clubs(CardRank.Ace), Spades(CardRank.Jack), Hearts(CardRank.Queen), Hearts(CardRank.King), Hearts(CardRank.Seven)], 
                        [Hearts(CardRank.Nine), Spades(CardRank.Eight)]),
                    // Lowest High card
                    ([Clubs(CardRank.Two), Spades(CardRank.Three), Hearts(CardRank.Four), Hearts(CardRank.Five), Hearts(CardRank.Seven)], 
                        [Hearts(CardRank.Nine), Spades(CardRank.Eight)]),
                    // just the pre flop
                    ([], 
                        [Hearts(CardRank.Six), Spades(CardRank.Eight)]),
                    // flop
                    ([Clubs(CardRank.Two), Spades(CardRank.Three), Hearts(CardRank.Four)], 
                        [Hearts(CardRank.Six), Spades(CardRank.Eight)]),
                    // tuen
                    ([Clubs(CardRank.Two), Spades(CardRank.Three), Hearts(CardRank.Four), Hearts(CardRank.Nine)], 
                        [Hearts(CardRank.Six), Spades(CardRank.Eight)]),
                    // 1. High card with a mix of suits
                    ([Hearts(CardRank.Four), Diamonds(CardRank.Seven), Clubs(CardRank.Nine), Spades(CardRank.Jack), Hearts(CardRank.Two)], 
                        [Diamonds(CardRank.Three), Clubs(CardRank.Five)]),
                    // 2. High card with a straight draw
                    ([Hearts(CardRank.Four), Diamonds(CardRank.Five), Clubs(CardRank.Six), Spades(CardRank.Eight), Hearts(CardRank.Nine)], 
                        [Diamonds(CardRank.Ten), Clubs(CardRank.Jack)]),
                    // 3. High card with a flush draw
                    ([Hearts(CardRank.Four), Hearts(CardRank.Seven), Hearts(CardRank.Nine), Hearts(CardRank.Jack), Spades(CardRank.Two)], 
                        [Diamonds(CardRank.Three), Clubs(CardRank.Five)]),
                    // 4. High card 
                    ([Hearts(CardRank.Four), Diamonds(CardRank.King), Clubs(CardRank.Six), Spades(CardRank.Eight), Hearts(CardRank.Nine)], 
                        [Diamonds(CardRank.Ten), Clubs(CardRank.Jack)]),
                    // 5. High card with all low cards
                    ([Hearts(CardRank.Two), Diamonds(CardRank.Three), Clubs(CardRank.Four), Spades(CardRank.Five), Hearts(CardRank.Jack)], 
                        [Diamonds(CardRank.Seven), Clubs(CardRank.Eight)]),
                    // 6. High card with Ace on the table
                    ([Hearts(CardRank.Ace), Diamonds(CardRank.Three), Clubs(CardRank.Four), Spades(CardRank.Five), Hearts(CardRank.Six)], 
                        [Diamonds(CardRank.Nine), Clubs(CardRank.Eight)]),
                    // 7. High card with only one high card on the table
                    ([Hearts(CardRank.Two), Diamonds(CardRank.Three), Clubs(CardRank.Four), Spades(CardRank.Five), Diamonds(CardRank.King)], 
                        [Hearts(CardRank.Seven), Clubs(CardRank.Eight)]),
                    // 8. High card with a scattered range of cards
                    ([Clubs(CardRank.Two), Spades(CardRank.Six), Diamonds(CardRank.Ten), Hearts(CardRank.Queen), Clubs(CardRank.King)], 
                        [Hearts(CardRank.Three), Spades(CardRank.Four)]),
                    // 9. High card with no consecutive cards
                    ([Hearts(CardRank.Two), Diamonds(CardRank.Seven), Clubs(CardRank.Eight), Spades(CardRank.Jack), Hearts(CardRank.Ace)], 
                        [Diamonds(CardRank.Three), Clubs(CardRank.Four)]),
                    // 10. High card with mixed high and low cards
                    ([Hearts(CardRank.Two), Diamonds(CardRank.Seven), Clubs(CardRank.Nine), Spades(CardRank.Queen), Hearts(CardRank.Ace)], 
                        [Diamonds(CardRank.Three), Clubs(CardRank.Four)]),
                    // 11. High card with no pair potential
                    ([Hearts(CardRank.Two), Diamonds(CardRank.Six), Clubs(CardRank.Eight), Spades(CardRank.Ten), Hearts(CardRank.Queen)], 
                        [Diamonds(CardRank.Three), Clubs(CardRank.Four)]),
                    // 12. High card with one high card in player's hand
                    ([Hearts(CardRank.Two), Diamonds(CardRank.Three), Clubs(CardRank.Four), Spades(CardRank.King), Hearts(CardRank.Six)], 
                        [Diamonds(CardRank.Seven), Spades(CardRank.Ace)]),
                    // 13. High card with player's hand lower than table cards
                    ([Hearts(CardRank.Ten), Diamonds(CardRank.Jack), Clubs(CardRank.Six), Spades(CardRank.King), Hearts(CardRank.Ace)], 
                        [Diamonds(CardRank.Two), Clubs(CardRank.Three)]),
                    // 14. High card with one high card in player's hand, rest low
                    ([Hearts(CardRank.Two), Diamonds(CardRank.Three), Clubs(CardRank.Four), Spades(CardRank.Eight), Hearts(CardRank.Six)], 
                        [Diamonds(CardRank.Seven), Spades(CardRank.King)]),
                    // 15. High card with all cards below Ten
                    ([Hearts(CardRank.Two), Diamonds(CardRank.Three), Clubs(CardRank.Four), Spades(CardRank.Eight), Hearts(CardRank.Nine)], 
                        [Diamonds(CardRank.Six), Clubs(CardRank.Seven)]),
                    // 16. High card with a broken straight
                    ([Hearts(CardRank.Two), Diamonds(CardRank.Three), Clubs(CardRank.Four), Spades(CardRank.Six), Hearts(CardRank.Seven)], 
                        [Diamonds(CardRank.Eight), Clubs(CardRank.Nine)]),
                    // 17. High card with a broken flush
                    ([Hearts(CardRank.Two), Hearts(CardRank.Three), Hearts(CardRank.Four), Hearts(CardRank.Six), Spades(CardRank.Seven)], 
                        [Diamonds(CardRank.Eight), Clubs(CardRank.Nine)]),
                    // 18. High card with scattered mid-range cards
                    ([Hearts(CardRank.Five), Diamonds(CardRank.Seven), Clubs(CardRank.Eight), Spades(CardRank.Ten), Hearts(CardRank.Jack)], 
                        [Diamonds(CardRank.Two), Clubs(CardRank.Three)]),
                    // 19. High card with highest card on the table
                    ([Hearts(CardRank.Ace), Diamonds(CardRank.King), Clubs(CardRank.Eight), Spades(CardRank.Jack), Hearts(CardRank.Ten)], 
                        [Diamonds(CardRank.Two), Clubs(CardRank.Three)]),
                ]

            },
        };
    private static Card Hearts(CardRank CardRank) => new Card(CardRank, CardSuit.Hearts);
    private static Card Spades(CardRank CardRank) => new Card(CardRank, CardSuit.Spades);
    private static Card Diamonds(CardRank CardRank) => new Card(CardRank, CardSuit.Diamonds);
    private static Card Clubs(CardRank CardRank) => new Card(CardRank, CardSuit.Clubs);
}
