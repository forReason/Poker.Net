using Poker.Net.PhysicalObjects.Cards;
using Poker.Net.PhysicalObjects.Decks;

namespace Poker.Net.PhysicalObjects.HandScores;
/// <summary>
/// represents the CardRank of a hand an allows to compare twi hands against each other
/// </summary>
public class HandScore(HandCardRank cardRank, CardRank[] score)
{
    /// <summary>
    /// the primary CardRank of the hand
    /// </summary>
    public HandCardRank CardRank { get; } = cardRank;

    /// <summary>
    /// the score is used to evaluate the hand in a tie scenario for tie breaking.
    /// the comparer moves from the first element of the array to the last one, checking each card individually
    /// </summary>
    public CardRank[] Score { get; } = score;

    /// <summary>
    /// score this hand against an enemy hand
    /// </summary>
    /// <param name="enemyHand"></param>
    /// <returns>
    /// 1 if this hand id better<br/>
    /// 0 if both hands are equal<br/>
    /// -1 if the enemies hand is better</returns>
    public int CompareHand(HandScore? enemyHand)
    {
        if (enemyHand == null)
            return 1;
        if (CardRank > enemyHand.CardRank)
            return 1;
        else if (CardRank == enemyHand.CardRank)
        {
            for (int i = 0; i < Score.Length; i++)
            {
                if (Score[i] > enemyHand.Score[i])
                    return 1;
                else if (Score[i] < enemyHand.Score[i])
                    return -1;
            }

            return 0;
        }

        return -1;
    }
    /// <summary>
    /// checks if the hand to the left of the comparison is better than the right one
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >(HandScore? lhs, HandScore? rhs)
    {
        if (lhs is null)
            return false; // null is not greater than anything
        if (rhs is null)
            return true; // anything is greater than null

        return lhs.CompareHand(rhs) > 0;
    }

    /// <summary>
    /// checks if the hand to the left of the comparison is worse than the right one
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <(HandScore? lhs, HandScore? rhs)
    {
        if (lhs is null)
            return rhs is not null; // null is less than anything except null
        if (rhs is null)
            return false; // nothing is less than null

        return lhs.CompareHand(rhs) < 0;
    }

    /// <summary>
    /// checks if both cards are exactly as good as each other
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator ==(HandScore? lhs, HandScore? rhs)
    {
        if (lhs is null || rhs is null)
            return lhs is null && rhs is null; // true if both are null, otherwise false

        return lhs.CompareHand(rhs) == 0;
    }

    /// <summary>
    /// checks if both hands are not exactly as good as the other
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator !=(HandScore? lhs, HandScore? rhs)
    {
        return !(lhs == rhs);
    }


    /// <summary>
    /// Checks if both hands ar exactly as good as the other
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        if (obj is HandScore other)
        {
            return CompareHand(other) == 0;
        }
        return false;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            // note: add a prime number to reduce hash collisions
            int hash = 17;
            hash = hash * 31 + CardRank.GetHashCode();
            foreach (var score in Score)
            {
                hash = hash * 31 + score.GetHashCode();
            }
            return hash;
        }
    }
}
