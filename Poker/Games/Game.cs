using System.Diagnostics;
using Poker.Blinds;
using Poker.Tables;

namespace Poker.Games;

public partial class Game
{
    public Game(
        BettingStructure structure,
        GameTimeStructure timeStructure,
        uint minimumPlayers = 2,
        uint maxPlayers = 6
    )
    {
        this.MinimumPlayerCount = minimumPlayers;
        this.BettingStructure = structure;
        this.GameTable = new Table(seats: maxPlayers, this);
        this.GameTimeStructure = timeStructure;
    }

    public ulong Round { get; set; } = 0;

    public Table GameTable { get; set; }

    public DateTime? StartTime { get; set; }

    public TimeSpan? GameLength
    {
        get
        {
            if (GameTimeStructure == GameTimeStructure.Simulated)
            {
                return Round * BettingStructure.TimePerRound;
            }
            else if (StartTime == null)
            {
                return null;
            }
            else return DateTime.UtcNow - StartTime;
        }
    }

    public int GetGameLevel()
    {
        TimeSpan? gameLength = GameLength;
        if (gameLength == null)
            return 0; 
        return BettingStructure.GetApropriateBlindLevel(gameLength.Value).Level;
    }

    public GameTimeStructure GameTimeStructure { get; set; }
    /// <summary>
    /// the amount of players when the game starts
    /// </summary>
    public uint MinimumPlayerCount { get; private set; }
    
    /// <summary>
    /// defines the betting structure of the game
    /// </summary>
    public BettingStructure BettingStructure { get; set; }

    public Task GameTask { get; private set; }
    public CancellationToken CancelGame = new CancellationToken();
    private readonly object _GameTaskInitLock = new object();

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
            
            while (!cancellationToken.IsCancellationRequested)
            {
                SitOutBrokePlayers();
                
                // check if the game has ended
                if (await CheckEndGame())
                {
                    Debug.WriteLine("less than 2 seats taken. the game has ended");
                    break;
                }

                // Pre check if a round can be started
                if (!await CheckRoundPrecondition())
                    continue;

                // init round
                this.GameTable.MoveButtons();
                this.GameTable.DealPlayerCards();

                // progress through stages

            }
        }
        finally
        {
            Monitor.Exit(_GameTaskInitLock);
        }
    }
}