using Poker.Chips;

namespace Poker.Blinds
{
    public class BlindStructure
    {
        /// <summary>
        /// the Time Between each Level increase
        /// </summary>
        public TimeSpan LevelTime { get; set; } = TimeSpan.FromMinutes(20);
        /// <summary>
        /// the time each round ROUGHLY takes (for simulation purposes)
        /// </summary>
        public TimeSpan TimePerRound { get; set; } = TimeSpan.FromMinutes(2.5);
        /// <summary>
        /// The amount of rounds equaling to one level (for simulation purposes)
        /// </summary>
        public int RoundsPerLevel { get; set; } = 6;
        /// <summary>
        /// The target time which the game should hold for.
        /// </summary>
        /// <remarks>Usually, the game ends very soon when SmallBlind >= BuyIn</remarks>
        public TimeSpan TargetTotalTimeEstimate { get; set; } = TimeSpan.FromHours(4);
        /// <summary>
        /// the Buyin amount to play on the Table
        /// </summary>
        public ulong BuyIn { get; set; }
        /// <summary>
        /// the ratio of the buyin to the minimum bet. The smaller the ratio, the longer the game will take.
        /// </summary>
        /// <remarks>1:25 seems like a reasonable starting point</remarks>
        public BlindToBuyInRatio BlindRatio { get; set; } = BlindToBuyInRatio.OneToTwentyFive;

        public int MinimumChipSize { get; set; } = -2; // 2 chip stages below small blind

        /// <summary>
        /// Whether to use ante or not
        /// </summary>
        /// <remarks>
        /// ante defines that all players must place a Bet pre-Flop (similar to the small blind)<br/>
        /// it is usually introduced after a few Rounds
        /// </remarks>
        public AnteToBigBlindRatio Ante { get; set; } = AnteToBigBlindRatio.None;

        // all chip sizes should be divided by 100
        public bool Micro { get; set; } = false;
        private PokerChip GetClosestChip(double value)
        {
            PokerChip closestChip = PokerChip.White;
            double closestDifference = double.MaxValue;

            foreach (PokerChip chip in Enum.GetValues(typeof(PokerChip)))
            {
                double chipValue = (double)chip;
                if (Micro)
                    chipValue /= 100;
                double difference = QuickStatistics.Net.Math_NS.Difference.Get(chipValue, value);

                if (difference == 0)
                {
                    return chip;
                }
                else if (difference < closestDifference)
                {
                    closestDifference = difference;
                    closestChip = chip;
                }
                else
                {
                    break;
                }
            }
            return closestChip;
        }
        /// <summary>
        /// Calculates the blind structure for the game.
        /// </summary>
        /// <returns>A list of blind levels and their corresponding values.</returns>
        public List<BlindLevel> CalculateBlindStructure()
        {
            // calculate the count of Levels
            ulong levelCount = (ulong)(TargetTotalTimeEstimate / LevelTime);
            // we need to get from start bet to the BuyIn Amount in the calculated amount of Levels
            // A linear approach does not suffice.
            // a non Linear approach poses the problem that you might get odd numbers
            // this is why an iterative approach is taken
            double smallBlindCalculated = BuyIn / (double)BlindRatio;
            if (smallBlindCalculated < 1)
                Micro = true;
            double smallBlind = (double)GetClosestChip(smallBlindCalculated);
            if (Micro)
                smallBlind /= 100;
            BlindLevel[] levels = new BlindLevel[levelCount];
            double finalValue = 0;
            double epochCount = 0;
            double stepSize = 0;
            double levelsPerEpoch = 0;
            double anteRatio = 1.0d/ (double)Ante;
            while (finalValue < BuyIn)
            {
                while (finalValue < BuyIn)
                {
                    stepSize++;
                    epochCount = 0;
                    while (epochCount <= levelCount && finalValue < BuyIn)
                    {
                        epochCount++;
                        levelsPerEpoch = (ulong)(levelCount / epochCount);
                        double currentValue = 0;
                        double currentSmallBlind = smallBlind * stepSize;
                        for (ulong epoch = 1; epoch <= epochCount; epoch++)
                        {
                            currentValue += levelsPerEpoch * currentSmallBlind * epoch;
                        }

                        if (Ante != AnteToBigBlindRatio.None && epochCount > 1)
                        {
                            currentValue += currentSmallBlind * 2 * anteRatio;
                        }
                        finalValue = currentValue;
                    }
                }
            }
            // compile
            List<BlindLevel> blindStructure = new List<BlindLevel>();
            double currentBlind = 0;
            for (double epoch = 1; epoch <= epochCount; epoch++)
            {
                for (double step = 1; step <= levelsPerEpoch; step++)
                {
                    double roundSmallBlind = currentBlind + smallBlind * stepSize * epoch * step;
                    double roundBigBlind = roundSmallBlind * 2;
                    double roundAnte = 0;
                    if (Ante != AnteToBigBlindRatio.None && epoch > 1)
                    {
                        roundAnte = roundBigBlind * anteRatio;
                    }
                    blindStructure.Add(
                        new BlindLevel((int)epoch, 
                            roundSmallBlind, 
                            roundBigBlind , 
                            roundAnte));
                }

                currentBlind = blindStructure[blindStructure.Count - 1].SmallBlind;
            }
            for (double epoch = 1; epoch <= 5; epoch++)
            {
                    double roundSmallBlind = currentBlind + smallBlind * stepSize * epoch * levelsPerEpoch;
                    double roundBigBlind = roundSmallBlind * 2;
                    double roundAnte = 0;
                    if (Ante != AnteToBigBlindRatio.None)
                    {
                        roundAnte = roundBigBlind * anteRatio;
                    }
                    blindStructure.Add(
                        new BlindLevel((int)epoch, 
                            roundSmallBlind, 
                            roundBigBlind , 
                            roundAnte));

                currentBlind = blindStructure[blindStructure.Count - 1].SmallBlind;
            }
            return blindStructure;
            
        }
        
    }
}
