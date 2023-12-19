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
                        break;
                    }
                }
            }
            // start the rounds
            StartTime = DateTime.UtcNow;
            while (!cancellationToken.IsCancellationRequested)
            {
                // check if the game has ended


                // Pre check if a round can be started
                if (!await CheckRoundPrecondition())
                    continue;


            }
        }
        finally
        {
            Monitor.Exit(_GameTaskInitLock);
        }
    }
}