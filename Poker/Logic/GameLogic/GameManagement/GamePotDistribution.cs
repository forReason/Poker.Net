using Poker.Logic.Blinds;
using Poker.PhysicalObjects.Chips;
using Poker.PhysicalObjects.Players;

namespace Poker.Logic.GameLogic.GameManagement;

public partial class Game
{
    /// <summary>
    /// Distributes the specified pot among the given winners after calculating and deducting the rake.
    /// </summary>
    /// <param name="pot">The pot to be distributed.</param>
    /// <param name="winners">An array of players who have won the pot.</param>
    /// <remarks>
    /// This function is called for each side pot individually. It first calculates the rake based on the current betting structure and game length. 
    /// The rake is then deducted from the total pot value, and the remainder is evenly distributed among the winners. 
    /// Any leftover due to division rounding, along with the rake, is added to the house's bank.
    /// </remarks>
    public void DistributePot(Pot pot, Player[] winners)
    {
        // calculate rake and win per player
        ulong rake = 0;
        if (this.Rules.RakeStructure != null)
        {
            BlindLevel level = this.Rules.GetApropriateBlindLevel(this.GameLength);
            rake = this.Rules.RakeStructure.CalculateRake(pot, level.SmallBlind, this.Rules.Micro);

        }
        ulong leftover = pot.StackValue - rake;
        ulong winPerPlayer = leftover / (ulong)winners.Length;
        // split pots
        foreach (Player player in winners)
        {
            pot.MoveValue(player.Seat.Stack, winPerPlayer, player);
        }
        // rake + leftover goes to the house
        House.Casino.AddPlayerBank(pot.Clear());
    }
}