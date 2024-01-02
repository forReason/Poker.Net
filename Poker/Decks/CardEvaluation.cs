using Poker.Cards;

namespace Poker.Decks;

public class CardEvaluation
{
    /// <summary>
    /// takes the community cards and player pocket in order to generate a HandScore which can easily be compared to each other with (==) operators
    /// </summary>
    /// <param name="communityCards">the shared community cards on the table</param>
    /// <param name="playerPocketCards">the player hand cards</param>
    /// <returns>a HandScore containing the score of the hand and operators for comparison</returns>
    public static HandScore ScoreCards(Card[] communityCards, PocketCards playerPocketCards)
    {

        // build card List, ordered by descending
        List<Card> allCards = [..communityCards];
        if (playerPocketCards.CardCount > 0)
            allCards.AddRange(playerPocketCards.Cards.Cast<Card>());
        allCards = allCards.OrderByDescending(c => c.Rank).ToList();

        // Score Cards
        HandScore? scoredCards = null;
        Dictionary<Suit, List<Card>> suitList = BuildSuitDictionary(allCards);
        // royal flush or straight flush
        scoredCards = IsStraightFlush(suitList);
        if (scoredCards != null)
            return scoredCards;

        // build rank dictionary after straight flush for the minority of cases where we have a straight flush (minor performance improvement)
        var rankList = BuildRankDictionary(allCards);

        // four of a kind
        scoredCards = IsFourOfAKind(rankList);
        if (scoredCards != null)
            return scoredCards;

        // Full House
        scoredCards = IsFullHouse(rankList);
        if (scoredCards != null)
            return scoredCards;

        // flush
        scoredCards = IsFlush(suitList);
        if (scoredCards != null)
            return scoredCards;

        // Straight
        scoredCards = IsStraight(allCards);
        if (scoredCards != null)
            return scoredCards;

        // Three of a kind
        scoredCards = IsThreeOfAKind(rankList);
        if (scoredCards != null)
            return scoredCards;

        // two pair
        scoredCards = IsTwoPair(rankList);
        if (scoredCards != null)
            return scoredCards;

        // one pair
        scoredCards = IsPair(rankList);
        if (scoredCards != null)
            return scoredCards;

        // leftover: High Card
        List<Rank> list = new List<Rank>();
        foreach (var c in allCards.Take(5)) list.Add(c.Rank);
        HandScore hand = new HandScore()
        {
            Rank = HandRank.HighCard,
            Score = list.ToArray()
        };
        return hand;
    }

    /// <summary>
    /// takes a sorted list of cards (rank descending) and organizes them by their suit.
    /// </summary>
    /// <remarks>
    ///this is useful to compare suited cards. Eg for a flush, straight flush or royal flush.
    /// </remarks>
    /// <param name="allCards">the input cards, preferably sorted by rank descending</param>
    /// <returns>a dictionary with up to 4 suits. Each suit contains its list of cards, sorted by rank descending</returns>
    private static Dictionary<Suit, List<Card>> BuildSuitDictionary(IEnumerable<Card> allCards)
    {
        Dictionary<Suit, List<Card>> resultDictionary = new Dictionary<Suit, List<Card>>();
        foreach (Card card in allCards)
        {
            if (resultDictionary.TryGetValue(card.Suit, out var suitedCards))
            {
                suitedCards.Add(card);
            }
            else
            {
                resultDictionary[card.Suit] = [card];
            }
        }

        return resultDictionary;
    }

    /// <summary>
    /// builds a sorted dictionary of card ranks, sorted by rank descending.
    /// </summary>
    /// <remarks>
    /// this is useful to find pairs
    /// </remarks>
    /// <param name="allCards">all cards to build the Dictionary with, organized y rank descending</param>
    /// <returns>a dictionary with the rank as key and the count of this rank as value</returns>
    private static SortedDictionary<Rank, ulong> BuildRankDictionary(IEnumerable<Card> allCards)
    {
        SortedDictionary<Rank, ulong> resultDictionary = new SortedDictionary<Rank, ulong>(
            Comparer<Rank>.Create((x, y) => y.CompareTo(x)) // sort by rank descending
        );
        foreach (Card card in allCards)
        {
            if (resultDictionary.ContainsKey(card.Rank))
            {
                resultDictionary[card.Rank]++;
            }
            else
            {
                resultDictionary[card.Rank] = 1;
            }
        }

        return resultDictionary;
    }

    /// <summary>
    /// checks if the supplied Cards contain a straight flush. Handles Royal Flush as well.
    /// </summary>
    /// <remarks>
    /// for tie resolution, compare the returned hand (Rank[]) with a simple high card comparison.
    /// </remarks>
    /// <param name="cards">the cards to check</param>
    /// <returns>a HandScore with the straight rank as score</returns>
    private static HandScore? IsStraightFlush(Dictionary<Suit, List<Card>> cards)
    {
        foreach (List<Card> cardList in cards.Values)
        {
            if (cardList.Count >= 5)
            {
                HandScore? straight = IsStraight(cardList);
                if (straight != null)
                {
                    HandScore hand = new HandScore()
                    {
                        Rank = HandRank.StraightFlush,
                        Score = straight.Score
                    };
                    if (straight.Score[0] == Rank.Ace)
                        hand.Rank = HandRank.RoyalFlush;
                    return hand;
                }

                break;
            }
        }

        return null;
    }

    /// <summary>
    /// Checks if there is at least one pair of four in the supplied cards array.
    /// </summary>
    /// <remarks>
    /// for tie resolution, compare the returned hand (Rank[]) with a simple high card comparison.
    /// </remarks>
    /// <param name="cards">The grouped card ranks from <see cref="BuildRankDictionary"/></param>
    /// <returns>The hand which was formed with the pair. Note that the array contains 2 elements, since the first card (pair) is a quadruplet.</returns>
    private static HandScore? IsFourOfAKind(SortedDictionary<Rank, ulong> cards)
    {
        Rank? kicker = null;
        Rank? pairRank = null;
        foreach (KeyValuePair<Rank, ulong> entry in cards)
        {
            if (entry.Value == 4 && pairRank == null)
            {
                pairRank = entry.Key;
            }
            else if (kicker == null)
            {
                kicker = entry.Key;
            }
            else if (pairRank != null) // hand is full
            {
                break;
            }
        }

        if (pairRank == null)
            return null;
        HandScore hand = new HandScore()
        {
            Rank = HandRank.FourOfAKind,
            Score = [pairRank.Value, kicker!.Value]
        };
        return hand;
    }


    /// <summary>
    /// Checks if the supplied Cards contain a full house.
    /// </summary>
    /// <remarks>
    /// for tie resolution, compare the returned hand (Rank[]) with a simple high card comparison.
    /// </remarks>
    /// <param name="cards">The grouped card ranks from <see cref="BuildRankDictionary"/></param>
    /// <returns>if a full house was found, returns a hand score with the highest three pair rank in score[0] and highest two pair rank in score[1]</returns>
    private static HandScore? IsFullHouse(SortedDictionary<Rank, ulong> cards)
    {
        Rank? highestThreePairRank = null;
        Rank? highestTwoPairRank = null;
        foreach (KeyValuePair<Rank, ulong> stack in cards)
        {
            if (stack.Value == 3)
            {
                if (highestThreePairRank == null)
                    highestThreePairRank = stack.Key;
                else if (stack.Key > highestThreePairRank)
                {
                    if (highestTwoPairRank == null || highestThreePairRank > highestTwoPairRank)
                        highestTwoPairRank = highestThreePairRank;
                    highestThreePairRank = stack.Key;
                }
                else if (highestTwoPairRank == null || stack.Key > highestTwoPairRank)
                {
                    highestTwoPairRank = stack.Key;
                }

            }
            else if (stack.Value == 2 && (highestTwoPairRank == null || stack.Key > highestTwoPairRank))
            {
                highestTwoPairRank = stack.Key;
            }
        }

        if (highestThreePairRank != null && highestTwoPairRank != null)
        {
            HandScore hand = new HandScore()
            {
                Rank = HandRank.FullHouse,
                Score = [highestThreePairRank.Value, highestTwoPairRank.Value]
            };
            return hand;
        }

        return null;
    }

    /// <summary>
    /// checks if the supplied cards contain a flush
    /// </summary>
    /// <remarks>
    /// for tie resolution, compare the returned hand (Rank[]) with a simple high card comparison.
    /// </remarks>
    /// <param name="cards">the list of cards, organized by suits from <see cref="BuildSuitDictionary"/></param>
    /// <returns>a HandScore with the 5 flush cards sorted from highest to lowest Rank</returns>
    private static HandScore? IsFlush(Dictionary<Suit, List<Card>> cards)
    {
        if (cards.Count > 3) // 3 cards and 4 suits 
            return null;
        foreach (List<Card> cardList in cards.Values)
        {
            if (cardList.Count < 5) continue;
            List<Rank> list = [];
            list.AddRange(cardList.Take(5).Select(c => c.Rank));

            HandScore hand = new HandScore()
            {
                Rank = HandRank.Flush,
                Score = list.ToArray()
            };
            return hand;
        }

        return null;
    }

    /// <summary>
    /// checks whether the supplied cards contain a street
    /// </summary>
    /// <remarks>
    /// for tie resolution, compare the returned hand (Rank[]) with a simple high card comparison.
    /// </remarks>
    /// <param name="cards">A list of cards, sorted by Rank Descending, Ace Highest</param>
    /// <returns>a HandScore with the Rank of the street if one was found</returns>
    private static HandScore? IsStraight(List<Card> cards)
    {
        if (cards.Count < 5)
            return null;
        ulong connectedCards = 1;
        Card highestConnectedCard = cards[0];
        Card lowestConnectedCard = cards[0];
        ulong wheelPossible = 0;
        if (cards[0].Rank == Rank.Ace && cards[^1].Rank == Rank.Two)
            wheelPossible++;
        for (int i = 1; i < cards.Count; i++)
        {
            if (cards[i].Rank == lowestConnectedCard.Rank) // neutral, card repeated
                continue;
            if (cards[i].Rank == lowestConnectedCard.Rank - 1) // found connected card
            {
                lowestConnectedCard = cards[i];
                connectedCards++;
                if (connectedCards == 5)
                {
                    HandScore hand = new HandScore()
                    {
                        Rank = HandRank.Straight,
                        Score = [highestConnectedCard.Rank]
                    };
                    return hand;
                }
            }
            else // either a repetition or not connected
            {
                if (cards[i].Rank != lowestConnectedCard.Rank) // there was a gap
                {
                    // reset straight
                    highestConnectedCard = cards[i];
                    lowestConnectedCard = cards[i];
                    connectedCards = 1;
                    // check if a straight is still possible
                    if (highestConnectedCard.Rank < Rank.Five - wheelPossible)
                        return null;
                    ulong maxStraightSpan = (highestConnectedCard.Rank - cards[^1].Rank)+1;
                    if (maxStraightSpan < 5 - wheelPossible) // account for possible wheel
                        return null;
                }
                else // was a repetition, chances shrank
                {
                    int chances = cards.Count - i;
                    if (connectedCards + (ulong)chances < 5 - wheelPossible) // account for possible wheel
                        return null;
                }
            }
        }

        connectedCards += wheelPossible;
        if (connectedCards == 5)
        {
            HandScore hand = new HandScore()
            {
                Rank = HandRank.Straight,
                Score = [highestConnectedCard.Rank]
            };
            return hand;
        }

        return null;
    }

    /// <summary>
    /// Checks if there is at least one pair of three in the supplied cards array.
    /// </summary>
    /// <remarks>
    /// for tie resolution, compare the returned hand (Rank[]) with a simple high card comparison.
    /// </remarks>
    /// <param name="cards">The grouped card ranks from <see cref="BuildRankDictionary"/></param>
    /// <returns>The hand which was formed with the pair. Note that the array contains 3 elements, since the first card (pair) is a triplet.</returns>
    private static HandScore? IsThreeOfAKind(SortedDictionary<Rank, ulong> cards)
    {
        List<Rank> kickers = new();
        Rank? pairRank = null;
        foreach (KeyValuePair<Rank, ulong> entry in cards)
        {
            if (entry.Value == 3 && pairRank == null)
            {
                pairRank = entry.Key;
            }
            else if (kickers.Count < 2)
            {
                kickers.Add(entry.Key);
            }
            else if (pairRank != null) // hand is full
            {
                break;
            }
        }

        if (pairRank == null)
            return null;
        HandScore hand = new HandScore()
        {
            Rank = HandRank.ThreeOfAKind,
            Score = [pairRank.Value, kickers[0], kickers[1]]
        };
        return hand;
    }

    /// <summary>
    /// Checks if there are at least two pairs in the cards.
    /// </summary>
    /// <remarks>
    /// for tie resolution, compare the returned hand (Rank[]) with a simple high card comparison.
    /// </remarks>
    /// <param name="cards">The grouped card ranks from <see cref="BuildRankDictionary"/>.</param>
    /// <returns>The hand which was formed with the two pairs. Note that the array contains 3 elements, since the first two cards (pairs) are a duplicate.</returns>
    private static HandScore? IsTwoPair(SortedDictionary<Rank, ulong> cards)
    {
        Rank? higherPair = null;
        Rank? lowerPair = null;
        Rank? kicker = null;
        foreach (KeyValuePair<Rank, ulong> entry in cards)
        {
            if (entry.Value == 2 && lowerPair == null)
            {
                if (higherPair == null)
                    higherPair = entry.Key;
                else
                {
                    lowerPair = entry.Key;
                }
            }
            else if (kicker == null)
            {
                kicker = entry.Key;
            }
            else if (lowerPair != null)
            {
                break;
            }
        }

        if (lowerPair == null || higherPair == null)
            return null;
        HandScore hand = new HandScore()
        {
            Rank = HandRank.TwoPairs,
            Score = [higherPair.Value, lowerPair.Value, kicker!.Value]
        };
        return hand;
    }

    /// <summary>
    /// Checks if there is at least one pair in the supplied cards array.
    /// </summary>
    /// <remarks>
    /// for tie resolution, compare the returned hand (Rank[]) with a simple high card comparison.
    /// </remarks>
    /// <param name="cards">The grouped card ranks from <see cref="BuildRankDictionary"/></param>
    /// <returns>The hand which was formed with the pair. Note that the array contains 4 elements, since the first card (pair) is a duplicate.</returns>
    private static HandScore? IsPair(SortedDictionary<Rank, ulong> cards)
    {
        if (cards.Count == 1)
        {
            HandScore pocketCards = new HandScore()
            {
                Rank = HandRank.OnePair,
                Score = [cards.First().Key]
            };
            return pocketCards;
        }

        List<Rank> kickers = [];
        Rank? pairRank = null;
        foreach (KeyValuePair<Rank, ulong> entry in cards)
        {
            if (entry.Value == 2 && pairRank == null)
            {
                pairRank = entry.Key;
            }
            else if (kickers.Count < 3)
            {
                kickers.Add(entry.Key);
            }
            else if (pairRank != null) // hand is full
            {
                break;
            }
        }

        if (pairRank == null)
            return null;
        HandScore hand = new HandScore()
        {
            Rank = HandRank.OnePair,
            Score = [pairRank.Value, kickers[0], kickers[1], kickers[2]]
        };
        return hand;
    }
}