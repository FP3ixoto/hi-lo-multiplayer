using Server.Domain;

namespace Server.Repositories;

public class GameRepository : IGameRepository
{
    private readonly IDictionary<Guid, Game> _games = new Dictionary<Guid, Game>();

    public void Add(Game game) => _games.Add(game.Id, game);

    public void Remove(Guid id) => _games.Remove(id);

    public Game? FindJoinable(string playerName) =>
        _games.Values.FirstOrDefault(x => x.Status == GameStatus.AwaitingPlayers
            && x.Player1?.Name != playerName); // for the sake of the demonstration,
                                               // let's not allow a game with two players with the same name

    public Game? Current(string playerConnectionId) =>
        _games.Values.FirstOrDefault(x =>
            x.Player1?.ConnectionId == playerConnectionId || x.Player2?.ConnectionId == playerConnectionId);
}
