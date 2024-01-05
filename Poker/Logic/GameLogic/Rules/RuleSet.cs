
using Poker.Logic.Blinds;
using Poker.Logic.Fees;
using Poker.PhysicalObjects.Chips;

namespace Poker.Logic.GameLogic.Rules
{
    /// <summary>
    /// Defines the betting structure for a poker game, including rules, blinds, antes, and rake.
    /// </summary>
    public class RuleSet
    {
        // TODO: test correct implementation
        /// <summary>
        /// Initializes a new instance of the <see cref="RuleSet"/> class.
        /// </summary>
        /// <param name="ruleSet">The set of rules governing the game (e.g., Cash, Tournament).</param>
        /// <param name="buyIn">The amount required to buy into the game.</param>
        /// <param name="blindtoByinRatio">The ratio of the blind to the buy-in amount.</param>
        /// <param name="maxBuyInRatio">The maximum ratio for the buy-in amount.</param>
        /// <param name="minPlayerCount">The minimum amount of players for the game / round to continue. Defaults to 2.</param>
        /// <param name="maxPlayerCount">The maximum amount of players (seats at the Table) defaults to 6.</param>
        /// <param name="ante">The ante amount in relation to the big blind.</param>
        /// <param name="limit">The type of limit applied to the game (e.g., No Limit, Pot Limit).</param>
        /// <param name="capBlindRatio">The ratio of the cap to the small blind.</param>
        /// <param name="timeMode">Whether we are in a simulated or live environment</param>
        /// <param name="levelTime">The duration of each level in the game.</param>
        /// <param name="targetTotalTime">The estimated total duration of the game.</param>
        /// <param name="registrationGracePeriod">The grace period for late registration in tournament games.</param>
        /// <param name="rakeStructure">The structure for calculating the rake.</param>
        /// <param name="autoCalculateRakeStructure">Indicates whether to automatically calculate the rake structure.</param>
        /// <remarks>
        /// This class provides the necessary settings to define the betting dynamics of a poker game, including blinds, antes, and rake calculations.
        /// </remarks>
        public RuleSet(
            GameMode ruleSet,
            ulong buyIn,
            BlindToBuyInRatio blindToByinRatio,
            decimal maxBuyInRatio = 2,
            ulong maxBuyInCount = 0,
            TimeSpan? latestAllowedBuyIn = null,
            uint maxPlayerCount = 6,
            uint minPlayerCount = 2,
            AnteToBigBlindRatio ante = AnteToBigBlindRatio.None,
            LimitType limit = LimitType.NoLimit,
            double capBlindRatio = 0,
            GameTime timeMode = GameTime.RealTime,
            TimeSpan? levelTime = null,
            TimeSpan? targetTotalTime = null,
            TimeSpan? registrationGracePeriod = null,
            RakeStructure? rakeStructure = null,
            bool autoCalculateRakeStructure = false)
        {
            MinimumPlayerCount = minPlayerCount;
            MaximumPlayerCount = maxPlayerCount;
            GameMode = ruleSet;
            GameTimeStructure = timeMode;
            BuyIn = buyIn;
            BlindRatio = blindToByinRatio;
            MaxBuyinRatio = maxBuyInRatio;
            MaxBuyInCount = maxBuyInCount;
            LatestAllowedBuyIn = latestAllowedBuyIn;
            Ante = ante;
            Limit = limit;
            CapXSmallBlind = capBlindRatio;
            if (levelTime != null)
                LevelTime = levelTime.Value;
            if (targetTotalTime != null)
                TargetTotalTimeEstimate = targetTotalTime.Value;
            if (registrationGracePeriod != null)
                RegistrationGracePeriod = registrationGracePeriod.Value;
            RakeStructure = rakeStructure;
            if (autoCalculateRakeStructure)
                RakeStructure = new RakeStructure(limit);
            CalculateBlindStructure();
        }

        /// <summary>
        /// defines the rules under which to operate, eg Cash or tournament
        /// </summary>
        public GameMode GameMode { get; set; }

        /// <summary>
        /// in tournament games defines the latest buyin after Game stard
        /// </summary>
        public TimeSpan RegistrationGracePeriod { get; set; } = TimeSpan.FromMinutes(0);

        /// <summary>
        /// the Time Between each Level increase
        /// </summary>
        public TimeSpan LevelTime { get; private set; } = TimeSpan.FromMinutes(20);

        /// <summary>
        /// defines how the gametime is beeing Calculated.
        /// </summary>
        /// <remarks>
        /// with real time set, the Engine uses the current system TimeStanmps<br/>
        /// when set to simulated, the Game time is beeing Calculated from the rounds played
        /// </remarks>
        public GameTime GameTimeStructure { get; set; }

        /// <summary>
        /// the amount of players when the game starts
        /// </summary>
        public uint MinimumPlayerCount { get; private set; } = 2;
        /// <summary>
        /// the maximum Amount of players which can Sit on the Table
        /// </summary>
        public uint MaximumPlayerCount { get; private set; } = 6;

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
        public LimitType Limit { get; private set; }

        /// <summary>
        /// The target time which the game should hold for.
        /// </summary>
        /// <remarks>Usually, the game ends very soon when SmallBlind >= BuyIn</remarks>
        public TimeSpan TargetTotalTimeEstimate { get; private set; } = TimeSpan.FromHours(4);

        /// <summary>
        /// the Buy-in amount to play on the Table
        /// </summary>
        public decimal BuyIn { get; private set; }

        /// <summary>
        /// the maximum buyin
        /// </summary>
        public decimal MaxBuyIn => BuyIn * MaxBuyinRatio;

        /// <summary>
        /// the ratio of the buy-in to the minimum bet. The smaller the ratio, the longer the game will take.
        /// </summary>
        /// <remarks>1:25 seems like a reasonable starting point</remarks>
        public BlindToBuyInRatio BlindRatio { get; private set; }

        /// <summary>
        /// defines how much larger the maximum buyin can be. for tournaments, this is usually the same as buyin, so a ratio of 1;
        /// </summary>
        public decimal MaxBuyinRatio { get; private set; } = 2;

        /// <summary>
        /// the maximum count of buyins allowed. 0 means infinity
        /// </summary>
        public ulong MaxBuyInCount { get; private set; } = 0;

        public TimeSpan? LatestAllowedBuyIn { get; private set; }

        /// <summary>
        /// defines the minimum chip size (defaults to 2 units under small blind. EG SB = 50, min chip size = 10
        /// </summary>
        public int MinimumChipSize { get; set; } = -2; // 2 chip stages below small blind

        /// <summary>
        /// defines the fees for the house
        /// </summary>
        public RakeStructure? RakeStructure { get; set; }

        /// <summary>
        /// Whether to use ante or not
        /// </summary>
        /// <remarks>
        /// ante defines that all players must place a Bet pre-Flop (similar to the small blind)<br/>
        /// it is usually introduced after a few Rounds
        /// </remarks>
        public AnteToBigBlindRatio Ante { get; private set; }

        /// <summary>
        /// all chip sizes should be divided by 100
        /// </summary>
        public bool Micro { get; private set; } = false;
        private PokerChip GetClosestChip(decimal value)
        {
            PokerChip closestChip = PokerChip.White;
            decimal closestDifference = decimal.MaxValue;

            foreach (PokerChip chip in Enum.GetValues(typeof(PokerChip)))
            {
                decimal chipValue = (decimal)chip;
                decimal difference = QuickStatistics.Net.Math_NS.Difference.Get(chipValue, value);

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
            if (GameMode == GameMode.Cash)
                return BlindStructure[0];
            int epoch = (int)(round / RoundsPerLevel);
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
        public BlindLevel GetApropriateBlindLevel(TimeSpan? GameDuration)
        {
            if (!GameDuration.HasValue)
                return BlindStructure[0];
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
            // CalculateBlinds
            decimal smallBlindCalculated = BuyIn / (ulong)BlindRatio;
            if (smallBlindCalculated < 1)
            {
                Micro = true;
                smallBlindCalculated *= 100;
            }
            ulong smallBlind = (ulong)GetClosestChip(smallBlindCalculated);
            decimal anteRatio = 0;
            if (Ante!= AnteToBigBlindRatio.None)
                anteRatio = 1.0m / (decimal)Ante;
            if (GameMode == GameMode.Cash)
            {
                ulong bigBlind = smallBlind * 2;
                ulong ante = (ulong)(bigBlind * anteRatio);
                List<BlindLevel> level = [new BlindLevel(1, smallBlind, bigBlind, ante)];
                BlindStructure = level;
                return;
            }
            ulong buyInChipValue = Bank.ConvertMicroToMacro(BuyIn);

            // create blind levels
            // Array to store the blind levels for the game.
            var blindLevels = new BlindLevel[levelCount];

            // Variable to track the total value accumulated in the blind structure.
            ulong totalBlindValue = 0;

            // Variables to determine the distribution of blinds across the game levels.
            ulong distributionCycleCount = 0;
            ulong blindIncrementFactor = 0;
            ulong levelsPerDistributionCycle = 0;

            // Loop to calculate the blind structure until the total value reaches or exceeds the BuyIn amount.
            while (totalBlindValue < buyInChipValue)
            {
                blindIncrementFactor++;
                distributionCycleCount = 0;
                // Inner loop to adjust the distribution of blinds across levels.
                while (distributionCycleCount <= levelCount && totalBlindValue < buyInChipValue)
                {
                    distributionCycleCount++;
                    levelsPerDistributionCycle = levelCount / distributionCycleCount;

                    ulong currentCycleValue = 0;
                    ulong currentCycleSmallBlind = smallBlind * blindIncrementFactor;

                    // Calculate the total value for the current distribution cycle.
                    for (ulong cycle = 1; cycle <= distributionCycleCount; cycle++)
                    {
                        currentCycleValue += levelsPerDistributionCycle * currentCycleSmallBlind * cycle;
                    }

                    // Adjust for ante if applicable.
                    if (Ante != AnteToBigBlindRatio.None && distributionCycleCount > 1)
                    {
                        currentCycleValue += (ulong)(currentCycleSmallBlind * 2 * anteRatio);
                    }

                    totalBlindValue = currentCycleValue;
                }
            }


            // compile output
            List<BlindLevel> blindStructure = new List<BlindLevel>();
            ulong currentBlind = 0;
            for (ulong epoch = 1; epoch <= distributionCycleCount; epoch++)
            {
                for (ulong step = 1; step <= levelsPerDistributionCycle; step++)
                {
                    ulong roundSmallBlind = currentBlind + smallBlind * blindIncrementFactor * epoch * step;
                    ulong roundBigBlind = roundSmallBlind * 2;
                    ulong roundAnte = 0;
                    if (Ante != AnteToBigBlindRatio.None && epoch > 1)
                    {
                        roundAnte = (ulong)(roundBigBlind * anteRatio);
                    }

                    blindStructure.Add(
                        new BlindLevel((int)epoch,
                            roundSmallBlind,
                            roundBigBlind,
                            roundAnte));
                }

                currentBlind = blindStructure[^1].SmallBlind;
            }

            for (ulong epoch = 1; epoch <= 5; epoch++)
            {
                ulong roundSmallBlind = currentBlind + smallBlind * blindIncrementFactor * epoch * levelsPerDistributionCycle;
                ulong roundBigBlind = roundSmallBlind * 2;
                ulong roundAnte = 0;
                if (Ante != AnteToBigBlindRatio.None)
                {
                    roundAnte = (ulong)(roundBigBlind * anteRatio);
                }

                blindStructure.Add(
                    new BlindLevel((int)epoch,
                        roundSmallBlind,
                        roundBigBlind,
                        roundAnte));

                currentBlind = blindStructure[^1].SmallBlind;
            }

            BlindStructure = blindStructure;
        }
    }
}
