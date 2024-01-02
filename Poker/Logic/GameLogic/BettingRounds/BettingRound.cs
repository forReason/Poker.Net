using Poker.Games;
using Poker.Logic.Blinds;

namespace Poker.Logic.GameLogic.BettingRounds;

/// <summary>
/// the betting round is one of the core logics of the game.<br/>
/// After dealing the pocket cards, the Betting round starts by collecting blinds and Antes according to the game rule set.<br/>
/// then the players can bet and the pots inclusive side pots are being built
/// </summary>
public partial class BettingRound
{
    /// <summary>
    /// the game instance which is to be managed
    /// </summary>
    private readonly Game _game;
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
    /// initializes a betting round instance for the referenced game. The Betting round can manage the game state such as Pots
    /// </summary>
    /// <param name="game"></param>
    public BettingRound(Game game)
    {
        _game = game;
    }
    
    /// <summary>
    /// this function sets the bets for BigBlind and SmallBlind player and Ante for everyone
    /// </summary>
    public void InitializeNewBettingRound()
    {
        BetsReceived = 0;
        CallValue = BlindsAndAnte.Set(_game);
    }
}