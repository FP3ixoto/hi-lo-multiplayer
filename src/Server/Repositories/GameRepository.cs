using Server.Domain;

namespace Server.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly IDictionary<Guid, Game> _games = new Dictionary<Guid, Game>();

        //public GameRepository()
        //{

        //}

        //TODO: Make thread safe
        public void Add(Game game) => _games.Add(game.Id, game);

        public void Remove(Guid id) => _games.Remove(id);

        public Game? FindOpen() => _games.Values.FirstOrDefault(x => x.State == GameState.AwaitingPlayers);

        public Game? Current(string playerConnectionId) =>
            _games.Values.FirstOrDefault(x =>
                x.Player1?.ConnectionId == playerConnectionId || x.Player2?.ConnectionId == playerConnectionId);
    }
}
