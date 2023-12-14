namespace Poker.Blinds
{
    internal class BlindStructure
    {
        /// <summary>
        /// the Time Between each Level increase
        /// </summary>
        public TimeSpan LevelTime { get; set; } = TimeSpan.FromMinutes(15);
        /// <summary>
        /// the time each round ROUGHLY takes (for simulation purposes)
        /// </summary>
        public TimeSpan TimePerRound { get; set; } = TimeSpan.FromMinutes(2.5);
        /// <summary>
        /// The amount of rounds equaling to one level (for simulation purposes)
        /// </summary>
        public int RoundsPerLevel { get; set; } = 6;
        ///
        public ulong BuyIn { get; set; }
    }
}
