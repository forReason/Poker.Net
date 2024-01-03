namespace Poker.PhysicalObjects.Cards;

/// <summary>
/// Individual cards are CardRanked, from highest to lowest: A, K, Q, J, 10, 9, 8, 7, 6, 5, 4, 3 and 2.<br/>
/// Aces have the highest CardRank under ace-to-five high or six-to-ace low rules, or under high rules as part of a five-high straight or straight flush.
/// </summary>
public enum CardRank : ulong
{
    /// <summary>
    /// the highest card in value
    /// </summary>
    /// <remarks>Can be used as a 1 at the beginning of a street: A, 2, 3, 4, 5 or at the end: 10, J, Q, K, A but not for rollover streets </remarks>
    Ace = 14,
    /// <summary>
    /// The lowest card in the game
    /// </summary>
    Two = 2,
    /// <summary>
    /// the second lowest card in the game
    /// </summary>
    Three = 3,
    /// <summary>
    /// the tird lowest card in the game
    /// </summary>
    Four = 4,
    /// <summary>
    /// a low card
    /// </summary>
    Five = 5,
    /// <summary>
    /// a low card
    /// </summary>
    Six = 6,
    /// <summary>
    /// a low card
    /// </summary>
    Seven = 7,
    /// <summary>
    /// a low card
    /// </summary>
    Eight = 8,
    /// <summary>
    /// a mediocre card
    /// </summary>
    Nine = 9,
    /// <summary>
    /// a high card
    /// </summary>
    Ten = 10,
    /// <summary>
    /// a high card
    /// </summary>
    Jack = 11,
    /// <summary>
    /// Third highest card 
    /// </summary>
    Queen = 12,
    /// <summary>
    /// Second highest card
    /// </summary>
    King = 13
}