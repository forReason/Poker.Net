namespace Poker.Decks
{
    /// <summary>
    /// represents the stage of the CommunityCards
    /// </summary>
    public enum CommunityCardStage
    {
        /// <summary>
        /// no cards are in the Community Cards
        /// </summary>
        PreFlop = 0,
        /// <summary>
        /// 3 Cards are open in the Community Cards
        /// </summary>
        Flop = 1,
        /// <summary>
        /// 4 Cards are open in the Community Cards
        /// </summary>
        Turn = 2,
        /// <summary>
        /// All 5 Cards are open in the Community Cards
        /// </summary>
        River = 3
    }
}
