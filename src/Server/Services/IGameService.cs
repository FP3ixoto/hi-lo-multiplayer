using Server.Domain;

namespace Server.Services
{
    public interface IGameService
    {
        Game Join(string playerName, string playerConnectionId);
        Game? FindCurrent(string playerConnectionId);
        void Abandon(Guid gameId);
    }
}
