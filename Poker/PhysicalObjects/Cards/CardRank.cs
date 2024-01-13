namespace Poker.PhysicalObjects.Cards;

/// <summary>
/// Individual cards are CardRanked, from highest to lowest: A, K, Q, J, 10, 9, 8, 7, 6, 5, 4, 3 and 2.<br/>
/// Aces have the highest CardRank under ace-to-five high or six-to-ace low rules, or under high rules as part of a five-high straight or straight flush.
/// </summary>
public enum CardRank : byte
{
    
    /// <summary>
    /// The lowest card in the game
    /// </summary>
    Two,
    /// <summary>
    /// the second lowest card in the game
    /// </summary>
    Three,
    /// <summary>
    /// the third lowest card in the game
    /// </summary>
    Four,
    /// <summary>
    /// a low card
    /// </summary>
    Five,
    /// <summary>
    /// a low card
    /// </summary>
    Six,
    /// <summary>
    /// a low card
    /// </summary>
    Seven,
    /// <summary>
    /// a low card
    /// </summary>
    Eight,
    /// <summary>
    /// a mediocre card
    /// </summary>
    Nine,
    /// <summary>
    /// a high card
    /// </summary>
    Ten,
    /// <summary>
    /// a high card
    /// </summary>
    Jack,
    /// <summary>
    /// Third highest card 
    /// </summary>
    Queen,
    /// <summary>
    /// Second highest card
    /// </summary>
    King,
    /// <summary>
    /// the highest card in value
    /// </summary>
    /// <remarks>Can be used as a 1 at the beginning of a street: A, 2, 3, 4, 5 or at the end: 10, J, Q, K, A but not for rollover streets </remarks>
    Ace
}