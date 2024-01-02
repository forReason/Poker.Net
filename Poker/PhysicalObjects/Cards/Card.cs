namespace Poker.Cards;

/// <summary>
/// this struct defines a Card. A Card consists of a Rank and a suit.
///<br/>
/// - The suit is determined to find correct pairs.<br/>
/// - The Rank defines the value of a card
/// </summary>
public struct Card : IEquatable<Card>, IComparable<Card>
{
    /// <summary>
    /// Creates a new card with a given rank and suit
    /// </summary>
    /// <param name="rank">The Rank of the Card</param>
    /// <param name="suit">The color group of the Card</param>
    public Card(Rank rank, Suit suit)
    {
        Rank = rank;
        Suit = suit;
    }
    
    /// <summary>
    /// The Rank defines the value of a card. the higher, the better.
    /// </summary>
    /// <remarks>
    /// Example: A king is of rank 13 and thus higher than a Jack with rank 11
    /// </remarks>
    public Rank Rank { get; set; }
    
    /// <summary>
    /// The Suit defines the Group or Kind a card belongs to. 
    /// </summary>
    public Suit Suit { get; set; }
    
    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
    public override bool Equals(object obj)
    {
        return obj is Card card && Equals(card);
    }

    /// <summary>
    /// Determines whether the specified Card is equal to the current Card.
    /// </summary>
    /// <param name="other">The Card to compare with the current Card.</param>
    /// <returns>true if the specified Card is equal to the current Card; otherwise, false.</returns>
    public bool Equals(Card other)
    {
        return Rank == other.Rank && Suit == other.Suit;
    }

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Rank, Suit);
    }

    /// <summary>
    /// Determines whether the Ranks of two Cards are equal.
    /// </summary>
    public static bool operator ==(Card left, Card right)
    {
        return (left.Rank == right.Rank);
    }

    /// <summary>
    /// Determines whether the Ranks of two cards are not equal.
    /// </summary>
    public static bool operator !=(Card left, Card right)
    {
        return (left.Rank != right.Rank);
    }
    
    /// <summary>
    /// Determines if the Card on the Left has a higher Rank than the Card on the Right
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator >(Card left, Card right)
    {
        return left.CompareTo(right) > 0;
    }

    //// <summary>
    /// Determines if the Card on the Left has a lower Rank than the Card on the Right
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator <(Card left, Card right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>
    /// Determines if the Card on the left is of higher than or equal to rank
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator >=(Card left, Card right)
    {
        return left.CompareTo(right) >= 0;
    }

    /// <summary>
    /// Determines if the Card on the left is of lower than or equal to rank
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator <=(Card left, Card right)
    {
        return left.CompareTo(right) <= 0;
    }
    
    /// <summary>
    /// returns 0 if the Cards are both of the same rank<br/>
    /// returns 1 if this card is higher
    /// returns -1 if the other card is higher
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(Card other)
    {
        if (this.Rank < other.Rank)
            return -1;
        else if (this.Rank > other.Rank)
            return 1;
        return 0;
    }

    public override string ToString()
    {
        return $"[{Suit}-{Rank}]";
    }
}