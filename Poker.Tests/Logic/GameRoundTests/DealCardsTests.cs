using Poker.Net.Logic.Blinds;
using Poker.Net.Logic.GameLogic.GameManagement;
using Poker.Net.Logic.GameLogic.Rules;
using Poker.Net.PhysicalObjects.Cards;
using Poker.Net.PhysicalObjects.Chips;
using Poker.Net.PhysicalObjects.Players;
using Poker.Net.PhysicalObjects.Tables;

namespace Poker.Tests.Logic.GameRoundTests;

public class DealCardsTests
{
    [Fact]
    public static void GenerateEntries()
    {
        // set up table
        uint seats = 6;
        RuleSet rules = new RuleSet(GameMode.Cash, 1000, BlindToBuyInRatio.OneToTen);
        Game game = new Game(rules);
        Table table = new Table(seats, game);
        Pot playersList = new Pot();
        // set up players
        for (uint i = 0; i < seats; i++)
        {
            Player player = new();
            player.AddPlayerBank(1000);
            SitInResult result = table.Seats[i].SitIn(player, 1000);
            table.Seats[i].Stack.PerformBet(playersList, 10, player);
        }

        // make 1000 iterations
        Player[] winners;
        List<Card> hand = new();
        List<Card> community = new();
        for (int i = 0; i < 1000; i++)
        {
            // deal cards
            table.CommunityCards.Clear();
            // past issue: index exception
            table.DealPlayerCards(); // shuffles cards and revokes hands
        }
    }
}