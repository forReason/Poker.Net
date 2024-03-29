
using Poker.Net.Logic.GameLogic.Rules;
using Poker.Net.PhysicalObjects.Decks;
using Poker.Net.PhysicalObjects.Tables;


namespace Poker.Net.Logic.GameLogic.BettingRounds;

public partial class BettingRound
{
    /// <summary>
    /// Conducts a betting round, allowing players to call, raise, or fold, and advances the game stage.
    /// </summary>
    /// <remarks>
    /// Iterates through active players for betting decisions and continues until the round is complete.
    /// </remarks>
    public void PerformBettingRound()
    {
        // Select the appropriate starting seat based on the stage of the Game.
        // In pre-flop, the player next to the Big Blind starts the betting round and blinds get collected.
        int startingSeat = _game.GameTable.DealerSeat;
        if (_game.GameTable.CommunityCards.Stage == CommunityCardStage.PreFlop)
        {
            startingSeat = _game.GameTable.BigBlindSeat;
            InitializeNewBettingRound();
        }

        // initialize
        int currentSeat = _game.GameTable.GetNextBettingSeat(startingSeat);
        int lastRaisedSeatId = currentSeat;
        BetsReceived = 0;

        do 
        {
            var (minRaise, maxRaise) = DetermineBettingLimits();

            Seat actionSeat = _game.GameTable.Seats[currentSeat];
            HandlePlayerAction(actionSeat, minRaise, maxRaise, ref lastRaisedSeatId);
            currentSeat = _game.GameTable.GetNextBettingSeat(currentSeat);
        } 
        while (IsBettingRoundContinuing(currentSeat, lastRaisedSeatId) && !_game.CancelGame.IsCancellationRequested);
    }

    /// <summary>
    /// Determines the minimum and maximum raise amounts for the current betting round.
    /// </summary>
    /// <returns>Tuple containing the minimum and maximum raise values.</returns>
    private (ulong minRaise, ulong maxRaise) DetermineBettingLimits()
    {
        ulong minRaise = CallValue * 2;
        ulong maxRaise = ulong.MaxValue;
        if (_game.Rules.Limit == LimitType.PotLimit)
        {
            ulong playerBets = 0;
            foreach (Seat seat in _game.GameTable.Seats)
            {
                playerBets += seat.PendingBets.StackValue;
            }
            maxRaise = _game.GameTable.GetTotalStackValue() + CallValue + playerBets;
        }
        else if(_game.Rules.Limit == LimitType.FixedLimit)
        {
            if (BetsReceived == 4)
            {
                minRaise = 0; maxRaise = 0;
            }
            else if (_game.GameTable.CommunityCards.Stage <= CommunityCardStage.Flop)
            {
                minRaise = _game.CurrentBlindLevel.BigBlind + CallValue;
                maxRaise = minRaise;
            }
            else
            {
                minRaise = _game.CurrentBlindLevel.BigBlind * 2UL + CallValue;
                maxRaise = minRaise;
            }
        }

        return (minRaise, maxRaise);
    }

    /// <summary>
    /// Handles the betting action of a player including calls, raises, and folds.
    /// </summary>
    /// <param name="actionSeat">The seat of the player taking action.</param>
    /// <param name="minRaise">The minimum raise amount.</param>
    /// <param name="maxRaise">The maximum raise amount.</param>
    /// <param name="lastRaisedSeatId">Reference to the ID of the last seat that raised.</param>
    private void HandlePlayerAction(Seat actionSeat, ulong minRaise, ulong maxRaise, ref int lastRaisedSeatId)
    {
        ulong? betValue = actionSeat.CallForPlayerAction(CallValue, minRaise, maxRaise, this._game.Rules.GameTimeStructure, TimeSpan.FromSeconds(20));
        if (betValue == null || betValue == 0)
        {
            actionSeat.Fold();
            return;
        }
        if (betValue >= CallValue || actionSeat.IsAllIn)
        {
            if (betValue > CallValue) // raise
            {
                ulong limitedBetValue = Math.Min(betValue.Value, maxRaise);
                if (limitedBetValue- actionSeat.PendingBets.StackValue > 0)
                {
                    CallValue = limitedBetValue;
                    lastRaisedSeatId = actionSeat.SeatID;
                    BetsReceived++;
                }

                actionSeat.ForceBet(limitedBetValue - actionSeat.PendingBets.StackValue);
            }
            else // call
            {
                actionSeat.ForceBet(CallValue - actionSeat.PendingBets.StackValue);
            }
        }
        // fold the player if he does not want to contribute enough
        else if (!actionSeat.IsAllIn && !actionSeat.IsAllIn)
        {
            actionSeat.Fold();
        }
    }

    /// <summary>
    /// Determines if the betting round should continue based on the current and last raised seats.
    /// </summary>
    /// <param name="currentSeat">The current seat in the betting round.</param>
    /// <param name="lastRaisedSeatId">The ID of the last seat that raised.</param>
    /// <returns>True if the betting round should continue.</returns>
    private bool IsBettingRoundContinuing(int currentSeat, int lastRaisedSeatId)
    {
        return currentSeat != -1 && currentSeat != lastRaisedSeatId;
    }

}