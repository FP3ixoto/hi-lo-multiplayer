using Server.Contracts;

namespace Server.Hubs
{
    public interface IGameClient
    {
        Task UpdateGameState(GameState gameState);
    }
}
