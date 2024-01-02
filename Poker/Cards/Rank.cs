namespace Poker.Cards;

/// <summary>
/// Individual cards are ranked, from highest to lowest: A, K, Q, J, 10, 9, 8, 7, 6, 5, 4, 3 and 2.<br/>
/// Aces have the highest rank under ace-to-five high or six-to-ace low rules, or under high rules as part of a five-high straight or straight flush.
/// </summary>
public enum Rank : ulong
{
    Ace = 14,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Jack = 11,
    Queen = 12,
    King = 13
}