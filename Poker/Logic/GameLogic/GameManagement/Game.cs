using Poker.Logic.Blinds;
using Poker.Logic.GameLogic.BettingRounds;
using Poker.Logic.GameLogic.Rules;
using Poker.PhysicalObjects.Chips;
using Poker.PhysicalObjects.Decks;
using Poker.PhysicalObjects.Players;
using Poker.PhysicalObjects.Tables;
using System.Diagnostics;

namespace Poker.Logic.GameLogic.GameManagement;

public partial class Game
{
    public Game(RuleSet gameRules)
    {
        this.Rules = gameRules;
        this.GameTable = new Table(seats: gameRules.MaximumPlayerCount, this);
        this.BettingRound = new BettingRound(this); 
    }

    /// <summary>
    /// specifies the current round the Table is in, increasing by 1 for each full round played (cards dealt)
    /// </summary>
    public ulong Round { get; set; } = 0;

    /// <summary>
    /// the Table associated to the game where the players sit and the action happens
    /// </summary>
    public Table GameTable { get; set; }

    /// <summary>
    /// The Time when the Game was started
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// the current Blind Level for betting
    /// </summary>
    public BlindLevel CurrentBlindLevel { get; set; }

    /// <summary>
    /// The current game Length
    /// </summary>
    /// <remarks>
    /// note that the calculation is automatically beeing switched to Round Based if GameTimeStructure == GameTime.Simulated<br/>
    /// this is useful for Simulations which do not run in Real Time
    /// </remarks>
    public TimeSpan? GameLength
    {
        get
        {
            if (Rules.GameTimeStructure == GameTime.Simulated)
            {
                return Round * Rules.TimePerRound;
            }
            else if (StartTime == null)
            {
                return null;
            }
            else return DateTime.UtcNow - StartTime;
        }
    }

    
    
    /// <summary>
    /// defines the betting structure of the game
    /// </summary>
    public RuleSet Rules { get; set; }

    /// <summary>
    /// The Asynchronous Thread which is running the Game
    /// </summary>
    public Task GameTask { get; private set; }
    /// <summary>
    /// request to cancle the Game
    /// </summary>
    public CancellationToken CancelGame = new CancellationToken();
    /// <summary>
    /// to make sure only one game can be started at a time
    /// </summary>
    private readonly object _GameTaskInitLock = new object();
    /// <summary>
    /// contains the logic for handling the Betting Rounds
    /// </summary>
    public readonly BettingRound BettingRound;

    private async Task Run(CancellationToken cancellationToken)
    {
        if (!Monitor.TryEnter(_GameTaskInitLock))
            return; // Another thread is already executing SeatPlayers
        try
        {
            // wait for game start
            bool IsGameStarted = false;
            while (!cancellationToken.IsCancellationRequested)
            {
                if (!IsGameStarted)
                {
                    IsGameStarted = await CheckStartGame();
                    if (IsGameStarted == true)
                    {
                        Debug.WriteLine("The Start Condition has been met. Game is starting!");
                        break;
                    }
                }
            }
            
            // start the rounds
            StartTime = DateTime.UtcNow;
            DetermineStartingDealer();


            while (!cancellationToken.IsCancellationRequested && GameTable.SeatsWithStakesCount > 2)
            {
                
                Round++;

                // Pre check if a round can be started
                if (!await CheckRoundPrecondition())
                    continue;

                // init round
                this.GameTable.MoveButtons();
                this.GameTable.DealPlayerCards();
                
                // progress through stages
                BettingRoundResult bettingResult;
                do
                {
                    BettingRound.PerformBettingRound();
                    bettingResult = BettingRound.EvaluateBettingRound();
                    if (bettingResult != BettingRoundResult.OpenNextStage)
                    {
                        if (bettingResult == BettingRoundResult.RevealAllCards)
                        {
                            this.GameTable.CommunityCards.RevealAll(this.GameTable.TableDeck);
                        }
                        break;
                    }
                    this.GameTable.CommunityCards.OpenNextStage(this.GameTable.TableDeck);
                } while (this.GameTable.CommunityCards.Stage != CommunityCardStage.River);
                
                // collect bets and create pots from it
                BettingRound.CollectAndSplitBets();
                
                // Evaluate Winner(s) and distribute wins
                foreach (Pot pot in GameTable.CenterPots)
                {
                    Player[] winners = EvaluateWinners(pot);
                    DistributePot(pot, winners);
                }

                // clean up table and everything from the round
                GameTable.CenterPots.Clear();
                SitOutBrokePlayers();
            }
        }
        finally
        {
            Monitor.Exit(_GameTaskInitLock);
            
        }
    }
}