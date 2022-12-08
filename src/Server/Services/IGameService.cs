using Server.Contracts;

namespace Server.Services
{
    public interface IGameService
    {
        GameState Join(string playerName, string playerConnectionId);
        GameState TryGuessMysteryNumber(string playerConnectionId, int number);
        GameState Abandon(string playerConnectionId);
    }
}
