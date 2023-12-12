namespace Poker.Cards;

/// <summary>
/// this struct defines a Card. A Card consists of a Rank and a suit.
///<br/>
/// - The suit is determined to find correct pairs.<br/>
/// - The Rank defines the value of a card
/// </summary>
public struct Card
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
}