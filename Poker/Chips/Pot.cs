using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using Poker.Players;

namespace Poker.Chips
{
    /// <summary>
    /// Represents the pot in a poker game, holding the chips bet by players.
    /// This class provides thread-safe operations to manage and distribute chips.
    /// </summary>
    public class Pot
    {
        private ConcurrentDictionary<PokerChip, ulong> _Chips;
        private readonly object _lock = new object();

        private HashSet<Player> _Players;

        /// <summary>
        /// Initializes a new instance of the Pot class.
        /// </summary>
        public Pot()
        {
            _Chips = new ConcurrentDictionary<PokerChip, ulong>();
            _Players = new HashSet<Player>();
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
                return new ReadOnlyDictionary<PokerChip, ulong>(_Chips);
            }
        }
        public IEnumerable<KeyValuePair<PokerChip, ulong>> GetSortedChips()
        {
            // Sorting the chips from high to low denomination
            return _Chips.OrderByDescending(chip => chip.Key);
        }

        /// <summary>
        /// returns the current instance of attached players to the Pot
        /// </summary>
        public ReadOnlyHashSet<Player> Players => new ReadOnlyHashSet<Player>(_Players);
        
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
            _Players.Add(player);
            foreach (var chip in chips)
            {
                _Chips.AddOrUpdate(chip.Key, chip.Value, (key, oldValue) => oldValue + chip.Value);
            }
        }
        
        /// <summary>
        /// Removes chips equivalent to the specified value from the pot.
        /// </summary>
        /// <param name="value">The monetary value to remove.</param>
        /// <returns>The actual chips removed from the pot.</returns>
        public IDictionary<PokerChip, ulong> RemoveValue(ulong value)
        {
            lock (_lock)
            {
                // Step 1: Calculate total value in the pot before removal
                ulong totalValueBeforeRemoval = Bank.ConvertChipsToValue(_Chips);
                if (value > totalValueBeforeRemoval)
                {
                    throw new InvalidOperationException("Cannot remove more value than is present in the pot.");
                }

                // Step 2: Remove Value as close as possible
                Dictionary<PokerChip, ulong> chipsToRemove = new Dictionary<PokerChip, ulong>();
                ulong remainingValue = value;
                foreach (KeyValuePair<PokerChip, ulong> stack in GetSortedChips())
                {
                    ulong chipCountToRemove = remainingValue / (ulong)stack.Key;
                    chipCountToRemove = Math.Min(chipCountToRemove, stack.Value);
                    chipsToRemove[stack.Key] = chipCountToRemove;
                    remainingValue -= Bank.ConvertChipsToValue(stack.Key, chipCountToRemove);
                    if (remainingValue <= 0)
                        break;
                }

                RemoveChipsInternal(chipsToRemove);

                // step 3: recolorize Deck to continue filling
                if (remainingValue > 0)
                {
                    Dictionary<PokerChip, ulong> remainderToRemove = new Dictionary<PokerChip, ulong>();
                    RecolorizeInternal();
                    foreach (KeyValuePair<PokerChip, ulong> stack in GetSortedChips())
                    {
                        ulong chipCountToRemove = remainingValue / (ulong)stack.Key;
                        chipCountToRemove = Math.Min(chipCountToRemove, stack.Value);
                        remainderToRemove[stack.Key] = chipCountToRemove;
                        remainingValue -= Bank.ConvertChipsToValue(stack.Key, chipCountToRemove);
                        if (remainingValue <= 0)
                            break;
                    }
                    RemoveChipsInternal(remainderToRemove);
                    Bank.MergeStacks(chipsToRemove, remainderToRemove);
                }
                
                return chipsToRemove;
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
            var recolorizedChips = Bank.Recolorize(this._Chips);

            // Convert IDictionary to ConcurrentDictionary
            this._Chips = new ConcurrentDictionary<PokerChip, ulong>(recolorizedChips);
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
            ulong totalRemovedValue = 0;
            foreach (var chip in chips)
            {
                if (_Chips.TryGetValue(chip.Key, out ulong currentAmount) && currentAmount >= chip.Value)
                {
                    _Chips[chip.Key] = currentAmount - chip.Value;
                    totalRemovedValue += chip.Value * (ulong)chip.Key;
                }
                else
                {
                    throw new Exception("you tried to remove chips which are not in the Pot!");
                }
            }
            return totalRemovedValue;
        }
        

        /// <returns>The total value of the chips in the pot.</returns>
        public ulong GetValue()
        {
            lock (_lock)
            {
                return GetValueInternal();
            }
        }
        private ulong GetValueInternal()
        {
            return Bank.ConvertChipsToValue(_Chips);
        }

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
            ulong totalValue = GetValueInternal();
            _Chips.Clear();
            _Players.Clear();
            return totalValue;
        }
    }
}
