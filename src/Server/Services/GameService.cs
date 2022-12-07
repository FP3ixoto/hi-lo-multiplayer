using Server.Domain;
using Server.Repositories;

namespace Server.Services
{
    public sealed class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;

        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public Game Join(string playerName, string playerConnectionId)
        {
            var openGame = _gameRepository.FindOpen();

            if (openGame is null)
            {
                openGame = new Game();
                _gameRepository.Add(openGame);
            }

            openGame.AddPlayer(playerName, playerConnectionId);
            return openGame;
        }

        public Game? FindCurrent(string playerConnectionId) =>
            _gameRepository.Current(playerConnectionId);

        public void Abandon(Guid gameId) => _gameRepository.Remove(gameId);
    }
}
