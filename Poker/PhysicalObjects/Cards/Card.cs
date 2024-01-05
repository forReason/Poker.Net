namespace Poker.PhysicalObjects.Cards;

/// <summary>
/// this struct defines a Card. A Card consists of a CardRank and a suit.
///<br/>
/// - The suit is determined to find correct pairs.<br/>
/// - The CardRank defines the value of a card
/// </summary>
public struct Card : IEquatable<Card>, IComparable<Card>
{
    /// <summary>
    /// Creates a new card with a given CardRank and suit
    /// </summary>
    /// <param name="cardRank">The CardRank of the Card</param>
    /// <param name="suit">The color group of the Card</param>
    public Card(CardRank cardRank, CardSuit suit)
    {
        this.CardRank = cardRank;
        this.Suit = suit;
    }
    
    /// <summary>
    /// The CardRank defines the value of a card. the higher, the better.
    /// </summary>
    /// <remarks>
    /// Example: A king is of CardRank 13 and thus higher than a Jack with CardRank 11
    /// </remarks>
    public CardRank CardRank { get; private set; }
    
    /// <summary>
    /// The Suit defines the Group or Kind a card belongs to. 
    /// </summary>
    public CardSuit Suit { get; private set; }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <remarks>Use operators such as == to compare the CardRanks of two cards</remarks>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        return obj is Card card && Equals(card);
    }

    /// <summary>
    /// Determines whether the specified Card is equal to the current Card.
    /// </summary>
    /// <remarks>Use operators such as == to compare the CardRanks of two cards</remarks>
    /// <param name="other">The Card to compare with the current Card.</param>
    /// <returns>true if the specified Card is equal to the curre/nt Card; otherwise, false.</returns>
    public bool Equals(Card other)
    {
        return CardRank == other.CardRank && Suit == other.Suit;
    }

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(CardRank, Suit);
    }

    /// <summary>
    /// Determines whether the CardRanks of two Cards are equal.
    /// </summary>
    /// <remarks>use <see cref="Equals(Card)"/> in order to check if two cards are exactly equal (CardRank & suit)</remarks>
    public static bool operator ==(Card left, Card right)
    {
        return (left.CardRank == right.CardRank);
    }

    /// <summary>
    /// Determines whether the CardRanks of two cards are not equal.
    /// </summary>
    /// <remarks>use <see cref="Equals(Card)"/> in order to check if two cards are exactly equal (CardRank & suit)</remarks>
    public static bool operator !=(Card left, Card right)
    {
        return (left.CardRank != right.CardRank);
    }
    
    /// <summary>
    /// Determines if the Card on the Left has a higher CardRank than the Card on the Right
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator >(Card left, Card right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <summary>
    /// Determines if the Card on the Left has a lower CardRank than the Card on the Right
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator <(Card left, Card right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>
    /// Determines if the Card on the left is of higher than or equal to CardRank
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator >=(Card left, Card right)
    {
        return left.CompareTo(right) >= 0;
    }

    /// <summary>
    /// Determines if the Card on the left is of lower than or equal to CardRank
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator <=(Card left, Card right)
    {
        return left.CompareTo(right) <= 0;
    }
    
    /// <summary>
    /// returns 0 if the Cards are both of the same CardRank<br/>
    /// returns 1 if this card is higher
    /// returns -1 if the other card is higher
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(Card other)
    {
        if (this.CardRank < other.CardRank)
            return -1;
        else if (this.CardRank > other.CardRank)
            return 1;
        return 0;
    }

    /// <summary>
    /// converts the card to string in the format of "Suit-Rank"
    /// </summary>
    /// <returns>the string representation of the card</returns>
    public override string ToString()
    {
        return $"[{Suit}-{CardRank}]";
    }
}