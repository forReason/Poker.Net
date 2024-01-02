using Poker.Cards;

namespace Poker.Decks;

public class HandScore
{
    public HandRank Rank { get; set; }
    /// <summary>
    /// the score is used to evaluate the hand in a tie scenario for tie breaking.
    /// the comparer moves from the first element of the array to the last one, checking each card individually
    /// </summary>
    public Rank[] Score { get; set; }
    /// <summary>
    /// score this hand against an enemy hand
    /// </summary>
    /// <param name="enemyHand"></param>
    /// <returns>
    /// 1 if this hand id better<br/>
    /// 0 if both hands are equal<br/>
    /// -1 if the enemies hand is better</returns>
    public int CompareHand (HandScore? enemyHand) {
        if (this.Rank > enemyHand.Rank)
            return 1;
        else if (this.Rank == enemyHand.Rank)
        {
            for (int i = 0; i < this.Score.Length; i++)
            {
                if (this.Score[i] > enemyHand.Score[i])
                    return 1;
                else if (this.Score[i] < enemyHand.Score[i])
                    return -1;
            }

            return 0;
        }

        return -1;
    }
    public static bool operator >(HandScore lhs, HandScore? rhs)
    {
        if (lhs is null)
            return false; // null is not greater than anything
        if (rhs is null)
            return true; // anything is greater than null

        return lhs.CompareHand(rhs) > 0;
    }

    public static bool operator <(HandScore lhs, HandScore? rhs)
    {
        if (lhs is null)
            return rhs is not null; // null is less than anything except null
        if (rhs is null)
            return false; // nothing is less than null

        return lhs.CompareHand(rhs) < 0;
    }

    public static bool operator ==(HandScore lhs, HandScore? rhs)
    {
        if (lhs is null || rhs is null)
            return lhs is null && rhs is null; // true if both are null, otherwise false

        return lhs.CompareHand(rhs) == 0;
    }

    public static bool operator !=(HandScore lhs, HandScore? rhs)
    {
        return !(lhs == rhs);
    }


    // Overriding Equals and GetHashCode is recommended when overloading == and !=
    public override bool Equals(object obj)
    {
        if (obj is HandScore other)
        {
            return this.CompareHand(other) == 0;
        }
        return false;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            // note: add a prime number to reduce hash collisions
            int hash = 17;
            hash = hash * 31 + Rank.GetHashCode();
            foreach (var score in Score)
            {
                hash = hash * 31 + score.GetHashCode();
            }
            return hash;
        }
    }
}
