using Poker.Chips;
using Poker.Tables;

namespace Poker.Blinds
{
    public class BettingStructure
    {
        public BettingStructure(
            TableRuleSet ruleSet, 
            ulong buyIn, 
            BlindToBuyInRatio blindtoByinRatio, 
            ulong maxBuyInRatio,
            AnteToBigBlindRatio ante = AnteToBigBlindRatio.None,
            bool limit = false,
            double capBlindRatio = 0,
            TimeSpan? levelTime = null,
            TimeSpan? targetTotalTime = null,
            TimeSpan? registrationGracePeriod = null)
        {
            this.RuleSet = ruleSet;
            this.BuyIn = buyIn;
            this.BlindRatio = blindtoByinRatio;
            this.MaxBuyinRatio = maxBuyInRatio;
            this.Ante = ante;
            this.Limit = false;
            this.CapXSmallBlind = capBlindRatio;
            if (levelTime != null)
                this.LevelTime = levelTime.Value;
            if (targetTotalTime != null)
                this.TargetTotalTimeEstimate = targetTotalTime.Value;
            if (registrationGracePeriod != null)
                this.RegistrationGracePeriod = registrationGracePeriod.Value;
            CalculateBlindStructure();
        }

        /// <summary>
        /// defines the rules under which to operate, eg Cash or tournament
        /// </summary>
        public TableRuleSet RuleSet { get; set; }

        /// <summary>
        /// in tournament games defines the latest buyin after Game stard
        /// </summary>
        public TimeSpan RegistrationGracePeriod { get; set; } = TimeSpan.FromMinutes(0);

        /// <summary>
        /// the Time Between each Level increase
        /// </summary>
        public TimeSpan LevelTime { get; private set; } = TimeSpan.FromMinutes(20);

        /// <summary>
        /// the time each round ROUGHLY takes (for simulation purposes)
        /// </summary>
        public TimeSpan TimePerRound { get; set; } = TimeSpan.FromMinutes(2.5);

        /// <summary>
        /// The amount of rounds equaling to one level (for simulation purposes)
        /// </summary>
        public ulong RoundsPerLevel => (ulong)(LevelTime / TimePerRound);

        /// <summary>
        /// cap the bet of each player to this factor of the small blind.
        /// </summary>
        /// <remarks>
        /// - smaller or equal 0: no cap <br/>
        /// - the maximum amount to bet per turn is equal to CapXSmallBlind * SmallBlind
        /// </remarks>
        public double CapXSmallBlind { get; private set; }

        /// <summary>
        /// defines Limit poker (can only raise the same amount than BigBlind)
        /// </summary>
        public bool Limit { get; private set; }

        /// <summary>
        /// The target time which the game should hold for.
        /// </summary>
        /// <remarks>Usually, the game ends very soon when SmallBlind >= BuyIn</remarks>
        public TimeSpan TargetTotalTimeEstimate { get; private set; } = TimeSpan.FromHours(4);
        
        /// <summary>
        /// the Buy-in amount to play on the Table
        /// </summary>
        public ulong BuyIn { get; private set; }

        /// <summary>
        /// the maximum buyin
        /// </summary>
        public ulong MaxBuyIn => BuyIn * MaxBuyinRatio;
        
        /// <summary>
        /// the ratio of the buy-in to the minimum bet. The smaller the ratio, the longer the game will take.
        /// </summary>
        /// <remarks>1:25 seems like a reasonable starting point</remarks>
        public BlindToBuyInRatio BlindRatio { get; private set; }

        /// <summary>
        /// defines how much larger the maxumum buyin can be. for tournaments, this is usually the same as buin, so a ratio of 1;
        /// </summary>
        public ulong MaxBuyinRatio { get; private set; } = 2;

        /// <summary>
        /// defines the minimum chip size (defaults to 2 units under small blind. EG SB = 50, min chip size = 10
        /// </summary>
        public int MinimumChipSize { get; set; } = -2; // 2 chip stages below small blind

        /// <summary>
        /// Whether to use ante or not
        /// </summary>
        /// <remarks>
        /// ante defines that all players must place a Bet pre-Flop (similar to the small blind)<br/>
        /// it is usually introduced after a few Rounds
        /// </remarks>
        public AnteToBigBlindRatio Ante { get; private set; }

        // all chip sizes should be divided by 100
        public bool Micro { get; private set; } = false;
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

        public List<BlindLevel> BlindStructure { get; private set; }

        /// <summary>
        /// Calculates the correct Blind structure according to the Current Game Round
        /// </summary>
        /// <remarks>this is good for simulations</remarks>
        /// <param name="round"></param>
        /// <returns></returns>
        public BlindLevel GetApropriateBlindLevel(ulong round)
        {
            if (this.RuleSet == TableRuleSet.Cash)
                return BlindStructure[0];
            int epoch = (int) (round / this.RoundsPerLevel);
            if (epoch >= BlindStructure.Count)
                return BlindStructure[^1];
            return BlindStructure[epoch];
        }
        /// <summary>
        /// Calculates the correct Blind structure according to the Current Game Duration
        /// </summary>
        /// <remarks>
        /// the correct Blind Structure also contains the Level/epoch information
        /// </remarks>
        /// <param name="GameDuration"></param>
        /// <returns></returns>
        public BlindLevel GetApropriateBlindLevel(TimeSpan GameDuration)
        {
            int epoch = (int)(GameDuration / TimePerRound);
            if (epoch >= BlindStructure.Count)
                return BlindStructure[^1];
            return BlindStructure[epoch];
        }

        /// <summary>
        /// Calculates the blind structure for the game.
        /// </summary>
        /// <returns>A list of blind levels and their corresponding values.</returns>
        public void CalculateBlindStructure()
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
            double anteRatio = 1.0d / (double)Ante;
            if (this.RuleSet == TableRuleSet.Cash)
            {
                double bigBlind = smallBlind * 2;
                double ante = bigBlind * anteRatio;
                List<BlindLevel> level = [new BlindLevel(1, smallBlind, bigBlind, ante)];
                this.BlindStructure = level;
                return;
            }

            var levels = new BlindLevel[levelCount];
            double finalValue = 0;
            double epochCount = 0;
            double stepSize = 0;
            double levelsPerEpoch = 0;

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
                            roundBigBlind,
                            roundAnte));
                }

                currentBlind = blindStructure[^1].SmallBlind;
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
                        roundBigBlind,
                        roundAnte));

                currentBlind = blindStructure[^1].SmallBlind;
            }

            this.BlindStructure = blindStructure;
        }
    }
}
