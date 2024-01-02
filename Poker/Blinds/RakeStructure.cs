using Poker.Chips;

namespace Poker.Blinds
{
    public class RakeStructure
    {
        public decimal PercentageRake { get; set; }
        private SortedList<decimal, RakeLevel> _rakeLevels;

        public RakeStructure(List<RakeLevel> rakeLevels)
        {
            this._rakeLevels = new SortedList<decimal, RakeLevel>(rakeLevels.Count);
            foreach (var rakeLevel in rakeLevels)
            {
                this._rakeLevels.Add(rakeLevel.SmallBlind, rakeLevel);
            }
        }
        /// <summary>
        /// sets the rake structure according to Pokerstars (2023)
        /// </summary>
        /// <param name="limit"></param>
        public RakeStructure (LimitType limit)
        {
            List<RakeLevel> levels = new List<RakeLevel>();
            if (limit == LimitType.NoLimit || limit == LimitType.PotLimit)
            {
                levels =
                    [
                        new RakeLevel(0.01m, 3.5m / 100, [new(2, 0.3m)]),
                        new RakeLevel(0.02m, 4.15m / 100, [new(4, 0.5m), new(5, 1m)]),
                        new RakeLevel(0.08m, 4.5m / 100, [new(2, 0.5m), new(4, 1m), new(5, 1.5m)]),
                        new RakeLevel(0.1m, 4.5m / 100, [new(2, 0.5m), new(4, 1m), new(5, 2m)]),
                        new RakeLevel(0.25m, 5m / 100, [new(4, 0.75m), new(5, 2m)]),
                        new RakeLevel(0.5m, 5m / 100, [new(4, 1m), new(5, 2.5m)]),
                        new RakeLevel(1m, 5m / 100, [new(4, 1.25m), new(5, 2.75m)]),
                        new RakeLevel(2m, 5m / 100, [new(4, 1.5m), new(5, 3m)]),
                        new RakeLevel(3m, 5m / 100, [new(4, 1.5m), new(5, 3.5m)]),
                        new RakeLevel(5m, 4.5m / 100, [new(4, 1.5m), new(5, 3m)]),
                        new RakeLevel(10m, 4.5m / 100, [new(4, 1.75m), new(5, 3m)]),
                        new RakeLevel(25m, 4.5m / 100, [new(4, 2.25m), new(5, 3m)]),
                        new RakeLevel(50m, 4.5m / 100, [new(2, 2.5m), new(4, 3m), new(5, 5m)]),
                        new RakeLevel(100m, 4.5m / 100, [new(2, 3m), new(4, 5m)])
                    ];
            }
            else
            {
                levels =
                    [
                        new RakeLevel(0.02m, 4.5m / 100, [new(2, 0.01m)]),
                        new RakeLevel(0.04m, 4.5m / 100, [new(2, 0.04m)]),
                        new RakeLevel(0.1m, 4.5m / 100, [new(2, 0.1m)]),
                        new RakeLevel(0.25m, 5m / 100, [new(2, 0.16m)]),
                        new RakeLevel(0.5m, 5m / 100, [new(2, 0.4m)]),
                        new RakeLevel(1m, 3m / 100, [new(2, 0.5m), new(4, 0.7m), new(5, 0.8m)]),
                        new RakeLevel(2m, 2.5m / 100, [new(2, 0.5m), new(4, 0.7m), new(5, 1.25m)]),
                        new RakeLevel(3m, 2.5m / 100, [new(2, 0.5m), new(4, 2m), new(5, 3m)]),
                        new RakeLevel(10m, 2.25m / 100, [new(2, 0.5m), new(4, 2m), new(5, 3m)]),
                        new RakeLevel(15m, 2m / 100, [new(2, 1m), new(4, 2m), new(5, 3m)]),
                        new RakeLevel(20m, 1m / 100, [new(2, 1m), new(4, 2m), new(5, 3m)]),
                        new RakeLevel(150m, 1m / 100, [new(2, 2m), new(4, 5m)]),
                    ];
            }
            _rakeLevels = new SortedList<decimal, RakeLevel>(levels.Count);
            foreach (var rakeLevel in levels)
            {
                this._rakeLevels.Add(rakeLevel.SmallBlind, rakeLevel);
            }
        }

        public ulong CalculateRake(Pot pot, ulong smallBlind, bool micro)
        {

            RakeLevel level = GetRakeLevelBasedOnSmallBlind(smallBlind, micro);
            decimal rake = pot.PotValue * level.PercentageRake;
            decimal cap = level.GetCapBasedOnPlayerCount(pot.Players.Count);
            if (micro)
            {
                cap *= 100;
            }

            return (ulong)Math.Min(rake, cap);
        }
        public RakeLevel GetRakeLevelBasedOnSmallBlind(ulong smallBlind, bool micro)
        {
            decimal smallBlindDecimal = (decimal)smallBlind;
            if (micro)
                smallBlindDecimal /= 100;
            foreach (var rakeLevel in _rakeLevels)
            {
                if (smallBlindDecimal <= rakeLevel.Key)
                {
                    return rakeLevel.Value;
                }
            }
            return _rakeLevels.Last().Value; // return highest level if blind larger than
        }
        
    }

    public class RakeLevel
    {
        public decimal SmallBlind;
        public decimal PercentageRake;
        private SortedList<int, decimal> _rakeCaps;
        public RakeLevel(decimal smallBlind, decimal percentageRake, RakeCap[] rakeCaps)
        {
            this.SmallBlind = smallBlind;
            this.PercentageRake = percentageRake;
            this._rakeCaps = new SortedList<int, decimal>(rakeCaps.Length);
            foreach (var rakeCap in rakeCaps)
            {
                this._rakeCaps.Add(rakeCap.PlayerCount, rakeCap.Cap);
            }
        }
        public decimal GetCapBasedOnPlayerCount(int playerCount)
        {
            foreach (var rakeCap in _rakeCaps)
            {
                if (playerCount <= rakeCap.Key)
                {
                    return rakeCap.Value;
                }
            }
            return _rakeCaps.Last().Value; // Or a suitable default if playerCount exceeds all caps
        }
    }
    public struct RakeCap
    {
        public int PlayerCount;
        public decimal Cap;

        public RakeCap(int playerCount, decimal cap)
        {
            PlayerCount = playerCount;
            Cap = cap;
        }
    }
}
