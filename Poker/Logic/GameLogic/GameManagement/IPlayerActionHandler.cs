using Poker.PhysicalObjects.Tables;

namespace Poker.Logic.GameLogic.GameManagement;

public interface IPlayerActionHandler
{
    ulong TakeAction(ulong callValue, ulong minRaise, ulong maxRaise, CancellationToken token);
}
