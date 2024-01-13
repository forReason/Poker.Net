using Poker.PhysicalObjects.Cards;
using Poker.PhysicalObjects.Chips;
using Poker.PhysicalObjects.HandScores;
using Poker.PhysicalObjects.Players;
using System.Collections.Generic;

namespace Poker.Logic.GameLogic.GameManagement;

public partial class Game
{
    public HashSet<Player> EvaluateWinners(Pot pot)
    {
        HashSet<Player> winners = new HashSet<Player>();
        HandScore? highScore = null;
        List<Card> communitsCards = new List<Card>();
        foreach (Card? card in GameTable.CommunityCards.TableCards)
            if (card != null)
                communitsCards.Add(card);
        foreach (Player player in pot.Players)
        {
            HandScore playerHandScore = PhysicalObjects.Decks.CardEvaluation.ScoreCards(communitsCards.ToArray(), player.Seat.PlayerPocketCards);
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

        return winners;
    }

}