namespace Poker.PhysicalObjects.Cards;

/// <summary>
/// Represents a playing card with a specific rank and suit, commonly used in card games.
/// </summary>
/// <remarks>
/// The <see cref="Card"/> class provides functionalities to compare cards based on their rank and suit.
/// It implements custom comparison operators to facilitate rank-based comparisons, as well as methods 
/// for more specific comparisons (e.g., comparing suits).
/// <para>
/// - The <see cref="CardRank"/> property represents the value or rank of the card.<br/>
/// - The <see cref="CardSuit"/> property represents the suit or category the card belongs to.<br/>
/// </para>
/// <para>
/// The class includes overridden operators for comparing card ranks (such as '==', '!=', '&gt;', '&lt;', '&gt;=', and '&lt;=')
/// and methods for checking exact equality and suit equality. 
/// </para>
/// </remarks>
public class Card : IEquatable<Card>, IComparable<Card>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Card"/> class with a specified rank and suit.
    /// </summary>
    /// <param name="cardRank">The rank of the card.</param>
    /// <param name="suit">The suit of the card.</param>
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
    public CardRank CardRank { get; }
    
    /// <summary>
    /// The Suit defines the Group or Kind a card belongs to. 
    /// </summary>
    public CardSuit Suit { get; }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <remarks>Use operators such as == to compare the CardRanks of two cards</remarks>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj is Card card && Equals(card);
    }
    
    /// <summary>
    /// Determines whether the current card is equal to another card in terms of both rank and suit.
    /// </summary>
    /// <remarks>
    /// This method checks for equality by comparing both the rank and the suit of the cards. It first checks for reference equality, 
    /// which handles cases where both cards are the same instance or both are null. If they are not the same instance, it compares 
    /// their ranks and suits for equality.
    /// <para>
    /// Use this method when you need to determine if two cards are exactly the same in every aspect (both rank and suit).
    /// </para>
    /// </remarks>
    /// <param name="otherCard">The card to compare with the current card.</param>
    /// <returns>True if both cards are equal in rank and suit; otherwise, false.</returns>
    public bool Equals(Card? otherCard)
    {
        if (ReferenceEquals(this, otherCard))
        {
            return true; // Handles both being null
        }
        return this == otherCard && Card.EqualSuits(this, otherCard);
    }

    /// <summary>
    /// Determines whether the suit of the current card is equal to the suit of another card.
    /// </summary>
    /// <remarks>
    /// This method compares the suits of two cards, ignoring their ranks. It is useful when you need to determine 
    /// if two cards belong to the same suit, regardless of their ranks.
    /// <para>
    /// The method internally calls the static <see cref="Card.EqualSuits"/> method for the actual suit comparison.
    /// </para>
    /// </remarks>
    /// <param name="otherCard">The card to compare with the current card for suit equality.</param>
    /// <returns>True if both cards have the same suit; otherwise, false.</returns>
    public bool EqualSuit(Card? otherCard)
    {
        return Card.EqualSuits(this,otherCard);
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
    /// Determines whether the suits of two cards are equal.
    /// </summary>
    /// <remarks>
    /// This operator compares the <see cref="CardSuit"/> of two <see cref="Card"/> instances.
    /// <para>
    /// Special handling for null values:<br/>
    /// - If both cards are null, they are considered to have equal suits.<br/>
    /// - If one card is null and the other is not, they are considered to have different suits.
    /// </para>
    /// </remarks>
    /// <param name="left">The left-hand side <see cref="Card"/> operand.</param>
    /// <param name="right">The right-hand side <see cref="Card"/> operand.</param>
    /// <returns>True if both cards have the same suit or both are null; otherwise, false.</returns>
    public static bool EqualSuits(Card? left, Card? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true; // Handles both being null
        }

        if (left is null || right is null)
        {
            return false; // One is null, the other isn't
        }

        return left.Suit == right.Suit; // Comparing suits
    }

    /// <summary>
    /// Compares two <see cref="Card"/> instances for equality based on their ranks, disregarding suits.
    /// </summary>
    /// <remarks>
    /// This operator defines equality in terms of the rank of the cards. It is used to determine
    /// if two cards have the same rank, which is the primary factor in most poker-related comparisons.
    /// Note that this comparison does not consider the suit of the cards, and two cards of different
    /// suits but the same rank are considered equal.
    /// <para>
    /// Special handling for null values:<br/>
    /// - If both <paramref name="left"/> and <paramref name="right"/> are null, they are considered equal.<br/>
    /// - If either <paramref name="left"/> or <paramref name="right"/> is null (but not both), they are considered not equal.<br/>
    /// </para>
    /// <para>
    /// For checking if two cards are exactly the same (including both rank and suit), use the
    /// <see cref="Equals(Card)"/> method. The <see cref="Equals(Card)"/> method considers both the rank and the suit,
    /// thus providing a stricter form of equality.
    /// </para>
    /// <para>
    /// Usage of this operator allows for convenient rank-based comparison in game logic,
    /// such as evaluating card hands, where the suit of a card may not impact its value.
    /// </para>
    /// </remarks>
    /// <param name="left">The left-hand side <see cref="Card"/> operand.</param>
    /// <param name="right">The right-hand side <see cref="Card"/> operand.</param>
    /// <returns><c>true</c> if both cards have the same rank or are both null; otherwise, <c>false</c>.</returns>
    public static bool operator ==(Card? left, Card? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true; // Handles both being null, as well as the same instance
        }

        if (left is null || right is null)
        {
            return false; // One is null, the other isn't
        }

        return left.CardRank == right.CardRank; // Comparing property values, not Card objects
    }

    /// <summary>
    /// Compares two <see cref="Card"/> instances for inequality based on their ranks, disregarding suits.
    /// </summary>
    /// <remarks>
    /// This operator defines equality in terms of the rank of the cards. It is used to determine
    /// if two cards have different ranks, which is a big factor in most poker-related comparisons.
    /// Note that this comparison does not consider the suit of the cards, and two cards of different
    /// suits but the same rank are considered equal.
    /// <para>
    /// Special handling for null values:<br/>
    /// - If both <paramref name="left"/> and <paramref name="right"/> are null, they are considered equal.<br/>
    /// - If either <paramref name="left"/> or <paramref name="right"/> is null (but not both), they are considered not equal.<br/>
    /// </para>
    /// <para>
    /// For checking if two cards are exactly the same (including both rank and suit), use the
    /// <see cref="Equals(Card)"/> method. The <see cref="Equals(Card)"/> method considers both the rank and the suit,
    /// thus providing a stricter form of equality.
    /// </para>
    /// <para>
    /// Usage of this operator allows for convenient rank-based comparison in game logic,
    /// such as evaluating card hands, where the suit of a card may not impact its value.
    /// </para>
    /// </remarks>
    /// <param name="left">The left-hand side <see cref="Card"/> operand.</param>
    /// <param name="right">The right-hand side <see cref="Card"/> operand.</param>
    /// <returns><c>true</c> if both cards have different ranks or one of the sides is null; otherwise, <c>false</c>.</returns>
    public static bool operator !=(Card? left, Card? right)
    {
        return !(left == right);
    }
    
    /// <summary>
    /// Determines whether the rank of the left-hand card is greater than the rank of the right-hand card.
    /// </summary>
    /// <remarks>
    /// This operator compares two <see cref="Card"/> instances based on their <see cref="CardRank"/>. The comparison disregards the <see cref="CardSuit"/>.
    /// <para>
    /// Special handling for null values:<br/>
    /// - If the left-hand card is null, the result is always false, as a null card is considered to have no rank and cannot be greater than any other card.<br/>
    /// - If both cards are non-null, their ranks are compared.
    /// </para>
    /// </remarks>
    /// <param name="left">The left-hand side <see cref="Card"/> operand.</param>
    /// <param name="right">The right-hand side <see cref="Card"/> operand.</param>
    /// <returns>True if the left-hand card's rank is greater than the right-hand card's rank; otherwise, false.</returns>
    public static bool operator >(Card? left, Card? right)
    {
        if (left is null)
        {
            return false;
        }
        return left.CompareTo(right) > 0;
    }

    
    /// <summary>
    /// Determines whether the rank of the left-hand card is less than the rank of the right-hand card.
    /// </summary>
    /// <remarks>
    /// This operator compares two <see cref="Card"/> instances based on their <see cref="CardRank"/>. The comparison disregards the <see cref="CardSuit"/>.
    /// <para>
    /// Special handling for null values:<br/>
    /// - If the left-hand card is null and the right-hand card is not null, the result is true, as a null card is considered to have no rank.<br/>
    /// - If both cards are non-null, their ranks are compared.
    /// </para>
    /// </remarks>
    /// <param name="left">The left-hand side <see cref="Card"/> operand.</param>
    /// <param name="right">The right-hand side <see cref="Card"/> operand.</param>
    /// <returns>True if the left-hand card's rank is less than the right-hand card's rank; otherwise, false.</returns>
    public static bool operator <(Card? left, Card? right)
    {
        if (left is null)
        {
            return right is not null;
        }
        return left.CompareTo(right) < 0;
    }

    
    /// <summary>
    /// Determines whether the rank of the left-hand card is greater than or equal to the rank of the right-hand card.
    /// </summary>
    /// <remarks>
    /// This operator compares two <see cref="Card"/> instances based on their <see cref="CardRank"/>. The comparison disregards the <see cref="CardSuit"/>.
    /// <para>
    /// Special handling for null values:<br/>
    /// - If the left-hand card is null and the right-hand card is also null, the result is true (null is considered equal to null).<br/>
    /// - If the left-hand card is not null, their ranks are compared.
    /// </para>
    /// </remarks>
    /// <param name="left">The left-hand side <see cref="Card"/> operand.</param>
    /// <param name="right">The right-hand side <see cref="Card"/> operand.</param>
    /// <returns>True if the left-hand card's rank is greater than or equal to the right-hand card's rank; otherwise, false.</returns>
    public static bool operator >=(Card? left, Card? right)
    {
        if (left is not null) return left.CompareTo(right) >= 0;
        return right is null;
    }

    
    public static bool operator <=(Card? left, Card? right)
    {
        if (left is null) return true;
        return left.CompareTo(right) <= 0;
    }
    
    /// <summary>
    /// returns 0 if the Cards are both of the same CardRank<br/>
    /// returns 1 if this card is higher
    /// returns -1 if the other card is higher
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(Card? other)
    {
        if (other is null)
            return 1;
        if (this.CardRank < other.CardRank)
            return -1;
        else if (this.CardRank > other.CardRank)
            return 1;
        return 0;
    }

    /// <summary>
    /// Converts the card to a string representation in the format of "Suit-Rank".
    /// </summary>
    /// <returns>A string representation of the card, showing its suit and rank.</returns>
    public override string ToString()
    {
        return $"[{Suit}-{CardRank}]";
    }
}