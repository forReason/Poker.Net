
using Poker.PhysicalObjects.Chips;

namespace Poker.Logic.GameLogic.BettingRounds;

public partial class BettingRound
{
    /// <summary>
    /// Collects all bets from the players and splits the pot if necessary.
    /// This method handles scenarios where side pots need to be created due to players going all-in with different bet amounts.
    /// </summary>
    public void CollectAndSplitBets()
    {
        ulong min, max;
        do
        {
            (min, max) = FindMinMaxBets();
            if (max == 0) break;

            Pot newCenterPot = CreateAndDistributeSidePot(min);
            _game.GameTable.CenterPots.Add(newCenterPot);
        } while (min != max);
    }

    /// <summary>
    /// finds the minimum and maximum bet on the Table
    /// </summary>
    /// <remarks>
    /// if min == max, this means that al players have bet the same amount. No side pot is required <br/>
    /// if min smaller than max, this means that one player is all in and a side pot has to be created.<br/>
    /// if min == ulong.MaxValue || max == 0, this means that there are no more remaining bets to process
    /// </remarks>
    /// <returns></returns>
    private (ulong min, ulong max) FindMinMaxBets()
    {
        ulong min = ulong.MaxValue;
        ulong max = 0;

        foreach (var seat in _game.GameTable.Seats)
            if (!seat.IsFold)
            {
                ulong potValue = seat.PendingBets.PotValue;
                if (potValue > max) max = potValue;
                if (potValue < min) min = potValue;
            }

        return (min, max);
    }

    /// <summary>
    /// takes the bet amount and creates a (side) pot from it
    /// </summary>
    /// <param name="minBet">the amount which should be moved to the pot from each bet</param>
    /// <returns></returns>
    private Pot CreateAndDistributeSidePot(ulong minBet)
    {
        Pot newCenterPot = new();
        foreach (var seat in _game.GameTable.Seats)
            if (seat.PendingBets.PotValue > 0)
                // Check if player is not null before accessing
                seat.PendingBets.MoveValue(newCenterPot, minBet, seat.Player ??
                                                                 // Move the bet to the center pot even if the player is null
                                                                 seat.PendingBets.Players.First());
        return newCenterPot;
    }

}