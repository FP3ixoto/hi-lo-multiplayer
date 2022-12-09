using Shared.Contracts;

namespace Server.Services;

public interface IGameService
{
    /// <summary>
    /// Join a player to a game that has not yet started.
    /// </summary>
    /// <param name="playerName">The player name.</param>
    /// <param name="playerConnectionId">The player connection id.</param>
    /// <exception cref="ArgumentException">If playerName is null or empty.</exception>
    /// <exception cref="ArgumentException">If playerConnectionId is null or empty.</exception>
    /// <returns>The state of the game.</returns>
    GameState Join(string playerName, string playerConnectionId);
    /// <summary>
    /// Tries to make a guess based on the proposed number, 
    /// validates if it is the mystery number and, if not, 
    /// returns information regarding the guess being higher or lower than the correct answer
    /// </summary>
    /// <param name="playerConnectionId">The player connection id.</param>
    /// <param name="number">The proposed number.</param>
    /// <exception cref="ArgumentException">If playerConnectionId is null or empty.</exception>
    /// <exception cref="InvalidOperationException">If the player hasn't joined a game yet.</exception>
    /// <exception cref="InvalidOperationException">If the game joined by the player hasn't started yet.</exception>
    /// <returns>The state of the game.</returns>
    GameState TryGuessMysteryNumber(string playerConnectionId, int number);
    /// <summary>
    /// Removes the player from the current game being played.
    /// </summary>
    /// <param name="playerConnectionId">The player connection id.</param>
    /// <exception cref="ArgumentException">If playerConnectionId is null or empty.</exception>
    /// <exception cref="InvalidOperationException">If the player hasn't joined a game yet.</exception>
    /// <exception cref="InvalidOperationException">If the game joined by the player hasn't started yet.</exception>
    /// <returns>The state of the game.</returns>
    GameState Abandon(string playerConnectionId);
}
