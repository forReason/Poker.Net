using Poker.Cards;
using Poker.Decks;

namespace Poker.Tests.TestAssets;

using Poker.Cards;
using System.Collections.Generic;

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
    public static Dictionary<HandRank, (Card[] CommunityCards, Card[] PocketCards)[]> Samples =
        new Dictionary<HandRank, (Card[] CommunityCards, Card[] PocketCards)[]>
        {
            {
                HandRank.RoyalFlush,
                [
                    // clean royal flush on table
                    ([Hearts(Rank.Ten), Hearts(Rank.Jack), Hearts(Rank.Queen), Hearts(Rank.King), Hearts(Rank.Ace)], 
                        [Hearts(Rank.Three), Hearts(Rank.Two)]),
                    // 2 cards with different suit
                    ([Hearts(Rank.Ten), Hearts(Rank.Jack), Hearts(Rank.Queen), Hearts(Rank.King), Hearts(Rank.Ace)], 
                        [Hearts(Rank.Ace), Hearts(Rank.King)]),
                    // 2 card with different suit and rank (one on hand
                    ([Hearts(Rank.Ten), Hearts(Rank.Jack), Hearts(Rank.Queen), Hearts(Rank.King), Diamonds(Rank.Nine)], 
                        [Hearts(Rank.Ace), Clubs(Rank.Three)]),
                    // flop with royal flush
                    ([Hearts(Rank.Ten), Hearts(Rank.Jack), Hearts(Rank.Queen)], 
                        [Hearts(Rank.King), Hearts(Rank.Ace)]),
                    // turn with royal flush
                    ([Hearts(Rank.Ten), Hearts(Rank.Jack), Hearts(Rank.Queen),Hearts(Rank.King)], 
                        [Spades(Rank.Nine), Hearts(Rank.Ace)]),
                    // Just Community Cards with royal flush
                    ([Hearts(Rank.Ten), Hearts(Rank.Jack), Hearts(Rank.Queen), Hearts(Rank.King), Hearts(Rank.Ace)], 
                        []),
                    // Existing cases...
                    // Additional cases
                    // 1. Royal flush with one card in hand
                    ([Hearts(Rank.Ten), Hearts(Rank.Jack), Hearts(Rank.Queen), Hearts(Rank.King)],
                        [Hearts(Rank.Ace), Diamonds(Rank.Nine)]),
                    // 2. Royal flush with two cards in hand
                    ([Hearts(Rank.Ten), Hearts(Rank.Jack), Hearts(Rank.Queen)],
                        [Hearts(Rank.King), Hearts(Rank.Ace)]),
                    // 3. Royal flush with mixed suits on the table
                    ([Diamonds(Rank.Ten), Diamonds(Rank.Jack), Diamonds(Rank.Queen), Diamonds(Rank.King), Spades(Rank.Ace)],
                        [Diamonds(Rank.Ace), Clubs(Rank.King)]),
                    // 4. Royal flush with other high cards
                    ([Hearts(Rank.Ten), Hearts(Rank.Jack), Hearts(Rank.Queen), Hearts(Rank.King), Hearts(Rank.Ace)],
                        [Diamonds(Rank.King), Spades(Rank.Queen)]),
                    // 5. Royal flush on turn
                    ([Hearts(Rank.Ten), Hearts(Rank.Jack), Hearts(Rank.Queen), Hearts(Rank.King)],
                        [Hearts(Rank.Ace), Clubs(Rank.Two)]),
                    // 6. Royal flush on flop, with high cards in hand
                    ([Hearts(Rank.Ten), Hearts(Rank.Jack), Hearts(Rank.Queen)],
                        [Hearts(Rank.King), Hearts(Rank.Ace)]),
                    // 7. Royal flush with all community cards
                    ([Hearts(Rank.Ten), Hearts(Rank.Jack), Hearts(Rank.Queen), Hearts(Rank.King), Hearts(Rank.Ace)],
                        [Diamonds(Rank.Two), Clubs(Rank.Three)]),
                    // 8. Royal flush with one pocket card
                    ([Hearts(Rank.Ten), Hearts(Rank.Jack), Hearts(Rank.Queen), Hearts(Rank.King)],
                        [Hearts(Rank.Ace), Spades(Rank.Four)]),
                    // 9. Mixed suits, but royal flush sequence
                    ([Diamonds(Rank.Ten), Diamonds(Rank.Jack), Diamonds(Rank.Queen), Diamonds(Rank.King), Diamonds(Rank.Ace)],
                        [Hearts(Rank.Three), Clubs(Rank.Four)]),
                    // 10. Royal flush, other high cards in hand
                    ([Hearts(Rank.Ten), Hearts(Rank.Jack), Hearts(Rank.Queen), Hearts(Rank.King), Hearts(Rank.Ace)],
                        [Diamonds(Rank.King), Spades(Rank.Queen)]),
                    // 11. Royal flush on turn, mixed suits
                    ([Diamonds(Rank.Ten), Diamonds(Rank.Jack), Diamonds(Rank.Queen), Diamonds(Rank.King)],
                        [Diamonds(Rank.Ace), Clubs(Rank.Two)]),
                    // 12. Royal flush with other suited cards
                    ([Hearts(Rank.Ten), Hearts(Rank.Jack), Hearts(Rank.Queen), Hearts(Rank.King), Hearts(Rank.Ace)],
                        [Hearts(Rank.Nine), Hearts(Rank.Eight)]),
                    // 13. Royal flush, one card on hand, other suits
                    ([Clubs(Rank.Ten), Clubs(Rank.Jack), Clubs(Rank.Queen), Clubs(Rank.King)],
                        [Clubs(Rank.Ace), Diamonds(Rank.Seven)]),
                    // 14. Royal flush with high cards outside the sequence
                    ([Hearts(Rank.Ten), Hearts(Rank.Jack), Hearts(Rank.Queen), Hearts(Rank.King), Hearts(Rank.Ace)],
                        [Diamonds(Rank.Queen), Spades(Rank.King)]),
                    // 15. Royal flush on flop, turn, and river
                    ([Clubs(Rank.Ten), Clubs(Rank.Jack), Clubs(Rank.Queen)],
                        [Clubs(Rank.King), Clubs(Rank.Ace)]),
                    // 16. Mixed ranks with a royal flush sequence
                    ([Hearts(Rank.Ten), Hearts(Rank.Jack), Hearts(Rank.Queen), Hearts(Rank.King), Hearts(Rank.Ace)],
                        [Clubs(Rank.Nine), Diamonds(Rank.Eight)]),
                    // 17. Royal flush, high cards in hand
                    ([Hearts(Rank.Ten), Hearts(Rank.Jack), Hearts(Rank.Queen), Hearts(Rank.King)],
                        [Hearts(Rank.Ace), Diamonds(Rank.King)]),
                    // 18. Royal flush with mixed suits, other high cards
                    ([Diamonds(Rank.Ten), Diamonds(Rank.Jack), Diamonds(Rank.Queen), Diamonds(Rank.King), Diamonds(Rank.Ace)],
                        [Hearts(Rank.Nine), Clubs(Rank.Eight)]),
                    // 19. Royal flush with one card in hand, mixed suits
                    ([Hearts(Rank.Ten), Hearts(Rank.Jack), Hearts(Rank.Queen), Hearts(Rank.King)],
                        [Hearts(Rank.Ace), Spades(Rank.Four)]),
                    // 20. Royal flush, lowest ranks outside sequence
                    ([Hearts(Rank.Ten), Hearts(Rank.Jack), Hearts(Rank.Queen), Hearts(Rank.King), Hearts(Rank.Ace)],
                        [Diamonds(Rank.Two), Clubs(Rank.Three)]),
                ]
            },
            {
                HandRank.StraightFlush,
                [
                    // highest straight flush on table
                    ([Hearts(Rank.Ten), Hearts(Rank.Jack), Hearts(Rank.Queen), Hearts(Rank.King), Hearts(Rank.Nine)], 
                    [Hearts(Rank.Three), Hearts(Rank.Two)]),
                    // lowest straight flush without wheel
                    ([Hearts(Rank.Ten), Hearts(Rank.Four), Hearts(Rank.Six), Hearts(Rank.Five), Hearts(Rank.Nine)], 
                        [Hearts(Rank.Three), Hearts(Rank.Two)]),
                    // lowest straight flush with wheel
                    ([Hearts(Rank.Ten), Hearts(Rank.Four), Hearts(Rank.Ace), Hearts(Rank.Five), Hearts(Rank.Six)], 
                        [Hearts(Rank.Three), Hearts(Rank.Two)]),
                    // straight flush with other cards mixed in
                    ([Clubs(Rank.Ten), Hearts(Rank.Four), Hearts(Rank.Ace), Hearts(Rank.Five), Spades(Rank.Six)], 
                        [Hearts(Rank.Three), Hearts(Rank.Two)]),
                    // flop
                    ([Hearts(Rank.Ten), Hearts(Rank.Jack), Hearts(Rank.Queen)], 
                        [Hearts(Rank.King), Hearts(Rank.Nine)]),
                    // turn
                    ([Hearts(Rank.Ten), Hearts(Rank.Four), Hearts(Rank.Five), Hearts(Rank.Six)], 
                        [Hearts(Rank.Three), Hearts(Rank.Two)]),
                    // Existing cases...
                    // Additional cases
                    // 1. Straight flush ending at King
                    ([Diamonds(Rank.Nine), Diamonds(Rank.Ten), Diamonds(Rank.Jack), Diamonds(Rank.Queen), Diamonds(Rank.King)],
                        [Diamonds(Rank.Four), Diamonds(Rank.Two)]),
                    // 2. Straight flush starting from Ace (wheel)
                    ([Clubs(Rank.Ace), Clubs(Rank.Two), Clubs(Rank.Three), Clubs(Rank.Four), Clubs(Rank.Five)],
                        [Clubs(Rank.Six), Clubs(Rank.Seven)]),
                    // 3. Straight flush in the middle of the deck
                    ([Hearts(Rank.Six), Hearts(Rank.Seven), Hearts(Rank.Eight), Hearts(Rank.Nine), Hearts(Rank.Ten)],
                        [Hearts(Rank.Jack), Hearts(Rank.Queen)]),
                    // 4. Straight flush with mixed suits but same rank
                    ([Spades(Rank.Eight), Spades(Rank.Nine), Spades(Rank.Ten), Spades(Rank.Jack), Spades(Rank.Queen)],
                        [Diamonds(Rank.King), Hearts(Rank.Seven)]),
                    // 5. Straight flush on flop, turn, and river
                    ([Diamonds(Rank.Seven), Diamonds(Rank.Eight), Diamonds(Rank.Nine)],
                        [Diamonds(Rank.Ten), Diamonds(Rank.Jack)]),
                    // 6. Lowest straight flush with wheel, other suits
                    ([Clubs(Rank.Ace), Clubs(Rank.Two), Clubs(Rank.Three), Clubs(Rank.Four), Clubs(Rank.Five)],
                        [Hearts(Rank.Six), Spades(Rank.Seven)]),
                    // 7. Straight flush with other high cards
                    ([Spades(Rank.Jack), Spades(Rank.Queen), Spades(Rank.King), Spades(Rank.Three), Spades(Rank.Ten)],
                        [Spades(Rank.Nine), Spades(Rank.Ten)]),
                    // 8. Straight flush on turn, other ranks
                    ([Hearts(Rank.Four), Hearts(Rank.Five), Hearts(Rank.Six), Hearts(Rank.Seven)],
                        [Hearts(Rank.Three), Hearts(Rank.Eight)]),
                    // 9. Mixed suits with a straight flush sequence
                    ([Diamonds(Rank.Seven), Diamonds(Rank.Eight), Diamonds(Rank.Nine), Diamonds(Rank.Ten), Diamonds(Rank.Jack)],
                        [Clubs(Rank.Queen), Spades(Rank.King)]),
                    // 10. Straight flush, Ace high
                    ([Spades(Rank.Four), Hearts(Rank.King), Spades(Rank.Queen), Spades(Rank.Jack), Spades(Rank.Ten)],
                        [Spades(Rank.Nine), Spades(Rank.Eight)]),
                    // 11. Straight flush, other suits
                    ([Clubs(Rank.Nine), Clubs(Rank.Eight), Clubs(Rank.Seven), Clubs(Rank.Six), Clubs(Rank.Five)],
                        [Hearts(Rank.Four), Diamonds(Rank.Three)]),
                    // 12. Mixed ranks with a straight flush
                    ([Hearts(Rank.Three), Hearts(Rank.Four), Hearts(Rank.Five), Hearts(Rank.Six), Hearts(Rank.Seven)],
                        [Spades(Rank.Eight), Diamonds(Rank.Nine)]),
                    // 13. Straight flush, low to high
                    ([Diamonds(Rank.Two), Diamonds(Rank.Three), Diamonds(Rank.Four), Diamonds(Rank.Five), Diamonds(Rank.Six)],
                        [Clubs(Rank.Seven), Spades(Rank.Eight)]),
                    // 14. Straight flush with high card outside the sequence
                    ([Clubs(Rank.Ten), Clubs(Rank.Jack), Clubs(Rank.Queen), Clubs(Rank.King), Clubs(Rank.Nine)],
                        [Diamonds(Rank.Nine), Hearts(Rank.Eight)]),
                    // 15. Straight flush, middle ranks
                    ([Spades(Rank.Six), Spades(Rank.Seven), Spades(Rank.Eight), Spades(Rank.Nine), Spades(Rank.Ten)],
                        [Hearts(Rank.Jack), Diamonds(Rank.Queen)]),
                    // 16. Straight flush with mixed high cards
                    ([Hearts(Rank.King), Hearts(Rank.Queen), Hearts(Rank.Jack), Hearts(Rank.Ten), Hearts(Rank.Nine)],
                        [Clubs(Rank.Eight), Diamonds(Rank.Seven)]),
                    // 17. Straight flush, low ranks
                    ([Diamonds(Rank.Three), Diamonds(Rank.Four), Diamonds(Rank.Five), Diamonds(Rank.Six), Diamonds(Rank.Seven)],
                        [Hearts(Rank.Two), Spades(Rank.Ace)]),
                    // 18. Straight flush on flop, turn, river with mixed suits
                    ([Clubs(Rank.Jack), Clubs(Rank.Queen), Clubs(Rank.King), Spades(Rank.Ace), Clubs(Rank.Ten)],
                        [Clubs(Rank.Nine), Spades(Rank.Eight)]),
                    // 19. Straight flush, other high cards
                    ([Spades(Rank.Five), Spades(Rank.Six), Spades(Rank.Seven), Spades(Rank.Eight), Spades(Rank.Nine)],
                        [Diamonds(Rank.Ten), Clubs(Rank.Jack)]),
                    // 20. Mixed suits, straight flush sequence
                    ([Hearts(Rank.Two), Hearts(Rank.Three), Hearts(Rank.Four), Hearts(Rank.Five), Hearts(Rank.Six)],
                        [Diamonds(Rank.Seven), Clubs(Rank.Eight)]),
                ]
            },
            {
                HandRank.FourOfAKind,
                [
                    // highest four of a kind
                    ([Hearts(Rank.Ten), Hearts(Rank.Ace), Hearts(Rank.Queen), Spades(Rank.Ace), Diamonds(Rank.Ace)], 
                        [Hearts(Rank.Three), Clubs(Rank.Ace)]),
                    // lowest four of a kind
                    ([Clubs(Rank.Two), Hearts(Rank.Four), Spades(Rank.Two), Hearts(Rank.Five), Diamonds(Rank.Two)], 
                        [Hearts(Rank.Three), Hearts(Rank.Two)]),
                    // flop
                    ([Hearts(Rank.Ace), Spades(Rank.Ace), Diamonds(Rank.Ace)], 
                        [Hearts(Rank.Three), Clubs(Rank.Ace)]),
                    // turn
                    ([Clubs(Rank.Two), Hearts(Rank.Four), Spades(Rank.Two), Diamonds(Rank.Two)], 
                        [Hearts(Rank.Three), Hearts(Rank.Two)]),
                        // Existing cases...
                    // Additional cases
                    // 1. Four of a kind with highest kicker
                    ([Hearts(Rank.King), Diamonds(Rank.King), Clubs(Rank.King), Spades(Rank.King), Hearts(Rank.Ace)], 
                        [Diamonds(Rank.Queen), Spades(Rank.Jack)]),
                    // 2. Four of a kind with lowest kicker
                    ([Clubs(Rank.Three), Spades(Rank.Three), Diamonds(Rank.Three), Hearts(Rank.Three), Spades(Rank.Two)], 
                        [Hearts(Rank.Four), Clubs(Rank.Five)]),
                    // 3. Four of a kind on the flop
                    ([Hearts(Rank.Jack), Diamonds(Rank.Jack), Clubs(Rank.Jack)], 
                        [Spades(Rank.Jack), Hearts(Rank.King)]),
                    // 4. Four of a kind on the turn
                    ([Clubs(Rank.Six), Spades(Rank.Six), Diamonds(Rank.Six), Hearts(Rank.Six)], 
                        [Hearts(Rank.Seven), Spades(Rank.Eight)]),
                    // 5. Four of a kind with one card in hand
                    ([Clubs(Rank.Nine), Spades(Rank.Nine), Diamonds(Rank.Nine), Hearts(Rank.Ten), Spades(Rank.Queen)], 
                        [Hearts(Rank.Nine), Diamonds(Rank.King)]),
                    // 6. Four of a kind with both cards in hand
                    ([Hearts(Rank.Seven), Diamonds(Rank.Five), Clubs(Rank.Ten), Spades(Rank.Five)], 
                        [Hearts(Rank.Five), Diamonds(Rank.Five)]),
                    // 7. Four of a kind with mixed suits
                    ([Clubs(Rank.Four), Spades(Rank.Four), Diamonds(Rank.Four), Hearts(Rank.Six), Spades(Rank.Seven)], 
                        [Hearts(Rank.Four), Diamonds(Rank.Eight)]),
                    // 8. Four of a kind with one card on the river
                    ([Hearts(Rank.Ten), Diamonds(Rank.Ace), Clubs(Rank.Queen), Spades(Rank.Ace)], 
                        [Hearts(Rank.Ace), Diamonds(Rank.Ace)]),
                    // 9. Four of a kind with scattered ranks
                    ([Clubs(Rank.Eight), Spades(Rank.Nine), Diamonds(Rank.Ten), Hearts(Rank.Jack), Spades(Rank.Ten)], 
                        [Hearts(Rank.Ten), Diamonds(Rank.Ten)]),
                    // 10. Four of a kind with high and low cards
                    ([Hearts(Rank.Two), Diamonds(Rank.Ten), Clubs(Rank.Six), Spades(Rank.Eight), Hearts(Rank.Ten)], 
                        [Diamonds(Rank.Ten), Spades(Rank.Ten)]),
                    // 11. Four of a kind with different ranks on the table
                    ([Clubs(Rank.Queen), Spades(Rank.King), Diamonds(Rank.Ace), Hearts(Rank.Ace), Spades(Rank.Ace)], 
                        [Hearts(Rank.Ace), Diamonds(Rank.Jack)]),
                    // 12. Four of a kind with one pair on the flop
                    ([Hearts(Rank.Seven), Diamonds(Rank.Seven), Clubs(Rank.Seven)], 
                        [Spades(Rank.Seven), Clubs(Rank.Eight)]),
                    // 13. Four of a kind with one card in hand and one on the river
                    ([Clubs(Rank.Two), Spades(Rank.Two), Diamonds(Rank.Five), Hearts(Rank.Six)], 
                        [Hearts(Rank.Two), Diamonds(Rank.Two)]),
                    // 14. Four of a kind with mixed high and low cards
                    ([Hearts(Rank.Nine), Diamonds(Rank.Jack), Clubs(Rank.Queen), Spades(Rank.King), Hearts(Rank.Queen)], 
                        [Diamonds(Rank.Queen), Spades(Rank.Queen)]),
                    // 15. Four of a kind with scattered suits
                    ([Clubs(Rank.Five), Spades(Rank.Six), Diamonds(Rank.Seven), Hearts(Rank.Eight), Spades(Rank.Five)], 
                        [Hearts(Rank.Five), Diamonds(Rank.Five)]),
                    // 16. Four of a kind with one pair in hand and one on the table
                    ([Hearts(Rank.Two), Diamonds(Rank.Four), Clubs(Rank.Two)], 
                        [Spades(Rank.Two), Hearts(Rank.Two)]),
                    // 17. Four of a kind with low and high kickers
                    ([Clubs(Rank.Jack), Spades(Rank.Jack), Diamonds(Rank.Jack), Hearts(Rank.Two), Spades(Rank.Three)], 
                        [Hearts(Rank.Jack), Diamonds(Rank.Four)]),
                    // 18. Four of a kind with one card on the flop and one on the turn
                    ([Hearts(Rank.Seven), Diamonds(Rank.Eight), Clubs(Rank.Seven)], 
                        [Spades(Rank.Seven), Hearts(Rank.Seven)]),
                    // 19. Four of a kind with scattered high cards
                    ([Clubs(Rank.Ten), Spades(Rank.Jack), Diamonds(Rank.Queen), Hearts(Rank.King), Spades(Rank.Ten)], 
                        [Hearts(Rank.Ten), Diamonds(Rank.Ten)]),
                    // 20. Four of a kind with various suits and ranks
                    ([Hearts(Rank.Three), Diamonds(Rank.Five), Clubs(Rank.Seven), Spades(Rank.Nine), Hearts(Rank.Five)], 
                        [Diamonds(Rank.Five), Spades(Rank.Five)]),
                ]
            },
            {
                HandRank.FullHouse,
                [
                    // highest FullHouse
                    ([Hearts(Rank.King), Hearts(Rank.Ace), Hearts(Rank.Queen), Spades(Rank.Ace), Spades(Rank.King)], 
                        [Hearts(Rank.Three), Clubs(Rank.Ace)]),
                    // lowest FullHouse
                    ([Diamonds(Rank.Three), Hearts(Rank.Two), Hearts(Rank.Queen), Spades(Rank.Two), Spades(Rank.Three)], 
                        [Hearts(Rank.Three), Clubs(Rank.Two)]),
                    // Existing cases...
                    // Additional cases
                    // 1. Full House with highest three-of-a-kind and highest pair
                    ([Hearts(Rank.Ace), Diamonds(Rank.Ace), Clubs(Rank.Ace), Spades(Rank.King), Hearts(Rank.King)], 
                        [Diamonds(Rank.Queen), Spades(Rank.Jack)]),
                    // 2. Full House with lowest three-of-a-kind and lowest pair
                    ([Clubs(Rank.Two), Spades(Rank.Two), Diamonds(Rank.Two), Hearts(Rank.Three), Spades(Rank.Three)], 
                        [Hearts(Rank.Four), Clubs(Rank.Five)]),
                    // 3. Full House formed on the flop
                    ([Hearts(Rank.Ace), Diamonds(Rank.Ace), Clubs(Rank.Ace)], 
                        [Spades(Rank.King), Hearts(Rank.King)]),
                    // 4. Full House formed on the turn
                    ([Clubs(Rank.Two), Spades(Rank.Two), Diamonds(Rank.Two), Hearts(Rank.Three)], 
                        [Spades(Rank.Three), Clubs(Rank.Four)]),
                    // 5. Full House with mixed ranks
                    ([Hearts(Rank.Six), Diamonds(Rank.Six), Clubs(Rank.Six), Spades(Rank.Seven), Hearts(Rank.Seven)], 
                        [Diamonds(Rank.Eight), Spades(Rank.Nine)]),
                    // 6. Full House with three-of-a-kind on table
                    ([Hearts(Rank.Jack), Diamonds(Rank.Jack), Clubs(Rank.Jack), Spades(Rank.Ten), Hearts(Rank.Nine)], 
                        [Diamonds(Rank.Ten), Spades(Rank.Ten)]),
                    // 7. Full House with pair in hand
                    ([Clubs(Rank.Four), Spades(Rank.Five), Diamonds(Rank.Five), Hearts(Rank.Six), Spades(Rank.Seven)], 
                        [Hearts(Rank.Four), Diamonds(Rank.Four)]),
                    // 8. Full House with different suits
                    ([Hearts(Rank.Eight), Diamonds(Rank.Eight), Clubs(Rank.Eight), Spades(Rank.Nine), Hearts(Rank.Ten)], 
                        [Diamonds(Rank.Nine), Spades(Rank.Nine)]),
                    // 9. Full House with one pair on the river
                    ([Clubs(Rank.Queen), Spades(Rank.Queen), Diamonds(Rank.King)], 
                        [Hearts(Rank.Queen), Spades(Rank.King)]),
                    // 10. Full House with Ace pocket
                    ([Hearts(Rank.Two), Diamonds(Rank.Two), Clubs(Rank.Four), Spades(Rank.Five), Clubs(Rank.Ace)], 
                        [Hearts(Rank.Ace), Diamonds(Rank.Ace)]),
                    // 11. Full House with high pair on table
                    ([Clubs(Rank.Jack), Spades(Rank.Jack), Diamonds(Rank.Ten), Hearts(Rank.Ten), Spades(Rank.Nine)], 
                        [Hearts(Rank.Jack), Diamonds(Rank.Seven)]),
                    // 12. Full House with low pair in hand
                    ([Hearts(Rank.Five), Diamonds(Rank.Six), Clubs(Rank.Seven), Spades(Rank.Six)], 
                        [Hearts(Rank.Five), Diamonds(Rank.Five)]),
                    // 13. Full House with scattered ranks
                    ([Clubs(Rank.Nine), Spades(Rank.Eight), Diamonds(Rank.Seven), Hearts(Rank.Nine), Spades(Rank.Five)], 
                        [Hearts(Rank.Seven), Diamonds(Rank.Seven)]),
                    // 14. Full House with three-of-a-kind on the flop
                    ([Hearts(Rank.King), Diamonds(Rank.King), Clubs(Rank.King)], 
                        [Spades(Rank.Queen), Hearts(Rank.Queen)]),
                    // 15. Full House with pair on the turn
                    ([Clubs(Rank.Three), Spades(Rank.Three), Diamonds(Rank.Three), Hearts(Rank.Four)], 
                        [Spades(Rank.Two), Clubs(Rank.Two)]),
                    // 16. Full House with highest three-of-a-kind and lowest pair
                    ([Hearts(Rank.Ace), Diamonds(Rank.Ace), Clubs(Rank.Ace), Spades(Rank.Two), Hearts(Rank.Two)], 
                        [Diamonds(Rank.Three), Spades(Rank.Four)]),
                    // 17. Full House with lowest three-of-a-kind and highest pair
                    ([Clubs(Rank.Two), Spades(Rank.Two), Diamonds(Rank.Two), Hearts(Rank.King), Spades(Rank.King)], 
                        [Hearts(Rank.Queen), Clubs(Rank.Jack)]),
                    // 18. Full House with three-of-a-kind and pair of different ranks
                    ([Hearts(Rank.Seven), Diamonds(Rank.Seven), Clubs(Rank.Seven), Spades(Rank.Eight), Hearts(Rank.Eight)], 
                        [Diamonds(Rank.Nine), Spades(Rank.Ten)]),
                    // 19. Full House with mixed high and low cards
                    ([Hearts(Rank.Two), Diamonds(Rank.Four), Clubs(Rank.Six), Spades(Rank.Six), Hearts(Rank.Six)], 
                        [Diamonds(Rank.Two), Spades(Rank.Two)]),
                    // 20. Full House with one pair in hand and one on table
                    ([Hearts(Rank.Three), Diamonds(Rank.Seven), Clubs(Rank.Four), Spades(Rank.Four)], 
                        [Hearts(Rank.Three), Diamonds(Rank.Three)]),
                ]
            },
            {
                HandRank.Flush,
                [
                    // highest Flush
                    ([Hearts(Rank.King), Hearts(Rank.Ace), Hearts(Rank.Queen), Spades(Rank.Ace), Spades(Rank.King)], 
                        [Hearts(Rank.Jack), Hearts(Rank.Nine)]),
                    // lowest Flush
                    ([Hearts(Rank.Three), Hearts(Rank.Two), Hearts(Rank.Seven), Hearts(Rank.Five), Spades(Rank.Three)], 
                        [Hearts(Rank.Four), Clubs(Rank.Two)]),
                    // flop
                    ([Hearts(Rank.King), Hearts(Rank.Ace), Hearts(Rank.Queen)], 
                    [Hearts(Rank.Jack), Hearts(Rank.Nine)]),
                    // turn
                    ([Hearts(Rank.Three), Hearts(Rank.Two), Hearts(Rank.Seven), Hearts(Rank.Five)], 
                        [Hearts(Rank.Four), Clubs(Rank.Two)]),
                        // Existing cases...
                    // Additional cases
                    // 1. Flush with mixed high and low cards
                    ([Hearts(Rank.King), Hearts(Rank.Nine), Hearts(Rank.Three), Hearts(Rank.Five), Hearts(Rank.Two)], 
                        [Hearts(Rank.Six), Clubs(Rank.Seven)]),
                    // 2. Flush with highest card in hand
                    ([Hearts(Rank.Queen), Hearts(Rank.Four), Hearts(Rank.Ten), Hearts(Rank.Nine), Spades(Rank.King)], 
                        [Hearts(Rank.Ace), Diamonds(Rank.Two)]),
                    // 3. Flush formed on the flop
                    ([Hearts(Rank.Ace), Hearts(Rank.King), Hearts(Rank.Queen)], 
                        [Hearts(Rank.Jack), Hearts(Rank.Nine)]),
                    // 4. Flush formed on the turn
                    ([Hearts(Rank.Four), Hearts(Rank.Seven), Hearts(Rank.Eight), Hearts(Rank.Nine)], 
                        [Hearts(Rank.Ten), Clubs(Rank.Two)]),
                    // 5. Flush with one card on the river
                    ([Hearts(Rank.Five), Hearts(Rank.Six), Hearts(Rank.Seven), Hearts(Rank.Seven)], 
                        [Hearts(Rank.Nine), Spades(Rank.King)]),
                    // 6. Flush with scattered ranks
                    ([Hearts(Rank.Three), Hearts(Rank.Six), Hearts(Rank.Ten), Hearts(Rank.Queen), Spades(Rank.Ace)], 
                        [Hearts(Rank.King), Diamonds(Rank.Two)]),
                    // 7. Flush with one pair in hand
                    ([Hearts(Rank.Four), Hearts(Rank.Eight), Hearts(Rank.Nine), Hearts(Rank.Ten)], 
                        [Hearts(Rank.Ten), Hearts(Rank.Queen)]),
                    // 8. Flush with high cards on table
                    ([Hearts(Rank.Ace), Hearts(Rank.King), Hearts(Rank.Queen), Hearts(Rank.Nine), Diamonds(Rank.Ten)], 
                        [Hearts(Rank.Nine), Clubs(Rank.Eight)]),
                    // 9. Flush with mixed suits on table
                    ([Hearts(Rank.Seven), Hearts(Rank.Eight), Hearts(Rank.Eight), Spades(Rank.Jack), Hearts(Rank.Queen)], 
                        [Hearts(Rank.Ten), Clubs(Rank.Ace)]),
                    // 10. Flush with lowest card in hand
                    ([Hearts(Rank.Two), Hearts(Rank.Three), Hearts(Rank.Four), Hearts(Rank.Five)], 
                        [Hearts(Rank.Five), Diamonds(Rank.Seven)]),
                    // 11. Flush with high community cards
                    ([Hearts(Rank.Jack), Hearts(Rank.Queen), Hearts(Rank.Seven), Hearts(Rank.Ace), Spades(Rank.Ten)], 
                        [Hearts(Rank.Nine), Clubs(Rank.Eight)]),
                    // 12. Flush with low community cards
                    ([Hearts(Rank.Two), Hearts(Rank.Three), Hearts(Rank.Four), Hearts(Rank.Nine), Spades(Rank.Six)], 
                        [Hearts(Rank.Seven), Diamonds(Rank.Eight)]),
                    // 13. Flush with one kicker card
                    ([Hearts(Rank.Nine), Hearts(Rank.Ten), Hearts(Rank.Four), Hearts(Rank.Queen), Diamonds(Rank.King)], 
                        [Hearts(Rank.Ace), Clubs(Rank.Two)]),
                    // 14. Flush with all community cards
                    ([Hearts(Rank.Ace), Hearts(Rank.King), Hearts(Rank.Seven), Hearts(Rank.Jack), Hearts(Rank.Ten)], 
                        [Diamonds(Rank.Three), Clubs(Rank.Four)]),
                    // 15. Flush with mixed ranks in hand
                    ([Hearts(Rank.Five), Hearts(Rank.Six), Hearts(Rank.Two), Hearts(Rank.Eight)], 
                        [Hearts(Rank.Nine), Hearts(Rank.Ten)]),
                    // 16. Flush with one card on the flop
                    ([Hearts(Rank.Ace), Hearts(Rank.King), Hearts(Rank.Queen)], 
                        [Hearts(Rank.Jack), Hearts(Rank.Nine)]),
                    // 17. Flush with one card on the turn
                    ([Hearts(Rank.Two), Hearts(Rank.Three), Hearts(Rank.Eight), Hearts(Rank.Five)], 
                        [Hearts(Rank.Six), Clubs(Rank.Seven)]),
                    // 18. Flush with one card on the river
                    ([Hearts(Rank.Five), Hearts(Rank.Six), Hearts(Rank.Seven), Hearts(Rank.Eight)], 
                        [Hearts(Rank.Jack), Spades(Rank.Ten)]),
                    // 19. Flush with two cards in hand
                    ([Hearts(Rank.Seven), Hearts(Rank.Eight), Hearts(Rank.Nine), Spades(Rank.Ten)], 
                        [Hearts(Rank.Jack), Hearts(Rank.Queen)]),
                    // 20. Flush with mixed high and low cards on table
                    ([Hearts(Rank.Two), Hearts(Rank.Four), Hearts(Rank.Six), Hearts(Rank.Eight), Hearts(Rank.Ten)], 
                        [Hearts(Rank.Ace), Clubs(Rank.Three)]),
                ]
            },
            {
                HandRank.Straight,
                [
                    // highest Straight
                    ([Hearts(Rank.Ten), Spades(Rank.Jack), Hearts(Rank.Queen), Clubs(Rank.King), Hearts(Rank.Ace)], 
                        [Diamonds(Rank.Three), Hearts(Rank.Two)]),
                    // lowest Straight without wheel
                    ([Hearts(Rank.Ten), Diamonds(Rank.Four), Hearts(Rank.Six), Clubs(Rank.Five), Diamonds(Rank.Nine)], 
                        [Hearts(Rank.Three), Spades(Rank.Two)]),
                    // lowest Straight with wheel
                    ([Hearts(Rank.Ten), Diamonds(Rank.Jack), Hearts(Rank.Queen), Clubs(Rank.King), Diamonds(Rank.Ace)], 
                        [Hearts(Rank.Three), Hearts(Rank.Two)]),
                    // straight with other cards mixed in
                    ([Clubs(Rank.Ten), Diamonds(Rank.Four), Hearts(Rank.Ace), Hearts(Rank.Five), Spades(Rank.Ten)], 
                        [Hearts(Rank.Three), Hearts(Rank.Two)]),
                    // flop
                    ([Hearts(Rank.Ten), Spades(Rank.Jack), Hearts(Rank.Queen)], 
                        [Hearts(Rank.King), Hearts(Rank.Ace)]),
                    // turn
                    ([Clubs(Rank.Ten), Diamonds(Rank.Four),  Hearts(Rank.Five), Spades(Rank.Six)], 
                        [Hearts(Rank.Three), Hearts(Rank.Two)]),
                    // Existing cases...
                    // Additional cases
                    // 1. Straight in sequential order
                    ([Hearts(Rank.Seven), Diamonds(Rank.Eight), Clubs(Rank.Nine), Spades(Rank.Ten), Hearts(Rank.Jack)], 
                        [Diamonds(Rank.Queen), Spades(Rank.King)]),
                    // 2. Straight with mixed suits
                    ([Hearts(Rank.Five), Diamonds(Rank.Six), Clubs(Rank.Seven), Spades(Rank.Eight), Hearts(Rank.Nine)], 
                        [Clubs(Rank.Ten), Diamonds(Rank.Jack)]),
                    // 3. Straight with low-high order
                    ([Hearts(Rank.Two), Diamonds(Rank.Three), Clubs(Rank.Four), Spades(Rank.Five), Hearts(Rank.Six)], 
                        [Diamonds(Rank.Seven), Spades(Rank.Eight)]),
                    // 4. Straight with gaps in the middle
                    ([Hearts(Rank.Ten), Diamonds(Rank.Jack), Clubs(Rank.Queen), Spades(Rank.King), Diamonds(Rank.Ace)], 
                        [Hearts(Rank.Nine), Clubs(Rank.Eight)]),
                    // 5. Straight formed on the flop
                    ([Hearts(Rank.Nine), Diamonds(Rank.Ten), Clubs(Rank.Jack)], 
                        [Spades(Rank.Queen), Hearts(Rank.King)]),
                    // 6. Straight formed on the turn
                    ([Hearts(Rank.Eight), Diamonds(Rank.Nine), Clubs(Rank.Ten), Spades(Rank.Jack)], 
                        [Diamonds(Rank.Queen), Hearts(Rank.King)]),
                    // 7. Straight with highest card in hand
                    ([Clubs(Rank.Nine), Diamonds(Rank.Ten), Hearts(Rank.Jack), Spades(Rank.Queen)], 
                        [Hearts(Rank.King), Diamonds(Rank.Ace)]),
                    // 8. Straight with one card on the river
                    ([Hearts(Rank.Seven), Diamonds(Rank.Eight), Clubs(Rank.Nine), Spades(Rank.Ten)], 
                        [Diamonds(Rank.Jack), Hearts(Rank.Queen)]),
                    // 9. Straight with mixed ranks on table
                    ([Clubs(Rank.Four), Diamonds(Rank.Five), Hearts(Rank.Six), Spades(Rank.Seven), Diamonds(Rank.Eight)], 
                        [Hearts(Rank.Ten), Spades(Rank.Jack)]),
                    // 10. Straight with both hole cards used
                    ([Clubs(Rank.Three), Diamonds(Rank.Four), Hearts(Rank.Six)], 
                        [Spades(Rank.Five), Diamonds(Rank.Seven)]),
                    // 11. Straight with all community cards
                    ([Hearts(Rank.Two), Diamonds(Rank.Three), Clubs(Rank.Four), Spades(Rank.Five), Diamonds(Rank.Six)], 
                        [Hearts(Rank.Eight), Spades(Rank.Ten)]),
                    // 12. Straight with low cards on table
                    ([Hearts(Rank.Ace), Diamonds(Rank.Two), Clubs(Rank.Three), Spades(Rank.Four), Diamonds(Rank.Five)], 
                        [Hearts(Rank.Seven), Spades(Rank.Eight)]),
                    // 13. Straight with one kicker card
                    ([Clubs(Rank.Jack), Diamonds(Rank.Queen), Hearts(Rank.King), Spades(Rank.Ace), Diamonds(Rank.Two)], 
                        [Hearts(Rank.Ten), Spades(Rank.Nine)]),
                    // 14. Straight with high card in hand
                    ([Clubs(Rank.Six), Diamonds(Rank.Seven), Hearts(Rank.Eight), Spades(Rank.Nine)], 
                        [Diamonds(Rank.Ten), Spades(Rank.Jack)]),
                    // 15. Straight with scattered cards
                    ([Clubs(Rank.Five), Diamonds(Rank.Seven), Hearts(Rank.Nine), Spades(Rank.Jack), Diamonds(Rank.King)], 
                        [Hearts(Rank.Ten), Spades(Rank.Eight)]),
                    // 16. Straight with one pair on the flop
                    ([Clubs(Rank.Four), Diamonds(Rank.Five), Spades(Rank.Six)], 
                        [Hearts(Rank.Seven), Spades(Rank.Eight)]),
                    // 17. Straight with one card on the turn
                    ([Clubs(Rank.Three), Diamonds(Rank.Four), Spades(Rank.Five), Hearts(Rank.Six)], 
                        [Diamonds(Rank.Seven), Spades(Rank.Eight)]),
                    // 18. Straight with one card on the river
                    ([Hearts(Rank.Two), Diamonds(Rank.Three), Clubs(Rank.Four), Spades(Rank.Five), Diamonds(Rank.Six)], 
                        [Hearts(Rank.Seven), Spades(Rank.Eight)]),
                    // 19. Straight with all community cards
                    ([Clubs(Rank.Seven), Diamonds(Rank.Eight), Hearts(Rank.Nine), Spades(Rank.Ten), Diamonds(Rank.Jack)], 
                        [Hearts(Rank.Queen), Spades(Rank.King)]),
                    // 20. Straight with low community cards
                    ([Hearts(Rank.Two), Diamonds(Rank.Three), Clubs(Rank.Four), Spades(Rank.Five), Diamonds(Rank.Six)], 
                        [Hearts(Rank.Seven), Spades(Rank.Eight)]),
                ]
            },
            {
                HandRank.ThreeOfAKind,
                [
                    // highest Three of a kind
                    ([Clubs(Rank.Ace), Spades(Rank.Jack), Hearts(Rank.Queen), Hearts(Rank.King), Hearts(Rank.Ace)], 
                        [Hearts(Rank.Nine), Spades(Rank.Ace)]),
                    // Lowest Three of a kind
                    ([Clubs(Rank.Two), Spades(Rank.Three), Hearts(Rank.Four), Hearts(Rank.Five), Hearts(Rank.Two)], 
                        [Hearts(Rank.Seven), Spades(Rank.Two)]),
                    // flop
                    ([Clubs(Rank.Ace), Spades(Rank.Jack), Hearts(Rank.Ace)], 
                        [Hearts(Rank.Nine), Spades(Rank.Ace)]),
                    // turn
                    ([Clubs(Rank.Two), Spades(Rank.Three), Hearts(Rank.Four), Hearts(Rank.Two)], 
                        [Hearts(Rank.Seven), Spades(Rank.Two)]),
                        // Existing cases...
                    // Additional cases
                    // 1. Three of a kind with high kickers
                    ([Hearts(Rank.Ace), Diamonds(Rank.King), Spades(Rank.Ace), Clubs(Rank.Jack), Hearts(Rank.Nine)], 
                        [Clubs(Rank.Ace), Diamonds(Rank.Queen)]),
                    // 2. Three of a kind with low kickers
                    ([Clubs(Rank.Four), Diamonds(Rank.Four), Hearts(Rank.Two), Spades(Rank.Five), Diamonds(Rank.Six)], 
                        [Hearts(Rank.Four), Spades(Rank.Seven)]),
                    // 3. Three of a kind with one kicker in hand
                    ([Hearts(Rank.Jack), Diamonds(Rank.Jack), Spades(Rank.Three), Clubs(Rank.Five), Hearts(Rank.Seven)], 
                        [Clubs(Rank.Jack), Diamonds(Rank.Ten)]),
                    // 4. Three of a kind formed on the flop
                    ([Hearts(Rank.Nine), Diamonds(Rank.Nine), Spades(Rank.Nine)], 
                        [Clubs(Rank.Eight), Diamonds(Rank.Seven)]),
                    // 5. Three of a kind formed on the turn
                    ([Clubs(Rank.Six), Diamonds(Rank.Six), Spades(Rank.Four), Hearts(Rank.Six)], 
                        [Diamonds(Rank.Queen), Spades(Rank.Jack)]),
                    // 6. Three of a kind with mixed suits
                    ([Clubs(Rank.Ten), Diamonds(Rank.Ten), Hearts(Rank.Ace), Spades(Rank.Ten), Diamonds(Rank.Three)], 
                        [Hearts(Rank.King), Spades(Rank.Two)]),
                    // 7. Three of a kind with one kicker
                    ([Clubs(Rank.Eight), Diamonds(Rank.Eight), Spades(Rank.Seven), Hearts(Rank.Eight), Diamonds(Rank.Five)], 
                        [Hearts(Rank.Four), Spades(Rank.Three)]),
                    // 8. Three of a kind with both kickers in hand
                    ([Clubs(Rank.Five), Diamonds(Rank.Two), Hearts(Rank.Three)], 
                        [Spades(Rank.Five), Diamonds(Rank.Five)]),
                    // 9. Three of a kind with low cards on the table
                    ([Hearts(Rank.Three), Diamonds(Rank.Three), Clubs(Rank.Two), Spades(Rank.Three), Diamonds(Rank.Four)], 
                        [Hearts(Rank.Ace), Spades(Rank.King)]),
                    // 10. Three of a kind with a high card in hand
                    ([Clubs(Rank.Seven), Diamonds(Rank.Seven), Hearts(Rank.Six), Spades(Rank.Seven), Diamonds(Rank.Nine)], 
                        [Hearts(Rank.Ace), Spades(Rank.Jack)]),
                    // 11. Three of a kind formed with pocket cards
                    ([Hearts(Rank.Four), Diamonds(Rank.Six), Clubs(Rank.Eight), Spades(Rank.Jack), Diamonds(Rank.Queen)], 
                        [Clubs(Rank.Four), Spades(Rank.Four)]),
                    // 12. Three of a kind with scattered kickers
                    ([Clubs(Rank.Nine), Diamonds(Rank.Nine), Hearts(Rank.Ten), Spades(Rank.Nine), Diamonds(Rank.Ace)], 
                        [Hearts(Rank.Queen), Spades(Rank.King)]),
                    // 13. Three of a kind with one low kicker
                    ([Hearts(Rank.Two), Diamonds(Rank.Two), Clubs(Rank.Five), Spades(Rank.Two), Diamonds(Rank.Seven)], 
                        [Hearts(Rank.Nine), Spades(Rank.Eight)]),
                    // 14. Three of a kind with one high kicker
                    ([Clubs(Rank.Jack), Diamonds(Rank.Jack), Hearts(Rank.Ace), Spades(Rank.Jack), Diamonds(Rank.King)], 
                        [Hearts(Rank.Queen), Spades(Rank.Nine)]),
                    // 15. Three of a kind with mixed ranks in hand
                    ([Hearts(Rank.Six), Diamonds(Rank.Eight), Clubs(Rank.Ten), Spades(Rank.Six), Diamonds(Rank.Queen)], 
                        [Clubs(Rank.Six), Spades(Rank.Four)]),
                    // 16. Three of a kind with one pair on the flop
                    ([Clubs(Rank.Seven), Diamonds(Rank.Seven), Spades(Rank.Seven)], 
                        [Hearts(Rank.Two), Spades(Rank.Five)]),
                    // 17. Three of a kind with one card on the turn
                    ([Clubs(Rank.Eight), Diamonds(Rank.Eight), Spades(Rank.Two), Hearts(Rank.Eight)], 
                        [Diamonds(Rank.Six), Spades(Rank.Three)]),
                    // 18. Three of a kind with one card on the river
                    ([Hearts(Rank.Five), Diamonds(Rank.Five), Clubs(Rank.Nine), Spades(Rank.Five), Diamonds(Rank.Ten)], 
                        [Hearts(Rank.Jack), Spades(Rank.Four)]),
                    // 19. Three of a kind with all community cards
                    ([Clubs(Rank.Three), Diamonds(Rank.Three), Hearts(Rank.Three), Spades(Rank.Two), Diamonds(Rank.Jack)], 
                        [Hearts(Rank.King), Spades(Rank.Queen)]),
                    // 20. Three of a kind with low community cards
                    ([Hearts(Rank.Two), Diamonds(Rank.Two), Clubs(Rank.Two), Spades(Rank.Four), Diamonds(Rank.Five)], 
                        [Hearts(Rank.Seven), Spades(Rank.Eight)]),
                ]
            },
            {
                HandRank.TwoPairs,
                [
                    // highest Two Pairs
                    ([Clubs(Rank.Ace), Spades(Rank.Jack), Hearts(Rank.Queen), Hearts(Rank.King), Hearts(Rank.Ace)], 
                        [Hearts(Rank.Nine), Spades(Rank.King)]),
                    // Lowest two pairs
                    ([Clubs(Rank.Two), Spades(Rank.Three), Hearts(Rank.Four), Hearts(Rank.Five), Hearts(Rank.Three)], 
                        [Hearts(Rank.Seven), Spades(Rank.Two)]),
                    // flop
                    ([Clubs(Rank.Ace), Hearts(Rank.King), Hearts(Rank.Ace)], 
                        [Hearts(Rank.Nine), Spades(Rank.King)]),
                    // turn
                    ([Clubs(Rank.Two), Spades(Rank.Three), Hearts(Rank.Five), Hearts(Rank.Three)], 
                        [Hearts(Rank.Seven), Spades(Rank.Two)]),
                        // Existing cases...
                    // Additional cases
                    // 1. Two pairs with high kickers
                    ([Clubs(Rank.Ace), Diamonds(Rank.Jack), Hearts(Rank.Queen), Spades(Rank.Ace), Hearts(Rank.Jack)], 
                        [Hearts(Rank.King), Spades(Rank.Nine)]),
                    // 2. Two pairs with low kickers
                    ([Clubs(Rank.Two), Diamonds(Rank.Four), Spades(Rank.Two), Hearts(Rank.Four), Hearts(Rank.Seven)], 
                        [Hearts(Rank.Three), Spades(Rank.Six)]),
                    // 3. Two pairs with one pair in hand
                    ([Clubs(Rank.Ace), Diamonds(Rank.Jack), Hearts(Rank.Queen), Spades(Rank.Three), Hearts(Rank.Six)], 
                        [Diamonds(Rank.Ace), Spades(Rank.Jack)]),
                    // 4. Two pairs with mixed suits
                    ([Clubs(Rank.Seven), Diamonds(Rank.Nine), Spades(Rank.Seven), Hearts(Rank.Ten), Diamonds(Rank.Ten)], 
                        [Hearts(Rank.Four), Spades(Rank.Two)]),
                    // 5. Two pairs with one high pair and one low pair
                    ([Clubs(Rank.Ace), Diamonds(Rank.King), Hearts(Rank.Three), Spades(Rank.Ace), Hearts(Rank.Three)], 
                        [Diamonds(Rank.Queen), Spades(Rank.Jack)]),
                    // 6. Two pairs with one pair on the flop
                    ([Clubs(Rank.Five), Diamonds(Rank.Eight), Spades(Rank.Eight)], 
                        [Hearts(Rank.Five), Spades(Rank.King)]),
                    // 7. Two pairs with one pair on the turn
                    ([Clubs(Rank.Nine), Diamonds(Rank.Six), Spades(Rank.Six), Hearts(Rank.Nine)], 
                        [Diamonds(Rank.Queen), Spades(Rank.Jack)]),
                    // 8. Two pairs with both pairs in hand
                    ([Clubs(Rank.Ace), Diamonds(Rank.Two), Hearts(Rank.Three)], 
                        [Spades(Rank.Ace), Diamonds(Rank.Three)]),
                    // 9. Two pairs with no high card possibility
                    ([Clubs(Rank.Four), Diamonds(Rank.Six), Hearts(Rank.Eight), Spades(Rank.Four), Diamonds(Rank.Eight)], 
                        [Hearts(Rank.Two), Spades(Rank.Three)]),
                    // 10. Two pairs with one kicker
                    ([Clubs(Rank.Ten), Diamonds(Rank.Jack), Spades(Rank.Queen), Hearts(Rank.Jack), Diamonds(Rank.Queen)], 
                        [Hearts(Rank.Three), Spades(Rank.Ace)]),
                    // 11. Two pairs with scattered kickers
                    ([Clubs(Rank.Seven), Diamonds(Rank.Eight), Hearts(Rank.Nine), Spades(Rank.Seven), Diamonds(Rank.Nine)], 
                        [Hearts(Rank.Five), Spades(Rank.Three)]),
                    // 12. Two pairs with one low pair and one mid-range pair
                    ([Clubs(Rank.Two), Diamonds(Rank.Six), Hearts(Rank.Nine), Spades(Rank.Two), Diamonds(Rank.Nine)], 
                        [Hearts(Rank.Ace), Spades(Rank.King)]),
                    // 13. Two pairs with one pair in the middle of the community cards
                    ([Hearts(Rank.Ace), Diamonds(Rank.King), Clubs(Rank.Queen), Spades(Rank.Jack), Hearts(Rank.Queen)], 
                        [Diamonds(Rank.Jack), Clubs(Rank.Nine)]),
                    // 14. Two pairs with high card in hand
                    ([Clubs(Rank.Two), Diamonds(Rank.Four), Spades(Rank.Six), Hearts(Rank.Eight), Diamonds(Rank.Ten)], 
                        [Hearts(Rank.Ten), Spades(Rank.Eight)]),
                    // 15. Two pairs with low card in hand
                    ([Hearts(Rank.Ace), Diamonds(Rank.King), Clubs(Rank.Queen), Spades(Rank.Jack), Hearts(Rank.Nine)], 
                        [Diamonds(Rank.Nine), Clubs(Rank.Jack)]),
                    // 16. Two pairs with a mixed suit in the community cards
                    ([Hearts(Rank.Ten), Diamonds(Rank.Jack), Clubs(Rank.Queen), Spades(Rank.King), Hearts(Rank.Jack)], 
                        [Diamonds(Rank.Queen), Clubs(Rank.Ten)]),
                    // 17. Two pairs with different suits in hand and table
                    ([Clubs(Rank.Ace), Diamonds(Rank.Jack), Hearts(Rank.Ten), Spades(Rank.Ace), Hearts(Rank.Jack)], 
                        [Diamonds(Rank.Ten), Clubs(Rank.King)]),
                    // 18. Two pairs with one kicker in hand
                    ([Clubs(Rank.Five), Diamonds(Rank.Seven), Hearts(Rank.Nine), Spades(Rank.Five), Diamonds(Rank.Nine)], 
                        [Hearts(Rank.King), Spades(Rank.Seven)]),
                    // 19. Two pairs with one kicker in hand
                    ([Clubs(Rank.Three), Diamonds(Rank.Five), Hearts(Rank.Seven), Spades(Rank.Three), Diamonds(Rank.Seven)], 
                        [Hearts(Rank.Ace), Spades(Rank.King)]),
                    // 20. Two pairs with both pairs scattered in the community cards
                    ([Hearts(Rank.Two), Diamonds(Rank.Four), Clubs(Rank.Six), Spades(Rank.Eight), Hearts(Rank.Ten)], 
                        [Diamonds(Rank.Six), Clubs(Rank.Four)]),
                ]
            },
            {
                HandRank.OnePair,
                [
                    // highest pair
                    ([Clubs(Rank.Ace), Spades(Rank.Jack), Hearts(Rank.Queen), Hearts(Rank.King), Hearts(Rank.Ace)], 
                        [Hearts(Rank.Nine), Spades(Rank.Eight)]),
                    // Lowest pair
                    ([Clubs(Rank.Two), Spades(Rank.Three), Hearts(Rank.Four), Hearts(Rank.Five), Hearts(Rank.Seven)], 
                        [Hearts(Rank.Nine), Spades(Rank.Two)]),
                    // just the pre flop
                    ([], 
                        [Hearts(Rank.Ace), Spades(Rank.Ace)]),
                    // flop
                    ([Clubs(Rank.Ace), Spades(Rank.Jack), Hearts(Rank.Ace)], 
                        [Hearts(Rank.Nine), Spades(Rank.Eight)]),
                    // turn
                    ([Clubs(Rank.Two), Spades(Rank.Three), Hearts(Rank.Four), Hearts(Rank.Seven)], 
                        [Hearts(Rank.Six), Spades(Rank.Two)]),
                    // 1. Pair with highest possible kicker
                    ([Diamonds(Rank.Ace), Spades(Rank.Jack), Hearts(Rank.Six), Clubs(Rank.King), Spades(Rank.Ace)], 
                        [Diamonds(Rank.Ten), Hearts(Rank.Nine)]),
                    // 2. Pair with lowest possible kicker
                    ([Clubs(Rank.Two), Diamonds(Rank.Four), Spades(Rank.King), Hearts(Rank.Seven), Diamonds(Rank.Two)], 
                        [Clubs(Rank.Three), Hearts(Rank.Six)]),
                    // 3. Pair in the middle of the table
                    ([Diamonds(Rank.Three), Spades(Rank.Five), Hearts(Rank.Seven), Clubs(Rank.Seven), Spades(Rank.Nine)], 
                        [Diamonds(Rank.Two), Hearts(Rank.Four)]),
                    // 4. Pair with all other higher cards
                    ([Clubs(Rank.Jack), Diamonds(Rank.Queen), Spades(Rank.Six), Hearts(Rank.Ace), Diamonds(Rank.Jack)], 
                        [Clubs(Rank.Two), Hearts(Rank.Three)]),
                    // 5. Pair with one card on the table
                    ([Diamonds(Rank.Ace), Spades(Rank.Jack), Hearts(Rank.Six), Clubs(Rank.King), Spades(Rank.Ten)], 
                        [Diamonds(Rank.Ace), Hearts(Rank.Nine)]),
                    // 6. Pair with one card in hand
                    ([Diamonds(Rank.Three), Spades(Rank.Four), Hearts(Rank.Five), Clubs(Rank.Eight), Spades(Rank.Seven)], 
                        [Spades(Rank.Three), Hearts(Rank.Two)]),
                    // 7. Pair with two high kickers
                    ([Diamonds(Rank.Ace), Spades(Rank.King), Hearts(Rank.Queen), Clubs(Rank.Ten), Spades(Rank.Ace)], 
                        [Diamonds(Rank.Nine), Hearts(Rank.Eight)]),
                    // 8. Pair with two low kickers
                    ([Clubs(Rank.Two), Diamonds(Rank.Four), Spades(Rank.Eight), Hearts(Rank.Seven), Diamonds(Rank.Two)], 
                        [Clubs(Rank.Three), Hearts(Rank.Six)]),
                    // 9. Pair with scattered kickers
                    ([Clubs(Rank.Two), Diamonds(Rank.Six), Spades(Rank.Ten), Hearts(Rank.Queen), Diamonds(Rank.Two)], 
                        [Clubs(Rank.Ace), Hearts(Rank.King)]),
                    // 10. Pair with one kicker same as one of the community cards
                    ([Diamonds(Rank.Ace), Spades(Rank.Seven), Hearts(Rank.Queen), Clubs(Rank.King), Spades(Rank.Jack)], 
                        [Diamonds(Rank.Nine), Hearts(Rank.King)]),
                    // 11. Pair with one high card and one low card in hand
                    ([Diamonds(Rank.Six), Spades(Rank.Four), Hearts(Rank.Five), Clubs(Rank.Six), Spades(Rank.Seven)], 
                        [Diamonds(Rank.Ace), Hearts(Rank.Two)]),
                    // 12. Pair with one high card on the table
                    ([Hearts(Rank.Ace), Diamonds(Rank.Four), Clubs(Rank.Queen), Spades(Rank.Jack), Hearts(Rank.Ten)], 
                        [Diamonds(Rank.Ten), Clubs(Rank.Nine)]),
                    // 13. Pair with mixed suits
                    ([Hearts(Rank.Four), Diamonds(Rank.Seven), Clubs(Rank.Nine), Spades(Rank.Jack), Hearts(Rank.Nine)], 
                        [Diamonds(Rank.Three), Clubs(Rank.Five)]),
                    // 14. Pair with no straight or flush potential
                    ([Hearts(Rank.Two), Diamonds(Rank.Six), Clubs(Rank.Eight), Spades(Rank.Ten), Hearts(Rank.Jack)], 
                        [Diamonds(Rank.Jack), Clubs(Rank.Four)]),
                    // 15. Pair with one high card and one mid-range card in hand
                    ([Hearts(Rank.Two), Diamonds(Rank.Five), Clubs(Rank.Eight), Spades(Rank.Ten), Hearts(Rank.Queen)], 
                        [Diamonds(Rank.Queen), Clubs(Rank.Seven)]),
                    // 16. Pair with one card in the middle of the community cards
                    ([Hearts(Rank.Two), Diamonds(Rank.Four), Clubs(Rank.Six), Spades(Rank.Eight), Hearts(Rank.Ten)], 
                        [Diamonds(Rank.Six), Clubs(Rank.Ace)]),
                    // 17. Pair on the flop
                    ([Hearts(Rank.Two), Diamonds(Rank.Four), Clubs(Rank.Six)], 
                        [Spades(Rank.Six), Hearts(Rank.Ace)]),
                    // 18. Pair on the turn
                    ([Hearts(Rank.Two), Diamonds(Rank.Four), Clubs(Rank.Six), Spades(Rank.Eight)], 
                        [Hearts(Rank.Six), Diamonds(Rank.Ace)]),
                    // 19. Pair with high card in hand and low card on the table
                    ([Clubs(Rank.Two), Diamonds(Rank.Four), Spades(Rank.Six), Hearts(Rank.Eight), Diamonds(Rank.Ten)], 
                        [Hearts(Rank.Ten), Spades(Rank.Ace)]),
                    // 20. Pair with low card in hand and high card on the table
                    ([Hearts(Rank.Ace), Diamonds(Rank.King), Clubs(Rank.Queen), Spades(Rank.Jack), Hearts(Rank.Nine)], 
                        [Diamonds(Rank.Nine), Clubs(Rank.Two)]),
                ]
            },
            {
                HandRank.HighCard,
                [
                    // highest High card
                    ([Clubs(Rank.Ace), Spades(Rank.Jack), Hearts(Rank.Queen), Hearts(Rank.King), Hearts(Rank.Seven)], 
                        [Hearts(Rank.Nine), Spades(Rank.Eight)]),
                    // Lowest High card
                    ([Clubs(Rank.Two), Spades(Rank.Three), Hearts(Rank.Four), Hearts(Rank.Five), Hearts(Rank.Seven)], 
                        [Hearts(Rank.Nine), Spades(Rank.Eight)]),
                    // just the pre flop
                    ([], 
                        [Hearts(Rank.Six), Spades(Rank.Eight)]),
                    // flop
                    ([Clubs(Rank.Two), Spades(Rank.Three), Hearts(Rank.Four)], 
                        [Hearts(Rank.Six), Spades(Rank.Eight)]),
                    // tuen
                    ([Clubs(Rank.Two), Spades(Rank.Three), Hearts(Rank.Four), Hearts(Rank.Nine)], 
                        [Hearts(Rank.Six), Spades(Rank.Eight)]),
                    // 1. High card with a mix of suits
                    ([Hearts(Rank.Four), Diamonds(Rank.Seven), Clubs(Rank.Nine), Spades(Rank.Jack), Hearts(Rank.Two)], 
                        [Diamonds(Rank.Three), Clubs(Rank.Five)]),
                    // 2. High card with a straight draw
                    ([Hearts(Rank.Four), Diamonds(Rank.Five), Clubs(Rank.Six), Spades(Rank.Eight), Hearts(Rank.Nine)], 
                        [Diamonds(Rank.Ten), Clubs(Rank.Jack)]),
                    // 3. High card with a flush draw
                    ([Hearts(Rank.Four), Hearts(Rank.Seven), Hearts(Rank.Nine), Hearts(Rank.Jack), Spades(Rank.Two)], 
                        [Diamonds(Rank.Three), Clubs(Rank.Five)]),
                    // 4. High card 
                    ([Hearts(Rank.Four), Diamonds(Rank.King), Clubs(Rank.Six), Spades(Rank.Eight), Hearts(Rank.Nine)], 
                        [Diamonds(Rank.Ten), Clubs(Rank.Jack)]),
                    // 5. High card with all low cards
                    ([Hearts(Rank.Two), Diamonds(Rank.Three), Clubs(Rank.Four), Spades(Rank.Five), Hearts(Rank.Jack)], 
                        [Diamonds(Rank.Seven), Clubs(Rank.Eight)]),
                    // 6. High card with Ace on the table
                    ([Hearts(Rank.Ace), Diamonds(Rank.Three), Clubs(Rank.Four), Spades(Rank.Five), Hearts(Rank.Six)], 
                        [Diamonds(Rank.Nine), Clubs(Rank.Eight)]),
                    // 7. High card with only one high card on the table
                    ([Hearts(Rank.Two), Diamonds(Rank.Three), Clubs(Rank.Four), Spades(Rank.Five), Diamonds(Rank.King)], 
                        [Hearts(Rank.Seven), Clubs(Rank.Eight)]),
                    // 8. High card with a scattered range of cards
                    ([Clubs(Rank.Two), Spades(Rank.Six), Diamonds(Rank.Ten), Hearts(Rank.Queen), Clubs(Rank.King)], 
                        [Hearts(Rank.Three), Spades(Rank.Four)]),
                    // 9. High card with no consecutive cards
                    ([Hearts(Rank.Two), Diamonds(Rank.Seven), Clubs(Rank.Eight), Spades(Rank.Jack), Hearts(Rank.Ace)], 
                        [Diamonds(Rank.Three), Clubs(Rank.Four)]),
                    // 10. High card with mixed high and low cards
                    ([Hearts(Rank.Two), Diamonds(Rank.Seven), Clubs(Rank.Nine), Spades(Rank.Queen), Hearts(Rank.Ace)], 
                        [Diamonds(Rank.Three), Clubs(Rank.Four)]),
                    // 11. High card with no pair potential
                    ([Hearts(Rank.Two), Diamonds(Rank.Six), Clubs(Rank.Eight), Spades(Rank.Ten), Hearts(Rank.Queen)], 
                        [Diamonds(Rank.Three), Clubs(Rank.Four)]),
                    // 12. High card with one high card in player's hand
                    ([Hearts(Rank.Two), Diamonds(Rank.Three), Clubs(Rank.Four), Spades(Rank.King), Hearts(Rank.Six)], 
                        [Diamonds(Rank.Seven), Spades(Rank.Ace)]),
                    // 13. High card with player's hand lower than table cards
                    ([Hearts(Rank.Ten), Diamonds(Rank.Jack), Clubs(Rank.Six), Spades(Rank.King), Hearts(Rank.Ace)], 
                        [Diamonds(Rank.Two), Clubs(Rank.Three)]),
                    // 14. High card with one high card in player's hand, rest low
                    ([Hearts(Rank.Two), Diamonds(Rank.Three), Clubs(Rank.Four), Spades(Rank.Eight), Hearts(Rank.Six)], 
                        [Diamonds(Rank.Seven), Spades(Rank.King)]),
                    // 15. High card with all cards below Ten
                    ([Hearts(Rank.Two), Diamonds(Rank.Three), Clubs(Rank.Four), Spades(Rank.Eight), Hearts(Rank.Nine)], 
                        [Diamonds(Rank.Six), Clubs(Rank.Seven)]),
                    // 16. High card with a broken straight
                    ([Hearts(Rank.Two), Diamonds(Rank.Three), Clubs(Rank.Four), Spades(Rank.Six), Hearts(Rank.Seven)], 
                        [Diamonds(Rank.Eight), Clubs(Rank.Nine)]),
                    // 17. High card with a broken flush
                    ([Hearts(Rank.Two), Hearts(Rank.Three), Hearts(Rank.Four), Hearts(Rank.Six), Spades(Rank.Seven)], 
                        [Diamonds(Rank.Eight), Clubs(Rank.Nine)]),
                    // 18. High card with scattered mid-range cards
                    ([Hearts(Rank.Five), Diamonds(Rank.Seven), Clubs(Rank.Eight), Spades(Rank.Ten), Hearts(Rank.Jack)], 
                        [Diamonds(Rank.Two), Clubs(Rank.Three)]),
                    // 19. High card with highest card on the table
                    ([Hearts(Rank.Ace), Diamonds(Rank.King), Clubs(Rank.Eight), Spades(Rank.Jack), Hearts(Rank.Ten)], 
                        [Diamonds(Rank.Two), Clubs(Rank.Three)]),
                ]

            },
        };
    private static Card Hearts(Rank rank) => new Card(rank, Suit.Hearts);
    private static Card Spades(Rank rank) => new Card(rank, Suit.Spades);
    private static Card Diamonds(Rank rank) => new Card(rank, Suit.Diamonds);
    private static Card Clubs(Rank rank) => new Card(rank, Suit.Clubs);
}
