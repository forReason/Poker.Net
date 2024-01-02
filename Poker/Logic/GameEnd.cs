namespace Poker.Games;

public partial class Game
{

    
    /// <summary>
    /// returns true if the game ended
    /// </summary>
    /// <returns></returns>
    private async Task<bool> CheckEndGame()
    {
        if (GameTable.SeatsWithStakesCount < 2)
            return true;

        return false;
    }
}