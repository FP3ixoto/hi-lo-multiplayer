using Server.Domain;

namespace Server.Repositories;

public interface IGameRepository
{
    /// <summary>
    /// Adds a game to the repo.
    /// </summary>
    /// <param name="game">The game to be added.</param>
    void Add(Game game);
    /// <summary>
    /// Removes a game from the repo.
    /// </summary>
    /// <param name="id">The game id.</param>
    void Remove(Guid id);
    /// <summary>
    /// Finds a joinable game.
    /// </summary>
    /// <param name="playerName">The player name.</param>
    /// <returns>The game, if exists</returns>
    Game? FindJoinable(string playerName);
    /// <summary>
    /// Fetches the current game being player by the player with said connection id.
    /// </summary>
    /// <param name="playerConnectionId">The player connection id.</param>
    /// <returns>The game, if exists.</returns>
    Game? Current(string playerConnectionId);
}
