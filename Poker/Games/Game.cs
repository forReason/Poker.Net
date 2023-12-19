using Poker.Blinds;
using Poker.Tables;

namespace Poker.Games;

public partial class Game
{
    public Game(
        BettingStructure structure,
        uint minimumPlayers = 2,
        uint maxPlayers = 6
    )
    {
        this.MinimumPlayerCount = minimumPlayers;
        this.BettingStructure = structure;
        this.GameTable = new Table(seats: maxPlayers);
    }

    public ulong Round { get; set; } = 0;

    public Table GameTable { get; set; }
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
            while (!cancellationToken.IsCancellationRequested)
            {
                // TODO: Go through the stages of betting
            }
        }
        finally
        {
            Monitor.Exit(_GameTaskInitLock);
        }
    }
}