using Poker.PhysicalObjects.Cards;

namespace Poker.PhysicalObjects.Decks;

/// <summary>
/// the CardEvaluation class is used to score hand against each other and determining the winner
/// </summary>
public static class CardEvaluation
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
        allCards = allCards.OrderByDescending(c => c.CardRank).ToList();

        // Score Cards
        Dictionary<CardSuit, List<Card>> suitList = BuildSuitDictionary(allCards);
        // royal flush or straight flush
        HandScore? scoredCards = IsStraightFlush(suitList);
        if (scoredCards != null)
            return scoredCards;

        // build CardRank dictionary after straight flush for the minority of cases where we have a straight flush (minor performance improvement)
        SortedDictionary<CardRank, ulong> cardRankList = BuildCardRankDictionary(allCards);

        // four of a kind
        scoredCards = IsFourOfAKind(cardRankList);
        if (scoredCards != null)
            return scoredCards;

        // Full House
        scoredCards = IsFullHouse(cardRankList);
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
        scoredCards = IsThreeOfAKind(cardRankList);
        if (scoredCards != null)
            return scoredCards;

        // two pair
        scoredCards = IsTwoPair(cardRankList);
        if (scoredCards != null)
            return scoredCards;

        // one pair
        scoredCards = IsPair(cardRankList);
        if (scoredCards != null)
            return scoredCards;

        // leftover: High Card
        List<CardRank> list = [];
        foreach (var c in allCards.Take(5))
        {
            list.Add(c.CardRank);
        }

        HandScore hand = new HandScore(HandCardRank.HighCard, list.ToArray());
        return hand;
    }

    /// <summary>
    /// takes a sorted list of cards (CardRank descending) and organizes them by their suit.
    /// </summary>
    /// <remarks>
    ///this is useful to compare suited cards. Eg for a flush, straight flush or royal flush.
    /// </remarks>
    /// <param name="allCards">the input cards, preferably sorted by CardRank descending</param>
    /// <returns>a dictionary with up to 4 suits. Each suit contains its list of cards, sorted by CardRank descending</returns>
    private static Dictionary<CardSuit, List<Card>> BuildSuitDictionary(IEnumerable<Card> allCards)
    {
        Dictionary<CardSuit, List<Card>> resultDictionary = new Dictionary<CardSuit, List<Card>>();
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
    /// builds a sorted dictionary of card CardRanks, sorted by CardRank descending.
    /// </summary>
    /// <remarks>
    /// this is useful to find pairs
    /// </remarks>
    /// <param name="allCards">all cards to build the Dictionary with, organized y CardRank descending</param>
    /// <returns>a dictionary with the CardRank as key and the count of this CardRank as value</returns>
    private static SortedDictionary<CardRank, ulong> BuildCardRankDictionary(IEnumerable<Card> allCards)
    {
        SortedDictionary<CardRank, ulong> resultDictionary = new SortedDictionary<CardRank, ulong>(
            Comparer<CardRank>.Create((x, y) => y.CompareTo(x)) // sort by CardRank descending
        );
        foreach (Card card in allCards)
        {
            if (resultDictionary.ContainsKey(card.CardRank))
            {
                resultDictionary[card.CardRank]++;
            }
            else
            {
                resultDictionary[card.CardRank] = 1;
            }
        }

        return resultDictionary;
    }

    /// <summary>
    /// checks if the supplied Cards contain a straight flush. Handles Royal Flush as well.
    /// </summary>
    /// <remarks>
    /// for tie resolution, compare the returned hand (CardRank[]) with a simple high card comparison.
    /// </remarks>
    /// <param name="cards">the cards to check</param>
    /// <returns>a HandScore with the straight CardRank as score</returns>
    private static HandScore? IsStraightFlush(Dictionary<CardSuit, List<Card>> cards)
    {
        foreach (List<Card> cardList in cards.Values)
        {
            if (cardList.Count >= 5)
            {
                HandScore? straight = IsStraight(cardList);
                if (straight != null)
                {
                    HandCardRank rank = HandCardRank.StraightFlush;
                    
                    if (straight.Score[0] == CardRank.Ace)
                        rank = HandCardRank.RoyalFlush;
                    
                    HandScore hand = new HandScore(rank, straight.Score);
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
    /// for tie resolution, compare the returned hand (CardRank[]) with a simple high card comparison.
    /// </remarks>
    /// <param name="cards">The grouped card CardRanks from <see cref="BuildCardRankDictionary"/></param>
    /// <returns>The hand which was formed with the pair. Note that the array contains 2 elements, since the first card (pair) is a quadruplet.</returns>
    private static HandScore? IsFourOfAKind(SortedDictionary<CardRank, ulong> cards)
    {
        CardRank? kicker = null;
        CardRank? pairCardRank = null;
        foreach (KeyValuePair<CardRank, ulong> entry in cards)
        {
            if (entry.Value == 4 && pairCardRank == null)
            {
                pairCardRank = entry.Key;
            }
            else if (kicker == null)
            {
                kicker = entry.Key;
            }
            else if (pairCardRank != null) // hand is full
            {
                break;
            }
        }

        if (pairCardRank == null)
            return null;
        HandScore hand = new HandScore(HandCardRank.FourOfAKind, [pairCardRank.Value, kicker!.Value]);
        return hand;
    }


    /// <summary>
    /// Checks if the supplied Cards contain a full house.
    /// </summary>
    /// <remarks>
    /// for tie resolution, compare the returned hand (CardRank[]) with a simple high card comparison.
    /// </remarks>
    /// <param name="cards">The grouped card CardRanks from <see cref="BuildCardRankDictionary"/></param>
    /// <returns>if a full house was found, returns a hand score with the highest three pair CardRank in score[0] and highest two pair CardRank in score[1]</returns>
    private static HandScore? IsFullHouse(SortedDictionary<CardRank, ulong> cards)
    {
        CardRank? highestThreePairCardRank = null;
        CardRank? highestTwoPairCardRank = null;
        foreach (KeyValuePair<CardRank, ulong> stack in cards)
        {
            if (stack.Value == 3)
            {
                if (highestThreePairCardRank == null)
                    highestThreePairCardRank = stack.Key;
                else if (stack.Key > highestThreePairCardRank)
                {
                    if (highestTwoPairCardRank == null || highestThreePairCardRank > highestTwoPairCardRank)
                        highestTwoPairCardRank = highestThreePairCardRank;
                    highestThreePairCardRank = stack.Key;
                }
                else if (highestTwoPairCardRank == null || stack.Key > highestTwoPairCardRank)
                {
                    highestTwoPairCardRank = stack.Key;
                }

            }
            else if (stack.Value == 2 && (highestTwoPairCardRank == null || stack.Key > highestTwoPairCardRank))
            {
                highestTwoPairCardRank = stack.Key;
            }
        }

        if (highestThreePairCardRank != null && highestTwoPairCardRank != null)
        {
            HandScore hand = new HandScore(HandCardRank.FullHouse,
                [highestThreePairCardRank.Value, highestTwoPairCardRank.Value]);
            return hand;
        }

        return null;
    }

    /// <summary>
    /// checks if the supplied cards contain a flush
    /// </summary>
    /// <remarks>
    /// for tie resolution, compare the returned hand (CardRank[]) with a simple high card comparison.
    /// </remarks>
    /// <param name="cards">the list of cards, organized by suits from <see cref="BuildSuitDictionary"/></param>
    /// <returns>a HandScore with the 5 flush cards sorted from highest to lowest CardRank</returns>
    private static HandScore? IsFlush(Dictionary<CardSuit, List<Card>> cards)
    {
        if (cards.Count > 3) // 3 cards and 4 suits 
            return null;
        foreach (List<Card> cardList in cards.Values)
        {
            if (cardList.Count < 5) continue;
            List<CardRank> list = [];
            list.AddRange(cardList.Take(5).Select(c => c.CardRank));

            HandScore hand = new HandScore(HandCardRank.Flush, list.ToArray());
            return hand;
        }

        return null;
    }

    /// <summary>
    /// checks whether the supplied cards contain a street
    /// </summary>
    /// <remarks>
    /// for tie resolution, compare the returned hand (CardRank[]) with a simple high card comparison.
    /// </remarks>
    /// <param name="cards">A list of cards, sorted by CardRank Descending, Ace Highest</param>
    /// <returns>a HandScore with the CardRank of the street if one was found</returns>
    private static HandScore? IsStraight(List<Card> cards)
    {
        if (cards.Count < 5)
            return null;
        ulong connectedCards = 1;
        Card highestConnectedCard = cards[0];
        Card lowestConnectedCard = cards[0];
        ulong wheelPossible = 0;
        if (cards[0].CardRank == CardRank.Ace && cards[^1].CardRank == CardRank.Two)
            wheelPossible++;
        for (int i = 1; i < cards.Count; i++)
        {
            if (cards[i].CardRank == lowestConnectedCard.CardRank) // neutral, card repeated
                continue;
            if (cards[i].CardRank == lowestConnectedCard.CardRank - 1) // found connected card
            {
                lowestConnectedCard = cards[i];
                connectedCards++;
                if (connectedCards == 5)
                {
                    HandScore hand = new HandScore(HandCardRank.Straight, [highestConnectedCard.CardRank]);
                    return hand;
                }
            }
            else // either a repetition or not connected
            {
                if (cards[i].CardRank != lowestConnectedCard.CardRank) // there was a gap
                {
                    // reset straight
                    highestConnectedCard = cards[i];
                    lowestConnectedCard = cards[i];
                    connectedCards = 1;
                    // check if a straight is still possible
                    if (highestConnectedCard.CardRank < CardRank.Five - wheelPossible)
                        return null;
                    ulong maxStraightSpan = (highestConnectedCard.CardRank - cards[^1].CardRank)+1;
                    if (maxStraightSpan < 5 - wheelPossible) // account for possible wheel
                        return null;
                }
                else // was a repetition, chances shCardRank
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
            HandScore hand = new HandScore(HandCardRank.Straight, [highestConnectedCard.CardRank]);
            return hand;
        }

        return null;
    }

    /// <summary>
    /// Checks if there is at least one pair of three in the supplied cards array.
    /// </summary>
    /// <remarks>
    /// for tie resolution, compare the returned hand (CardRank[]) with a simple high card comparison.
    /// </remarks>
    /// <param name="cards">The grouped card CardRanks from <see cref="BuildCardRankDictionary"/></param>
    /// <returns>The hand which was formed with the pair. Note that the array contains 3 elements, since the first card (pair) is a triplet.</returns>
    private static HandScore? IsThreeOfAKind(SortedDictionary<CardRank, ulong> cards)
    {
        List<CardRank> kickers = new();
        CardRank? pairCardRank = null;
        foreach (KeyValuePair<CardRank, ulong> entry in cards)
        {
            if (entry.Value == 3 && pairCardRank == null)
            {
                pairCardRank = entry.Key;
            }
            else if (kickers.Count < 2)
            {
                kickers.Add(entry.Key);
            }
            else if (pairCardRank != null) // hand is full
            {
                break;
            }
        }

        if (pairCardRank == null)
            return null;
        HandScore hand = new HandScore(HandCardRank.ThreeOfAKind, [pairCardRank.Value, kickers[0], kickers[1]]);
        return hand;
    }

    /// <summary>
    /// Checks if there are at least two pairs in the cards.
    /// </summary>
    /// <remarks>
    /// for tie resolution, compare the returned hand (CardRank[]) with a simple high card comparison.
    /// </remarks>
    /// <param name="cards">The grouped card CardRanks from <see cref="BuildCardRankDictionary"/>.</param>
    /// <returns>The hand which was formed with the two pairs. Note that the array contains 3 elements, since the first two cards (pairs) are a duplicate.</returns>
    private static HandScore? IsTwoPair(SortedDictionary<CardRank, ulong> cards)
    {
        CardRank? higherPair = null;
        CardRank? lowerPair = null;
        CardRank? kicker = null;
        foreach (KeyValuePair<CardRank, ulong> entry in cards)
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
        HandScore hand = new HandScore(HandCardRank.TwoPairs, [higherPair.Value, lowerPair.Value, kicker!.Value]);
        return hand;
    }

    /// <summary>
    /// Checks if there is at least one pair in the supplied cards array.
    /// </summary>
    /// <remarks>
    /// for tie resolution, compare the returned hand (CardRank[]) with a simple high card comparison.
    /// </remarks>
    /// <param name="cards">The grouped card CardRanks from <see cref="BuildCardRankDictionary"/></param>
    /// <returns>The hand which was formed with the pair. Note that the array contains 4 elements, since the first card (pair) is a duplicate.</returns>
    private static HandScore? IsPair(SortedDictionary<CardRank, ulong> cards)
    {
        if (cards.Count == 1)
        {
            HandScore pocketCards = new HandScore(HandCardRank.OnePair, [cards.First().Key]);
            return pocketCards;
        }

        List<CardRank> kickers = [];
        CardRank? pairCardRank = null;
        foreach (KeyValuePair<CardRank, ulong> entry in cards)
        {
            if (entry.Value == 2 && pairCardRank == null)
            {
                pairCardRank = entry.Key;
            }
            else if (kickers.Count < 3)
            {
                kickers.Add(entry.Key);
            }
            else if (pairCardRank != null) // hand is full
            {
                break;
            }
        }

        if (pairCardRank == null)
            return null;
        HandScore hand = new HandScore(HandCardRank.OnePair, [pairCardRank.Value, kickers[0], kickers[1], kickers[2]]);
        return hand;
    }
}