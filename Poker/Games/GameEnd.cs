using System.Diagnostics;
using Poker.Tables;
using System.Reflection.Metadata.Ecma335;

namespace Poker.Games;

public partial class Game
{

    
    /// <summary>
    /// returns true if the game ended
    /// </summary>
    /// <returns></returns>
    private async Task<bool> CheckEndGame()
    {
        if (GameTable.TakenSeats < 2)
            return true;

        return false;
    }
}