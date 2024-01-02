using Poker.Games;
using Poker.Tables;

namespace Poker.Logic.Blinds;

/// <summary>
/// Provides functionality to set the blinds and ante for a poker game. 
/// This static class includes a method to update the game's blind levels and enforce the collection of blinds and antes from the players.
/// </summary>
/// <remarks>
/// The class is designed to be used with a game instance, ensuring the blinds and antes are set in accordance with the game's current state and length.
/// It is a key component in managing the betting structure of the game.
/// </remarks>
public static class BlindsAndAnte
{
    /// <summary>
    /// Sets the blinds and ante for a given game, updating the game state accordingly.
    /// </summary>
    /// <param name="game">The game instance where blinds and ante need to be set.</param>
    /// <returns>
    /// The initial call value for the round, which is the sum of the big blind and ante, if applicable.
    /// </returns>
    /// <remarks>
    /// This method first updates the game's current blind level based on the game length. It then enforces bets for the small and big blinds.
    /// If the current blind level includes an ante, it collects the ante from each participating player. The method returns the total call value,
    /// which is the big blind plus the ante (if any), or just the big blind if no ante is set.
    /// </remarks>
    public static ulong Set(Game game)
    {
        game.CurrentBlindLevel = game.Rules.GetApropriateBlindLevel(game.GameLength);
        // collect blinds
        game.GameTable.Seats[game.GameTable.SmallBlindSeat].ForceBet(game.CurrentBlindLevel.SmallBlind);
        game.GameTable.Seats[game.GameTable.BigBlindSeat].ForceBet(game.CurrentBlindLevel.BigBlind);
        
        // early return of the initial call value if ante is not set
        if (game.CurrentBlindLevel.Ante == 0)
            return game.CurrentBlindLevel.BigBlind;
        
        // set ante
        foreach(Seat seat in game.GameTable.Seats)
        {
            if (seat.IsParticipatingGame())
            {
                seat.ForceBet(game.CurrentBlindLevel.Ante);
            }
        }
        
        // return call value
        return game.CurrentBlindLevel.BigBlind + game.CurrentBlindLevel.Ante;
    }
}