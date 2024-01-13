namespace Poker.PhysicalObjects.Cards;

/// <summary>
/// Suits are not CardRanked, so hands that differ by suit alone are of equal CardRank.
/// </summary>
public enum CardSuit : byte
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
