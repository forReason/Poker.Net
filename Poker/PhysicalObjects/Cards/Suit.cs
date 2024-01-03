namespace Poker.Cards;

/// <summary>
/// Suits are not ranked, so hands that differ by suit alone are of equal rank.
/// </summary>
public enum Suit
{
    /// <summary>
    /// a black leaf
    /// </summary>
    Spades,
    /// <summary>
    /// a red heart
    /// </summary>
    Hearts,
    /// <summary>
    /// a red Rhombus
    /// </summary>
    Diamonds,
    /// <summary>
    /// a black cross
    /// </summary>
    Clubs
}