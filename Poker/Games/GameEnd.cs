using Poker.Tables;
using System.Reflection.Metadata.Ecma335;

namespace Poker.Games;

public partial class Game
{

    /// <summary>
    /// returns true if the game got started
    /// </summary>
    /// <returns></returns>
    private async Task<bool> CheckEndGame()
    {
        // initialize dealer
        for(int i = 0; i < GameTable.Seats.Length; i++)
        {
            if (GameTable.Seats[i].IsDealer)
            {

            }
        }
    }
}