using Poker.Net.PhysicalObjects.Tables;

namespace Poker.Net.Logic.GameLogic.GameManagement;

public interface IPlayerActionHandler
{
    ulong TakeAction(ulong callValue, ulong minRaise, ulong maxRaise, CancellationToken token);
}
