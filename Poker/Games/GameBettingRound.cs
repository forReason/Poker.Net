using Poker.Chips;
using Poker.Decks;
using Poker.Tables;

namespace Poker.Games;


public partial class Game
{
    /// <summary>
    /// this function sets the bets for BigBlind and SmallBlind player and Ante for everyone
    /// </summary>
    public void SetBlindsAndAnte()
    {
        this.CurrentBlindLevel = this.BettingStructure.GetApropriateBlindLevel(this.GameLength);
        // collect blinds
        this.GameTable.Seats[this.GameTable.SmallBlindSeat].ForceBet(this.CurrentBlindLevel.SmallBlind);
        this.GameTable.Seats[this.GameTable.BigBlindSeat].ForceBet(this.CurrentBlindLevel.BigBlind);
        CallValue = this.CurrentBlindLevel.BigBlind;
        // set ante
        if (this.CurrentBlindLevel.Ante == 0)
            return;
        foreach(Seat seat in this.GameTable.Seats)
        {
            if (seat.IsParticipatingGame())
            {
                seat.ForceBet(this.CurrentBlindLevel.Ante);
            }
        }
        CallValue += this.CurrentBlindLevel.Ante;
    }
    /// <summary>
    /// the current bet call value for the round which has to be matched.
    /// </summary>
    public ulong CallValue { get; private set; } = 0;
    /// <summary>
    /// this variable counts how many Bets/raises have been executed during this betting round. 
    /// It is used to limit the maximum of raises in limit poker
    /// </summary>
    public ulong BetsReceived { get; private set; } = 0;
    /// <summary>
    /// Performs one full betting round for one game stage (pre-flop, flop, etc.), 
    /// where all players have the chance to call, raise, or fold. 
    /// Once the betting round is over, the game transitions to the next stage, 
    /// either revealing more cards or finally revealing player hands.
    /// </summary>
    /// <param name="stage">
    /// Defines the game stage. In pre-flop, collects blinds, antes, and sets the starting player
    /// to the one next to the Big Blind.
    /// </param>
    /// <remarks>
    /// The method iterates through all active players, allowing them to make their betting decisions.
    /// The round continues until it either comes back to the last player who raised, 
    /// or there are no more players who can act due to folding or going all-in.
    /// </remarks>
    /// <returns>true if all cards are to </returns>
    public async Task PerformBettingRound()
    {
        // Select the appropriate starting seat based on the stage of the game.
        // In pre-flop, the player next to the Big Blind starts the betting round and blinds get collected.
        int startingSeat = this.GameTable.DealerSeat;
        if (this.GameTable.CommunityCards.Stage == CommunityCardStage.PreFlop)
        {
            startingSeat = this.GameTable.BigBlindSeat;
            SetBlindsAndAnte();
        }

        // initialize
        int currentSeat = this.GameTable.GetNextBettingSeat(startingSeat);
        int lastRaisedSeatId = currentSeat;
        BetsReceived = 0;
        // Loop through the betting rounds, allowing each player to take action.
        // Players can choose to call, raise, or fold based on their strategy and hand.
        do 
        {
            ulong minRaise = CallValue * 2;
            ulong maxRaise = ulong.MaxValue;
            if (this.BettingStructure.Limit == Blinds.LimitType.PotLimit)
            {
                ulong playerBets = 0;
                foreach (Seat seat in this.GameTable.Seats)
                {
                    playerBets += seat.PendingBets.PotValue;
                }
                maxRaise = this.GameTable.GetTotalPotValue() + CallValue + playerBets;
            }
            else if(this.BettingStructure.Limit == Blinds.LimitType.FixedLimit)
            {
                if (BetsReceived == 4)
                {
                    minRaise = 0; maxRaise = 0;
                }
                else if (this.GameTable.CommunityCards.Stage <= CommunityCardStage.Flop)
                {
                    minRaise = this.CurrentBlindLevel.BigBlind + CallValue;
                    maxRaise = minRaise;
                }
                else
                {
                    minRaise = this.CurrentBlindLevel.BigBlind * 2 + CallValue;
                    maxRaise = minRaise;
                }
            }

            Seat actionSeat = this.GameTable.Seats[currentSeat];
            actionSeat.Player.CallForAction(CallValue, minRaise, maxRaise);
            // evaluate playerAction
            ulong betValue = actionSeat.PendingBets.PotValue + actionSeat.UncalledPendingBets.PotValue;
            if (betValue >= CallValue || actionSeat.IsAllInCall)
            {
                if (betValue > CallValue) // raise
                {
                    ulong limitedBetValue = Math.Min(betValue, maxRaise);
                    if (limitedBetValue- actionSeat.PendingBets.PotValue > 0)
                    {
                        CallValue = limitedBetValue;
                        lastRaisedSeatId = currentSeat;
                        BetsReceived++;
                    }
                    actionSeat.UncalledPendingBets.MoveValue(actionSeat.PendingBets, limitedBetValue - actionSeat.PendingBets.PotValue, actionSeat.Player);
                }
                else // call
                {
                    actionSeat.UncalledPendingBets.MoveValue(actionSeat.PendingBets, CallValue - actionSeat.PendingBets.PotValue, actionSeat.Player);
                }
                // move leftover chips back to the player stack
                actionSeat.UncalledPendingBets.MoveAllChips(actionSeat.Stack, actionSeat.Player);
            }
            // fold the player if he does not want to contribute enough
            else if (!actionSeat.IsAllIn && !actionSeat.IsAllInCall)
            {
                actionSeat.Fold();
            }
            // select the betting seat for the next player to bet
            // this is also beeing used to determine if the betting round/stage is over
            currentSeat = this.GameTable.GetNextBettingSeat(currentSeat);
        }
        // The betting round continues until one of the following conditions is met:
        // 1. All players have had a chance to act and the play returns to the last player who raised.
        //    This ensures each player has responded to the most recent bet.
        // 2. There are no more players who can act because they have all folded or are all-in.
        //    In this case, 'currentSeat' will be -1, signaling the end of the round.
        while (
            currentSeat != -1 && // there are no more players who can take action (either all are all in, or folded, or only 1 player left in the money)
            currentSeat != lastRaisedSeatId // the round has progressed without any raise
        );
    }

    /// <summary>
    /// Collects all bets from the players and splits the pot if necessary.
    /// This method handles scenarios where side pots need to be created due to players going all-in with different bet amounts.
    /// </summary>
    public void CollectAndSplitBets()
    {
        ulong min, max;
        do
        {
            // Initialize minimum and maximum values to track the smallest and largest bets in this iteration.
            min = ulong.MaxValue;
            max = 0;

            // Iterate over all seats to find the minimum and maximum bet amounts among players who are still in the game.
            foreach (var seat in this.GameTable.Seats)
            {
                if (!seat.IsFold)
                {
                    ulong potValue = seat.PendingBets.PotValue;
                    if (potValue > max)
                        max = potValue;
                    if (potValue < min)
                        min = potValue;
                }
            }

            // If max is zero, it means no active bets are present, and the loop can be exited.
            if (max == 0)
                break;

            // Create a new pot to handle the side pot for this iteration.
            Pot newCenterPot = new();
        
            // Move the minimum bet amount from each active player's pending bets to the new side pot.
            foreach (var seat in this.GameTable.Seats)
            {
                if (seat.PendingBets.PotValue > 0)
                    seat.PendingBets.MoveValue(newCenterPot, min, seat.Player);
            }

            // Add the new side pot to the collection of center pots.
            this.GameTable.CenterPots.Add(newCenterPot);

            // Continue the loop until the minimum and maximum bet amounts are equal, indicating no further side pots are needed.
        } while (min != max);
    }


    public BettingRoundResult EvaluateBettingRound()
    {
        if (this.GameTable.PlayersInBettingRoundCount <= 1)
            return BettingRoundResult.LastManStanding;
        if (this.GameTable.CheckAllPlayersAllIn())
            return BettingRoundResult.RevealAllCards;
        return BettingRoundResult.OpenNextStage;
    }
}