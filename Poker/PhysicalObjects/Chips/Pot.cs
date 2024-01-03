using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using Poker.Players;
using Poker.Tables;

namespace Poker.Chips
{
    /// <summary>
    /// Represents the pot in a poker game, holding the chips bet by players.
    /// This class provides thread-safe operations to manage and distribute chips.
    /// </summary>
    public class Pot
    {
        /// <summary>
        /// represents all individual chips in the pot
        /// </summary>
        private Dictionary<PokerChip, ulong> _chips;
        /// <summary>
        /// lock to make sure interactions cannot cross each other
        /// </summary>
        private readonly object _lock = new object();
        /// <summary>
        /// all players afiliated with this pot
        /// </summary>
        private HashSet<Player> _players;

        /// <summary>
        /// Initializes a new instance of the Pot class.
        /// </summary>
        public Pot()
        {
            _chips = new Dictionary<PokerChip, ulong>();
            _players = new HashSet<Player>();
        }
        /// <summary>
        /// Gets a read-only snapshot of the chips in the pot.
        /// </summary>
        /// <remarks>This dictionary dos not update when the Pot updates</remarks>
        /// <returns>A read-only dictionary representing the chips currently in the pot.</returns>
        public IReadOnlyDictionary<PokerChip, ulong> GetChips()
        {
            lock (_lock)
            {
                return new ReadOnlyDictionary<PokerChip, ulong>(_chips);
            }
        }
        /// <summary>
        /// returns the current poker chips of the pot in descending individual chipValue
        /// </summary>
        /// <param name="reverse">sort by ascending chip value</param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<PokerChip, ulong>> GetSortedChips(bool reverse = false)
        {
            lock (_lock)
            {
                if (!reverse)
                {
                    if (_sortedChipsDecending == null)
                    {
                        var sortedChips = _chips.OrderByDescending(chip => chip.Key).ToList();
                        _sortedChipsDecending = new ReadOnlyCollection<KeyValuePair<PokerChip, ulong>>(sortedChips);
                    }
                    return _sortedChipsDecending;
                }
                else
                {
                    if(_sortedChipsAscending == null)
                    {
                        var sortedChipsReverse = _chips.OrderBy(chip => chip.Key).ToList();
                        _sortedChipsAscending = new ReadOnlyCollection<KeyValuePair<PokerChip, ulong>>(sortedChipsReverse);
                    }
                    return _sortedChipsAscending;
                }
            }
        }
        private IEnumerable<KeyValuePair<PokerChip, ulong>>? _sortedChipsDecending;
        private IEnumerable<KeyValuePair<PokerChip, ulong>>? _sortedChipsAscending;

        /// <summary>
        /// returns the current instance of attached players to the Pot
        /// </summary>
        public ReadOnlyHashSet<Player> Players => new ReadOnlyHashSet<Player>(_players);
        
        /// <summary>
        /// Adds chips to the pot. This operation is thread-safe.
        /// </summary>
        /// <param name="chips">The chips to add to the pot.</param>
        public void AddChips(IDictionary<PokerChip, ulong> chips, Player player)
        {
            lock (_lock)
            {
                AddChipsInternal(chips, player);
            }
        }
        private void AddChipsInternal(IDictionary<PokerChip, ulong> chips, Player player)
        {
            _players.Add(player);
            _sortedChipsAscending = null;
            _sortedChipsDecending = null;
            foreach (var chip in chips)
            {
                this.PotValue += ((ulong)chip.Key) * chip.Value;
                if (_chips.ContainsKey(chip.Key))
                {
                    _chips[chip.Key] += chip.Value;
                }
                else
                {
                    _chips.Add(chip.Key, chip.Value);
                }
            }
        }
        
        /// <summary>
        /// Removes chips equivalent to the specified value from the pot.
        /// </summary>
        /// <remarks>
        /// automatically exchanges chips with the bank if the colors do not match
        /// </remarks>
        /// <param name="value">The monetary value to remove.</param>
        /// <returns>The actual chips removed from the pot.</returns>
        public IDictionary<PokerChip, ulong> RemoveValue(ulong value)
        {
            lock (_lock)
            {
                return RemoveValueInternal(value);
            }
        }
        private IDictionary<PokerChip, ulong> RemoveValueInternal(ulong value)
        {
            // Step 1: Calculate total value in the pot before removal
            if (value > PotValue)
            {
                throw new InvalidOperationException("Cannot remove more value than is present in the pot.");
            }

            // Step 2: Remove Value as close as possible
            Dictionary<PokerChip, ulong> removedChips = new Dictionary<PokerChip, ulong>();
            ulong remainingValue = value;
            do
            {
                Dictionary<PokerChip, ulong> remainderToRemove = new Dictionary<PokerChip, ulong>();
                // select chips which can be removed
                foreach (KeyValuePair<PokerChip, ulong> stack in GetSortedChips())
                {
                    ulong chipCountToRemove = remainingValue / (ulong)stack.Key;
                    chipCountToRemove = Math.Min(chipCountToRemove, stack.Value);
                    remainderToRemove[stack.Key] = chipCountToRemove;
                    remainingValue -= Bank.ConvertChipsToValue(stack.Key, chipCountToRemove);
                    if (remainingValue <= 0)
                        break;
                }
                // remove the chips
                RemoveChipsInternal(remainderToRemove);
                Bank.MergeStacks(removedChips, remainderToRemove);
                // recolorize the pot
                RecolorizeInternal();
            }
            while (remainingValue > 0);

            return removedChips;
        }

        /// <summary>
        /// returns all chips, then clears the internal values
        /// </summary>
        /// <returns></returns>
        public IDictionary<PokerChip, ulong> RemoveAllChips()
        {
            lock (_lock)
            {
                var chips = RemoveValueInternal(PotValue);
                ClearInternal();
                return chips;
            }
        }

        /// <summary>
        /// moves chips to another pot in thread safe manner
        /// </summary>
        /// <param name="target"></param>
        /// <param name="owner"></param>
        public void MoveAllChips(Pot target, Player owner)
        {
            lock (_lock)
            {
                target.AddChips(RemoveValueInternal(PotValue), owner);
            }
        }

        /// <summary>
        /// moves the desired value from pot to pot
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <param name="owner"></param>
        /// <returns>false if not enough balance</returns>
        public bool MoveValue(Pot target, ulong value, Player owner)
        {
            lock (_lock)
            {
                return MoveValueInternal(target, value, owner);
            }
        }
        private bool MoveValueInternal(Pot target, ulong value, Player owner)
        {
            if (PotValue < value)
                return false;
            target.AddChips(RemoveValueInternal(value), owner);
            return true;
        }
        /// <summary>
        /// moves a requested value to a pot, according to betting rules (if not enough balance, still perform)
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public PerformBetResult PerformBet(Pot target, ulong value, Player owner)
        {
            lock (_lock)
            {
                if (PotValue == 0)
                {
                    return PerformBetResult.PlayerHasNoFunds;
                }
                if (PotValue < value)
                {
                    MoveAllChips(target, owner);
                    return PerformBetResult.AllIn;
                }
                MoveValueInternal(target, value, owner);
                return PerformBetResult.Success;
            }

        }

        /// <summary>
        /// recolorizes the own stack
        /// </summary>
        public void Recolorize()
        {
            lock (_lock)
            {
                RecolorizeInternal();
            }
        }
        
        /// <summary>
        /// this method assumes a lock is in place
        /// </summary>
        private void RecolorizeInternal()
        {
            // Assuming Bank.Recolorize returns IDictionary<PokerChip, ulong>
            var recolorizedChips = Bank.Recolorize(this._chips);

            // Convert IDictionary to Dictionary
            this._chips = new Dictionary<PokerChip, ulong>(recolorizedChips);
            _sortedChipsAscending = null;
            _sortedChipsDecending = null;
        }
        
        /// <summary>
        /// Removes specific chips from the pot. This operation is thread-safe.
        /// </summary>
        /// <param name="chips">The specific chips to remove from the pot.</param>
        /// <returns>The total monetary value of the chips removed.</returns>
        public ulong RemoveChips(IDictionary<PokerChip, ulong> chips)
        {
            lock (_lock)
            {
                return RemoveChipsInternal(chips);
            }
        }
        private ulong RemoveChipsInternal(IDictionary<PokerChip, ulong> chips)
        {
            _sortedChipsAscending = null;
            _sortedChipsDecending = null;
            ulong totalRemovedValue = 0;
            foreach (var chip in chips)
            {
                if (_chips.TryGetValue(chip.Key, out ulong currentAmount) && currentAmount >= chip.Value)
                {
                    _chips[chip.Key] = currentAmount - chip.Value;
                    ulong chipStackValue = chip.Value * (ulong)chip.Key;
                    PotValue -= chipStackValue;
                    totalRemovedValue += chipStackValue;
                }
                else
                {
                    throw new InvalidOperationException("you tried to remove chips which are not in the Pot!");
                }
            }
            return totalRemovedValue;
        }
        

        /// <returns>The total value of the chips in the pot.</returns>
        public ulong RecalculatePotValue()
        {
            lock (_lock)
            {
                return RecalculatePotValueInternal();
            }
        }
        private ulong RecalculatePotValueInternal()
        {
            PotValue = Bank.ConvertChipsToValue(_chips);
            return PotValue;
        }
        /// <summary>
        /// represents the current Value of the pot as ulong
        /// </summary>
        public ulong PotValue { get; private set; } = 0;

        /// <summary>
        /// Clears all chips from the pot. This operation is thread-safe.
        /// </summary>
        /// <returns>The total value of the chips that were in the pot.</returns>
        public ulong Clear()
        {
            lock (_lock)
            {
                return ClearInternal();
            }
        }
        private ulong ClearInternal()
        {
            ulong totalValue = PotValue;
            PotValue = 0;
            _chips.Clear();
            _players.Clear();
            _sortedChipsAscending = null;
            _sortedChipsDecending = null;
            return totalValue;
        }
    }
}
