namespace Poker.Net.PhysicalObjects.Decks;

/// <summary>
/// defines the primary score of a hand which can be compared to each other
/// </summary>
/// <remarks>
/// within each handCardRank, the hands have to be compared further
/// </remarks>
public enum HandCardRank
{
    /// <summary>
    /// the lowest CardRank out there
    /// </summary>
    /// <remarks>
    /// win-rate: Better Fold<br/>
    /// probability: 50.11%
    /// </remarks>
    HighCard,
    /// <summary>
    /// a low hand
    /// </summary>
    /// <remarks>
    /// win-rate: Lower Win.rate<br/>
    /// probability: 42.25%
    /// </remarks>
    OnePair,
    /// <summary>
    /// a medium hand
    /// </summary>
    /// <remarks>
    /// win-rate: Moderate Win.rate<br/>
    /// probability: 4.75%
    /// </remarks>
    TwoPairs,
    /// <summary>
    /// a medium-high hand
    /// </summary>
    /// <remarks>
    /// win-rate: Decent win rate<br/>
    /// probability: 2.11%
    /// </remarks>
    ThreeOfAKind,
    /// <summary>
    /// the sixth highest CardRank
    /// </summary>
    /// <remarks>
    /// win-rate: Solid hand<br/>
    /// probability: 0.3925%
    /// </remarks>
    Straight,
    /// <summary>
    /// the fifth highest CardRank
    /// </summary>
    /// <remarks>
    /// win-rate: Strong hand with a good win rate<br/>
    /// probability: 0.197%
    /// </remarks>
    Flush,
    /// <summary>
    /// the fourth highest CardRank
    /// </summary>
    /// <remarks>
    /// win-rate: High win rate<br/>
    /// probability: 0.144%
    /// </remarks>
    FullHouse,
    /// <summary>
    /// the third highest CardRank
    /// </summary>
    /// <remarks>
    /// win-rate: Very high win rate, rarely beaten<br/>
    /// probability: 0.024%
    /// </remarks>
    FourOfAKind,
    /// <summary>
    /// the second highest CardRank
    /// </summary>
    /// <remarks>
    /// win-rate: Extremely high<br/>
    /// probability: 0.00139%
    /// </remarks>
    StraightFlush,
    /// <summary>
    /// the highest CardRank
    /// </summary>
    /// <remarks>
    /// win-rate: 100%<br/>
    /// probability: 0.000154%
    /// </remarks>
    RoyalFlush
}