using Poker.Cards;
using Poker.Chips;
using Poker.Decks;
using Poker.Players;

namespace Poker.Logic.GameLogic.GameManagement;

public partial class Game
{
    public Player[] EvaluateWinners(Pot pot)
    {
        List<Player> winners = new List<Player>();
        HandScore? highScore = null;
        List<Card> communitsCards = new List<Card>();
        foreach (Card? card in GameTable.CommunityCards.TableCards)
            if (card != null)
                communitsCards.Add(card.Value);
        foreach (Player player in pot.Players)
        {
            HandScore playerHandScore =
                CardEvaluation.ScoreCards(communitsCards.ToArray(), player.Seat.PlayerPocketCards);
            if (highScore == null || playerHandScore > highScore)
            {
                highScore = playerHandScore;
                winners.Clear();
                winners.Add(player);
            }
            else if (playerHandScore == highScore)
            {
                winners.Add(player);
            }
        }

        return winners.ToArray();
    }

}