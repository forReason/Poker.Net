using Poker.Players;
using System.Collections.Concurrent;

namespace Poker.Chips
{
    public class Pot
    {
        public ConcurrentDictionary<PokerChip, ulong> Chips = new();
        public ConcurrentDictionary<string, bool> AfflictedPlayers = new();

        public ulong Value
        {
            get
            {
                // Calculating value based on Chips
                ulong totalValue = 0;
                foreach (var chip in Chips)
                {
                    totalValue += (ulong)chip.Key * chip.Value;
                }
                return totalValue;
            }
        }

        // Add chips and mark players as afflicted
        public void AddChips(ConcurrentDictionary<PokerChip, ulong> addChips, ConcurrentDictionary<string, bool> afflictedPlayers)
        {
            foreach (var chipToAdd in addChips)
            {
                Chips.AddOrUpdate(chipToAdd.Key, chipToAdd.Value, (key, oldValue) => oldValue + chipToAdd.Value);
            }
            foreach (var player in afflictedPlayers)
            {
                AfflictedPlayers[player.Key] = true;
            }
        }

        // Add value to the pot, converting it into chips
        public void AddValue(ulong value, Player player)
        {
            // Assuming you have a method to convert value to chips
            var chipsToAdd = ConvertValueToChips(value);

            foreach (var chip in chipsToAdd)
            {
                Chips.AddOrUpdate(chip.Key, chip.Value, (key, oldValue) => oldValue + chip.Value);
            }

            AfflictedPlayers[player.UniqueIdentifier] = true;
        }

        public ConcurrentDictionary<PokerChip, ulong> RemoveValue(ulong value)
        {
            var chipsToRemove = new ConcurrentDictionary<PokerChip, ulong>();
            // Logic to calculate which chips to remove based on the value
            // Remove chips from the pot and add them to chipsToRemove
            return chipsToRemove;
        }



        public ulong RemoveChips(ConcurrentDictionary<PokerChip, ulong> chips)
        {
            ulong totalValueRemoved = 0;
            foreach (var chip in chips)
            {
                if (Chips.TryGetValue(chip.Key, out ulong currentCount) && currentCount >= chip.Value)
                {
                    Chips[chip.Key] -= chip.Value;
                    totalValueRemoved += chip.Key.Value * chip.Value;
                }
                // Handle the case where not enough chips of a type are present
            }
            return totalValueRemoved;
        }


        // Remove specific chips from the pot
        public ulong RemoveChips(ConcurrentDictionary<PokerChip, ulong> chips)
        {
            // Logic to remove specified chips
            // Return the total value of removed chips
        }

        // Move a specified value to another pot
        public void MoveToPot(ulong value, Pot targetPot)
        {
            // Logic to move value from this pot to targetPot
        }

        // Clear the pot and return the total value
        public ulong Clear()
        {
            // Logic to clear the pot and return the total value
        }
    }
}
