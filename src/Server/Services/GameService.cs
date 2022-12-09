using Server.Domain;
using Server.Repositories;
using Shared.Contracts;

namespace Server.Services;

public sealed class GameService : IGameService
{
    private static readonly object Object = new();
    private readonly IGameRepository _gameRepository;
    private readonly IRandomNumberProvider _randomNumberProvider;

    public GameService(IGameRepository gameRepository, IRandomNumberProvider randomNumberProvider)
    {
        ArgumentNullException.ThrowIfNull(gameRepository);
        ArgumentNullException.ThrowIfNull(randomNumberProvider);

        _gameRepository = gameRepository;
        _randomNumberProvider = randomNumberProvider;
    }

    public GameState Join(string playerName, string playerConnectionId)
    {
        ArgumentException.ThrowIfNullOrEmpty(playerName);
        ArgumentException.ThrowIfNullOrEmpty(playerConnectionId);

        lock (Object)
        {
            var game = _gameRepository.FindJoinable(playerName);

            if (game is null)
            {
                game = new Game(_randomNumberProvider);
                _gameRepository.Add(game);
            }

            game.AddPlayer(playerName, playerConnectionId);

            return new GameState()
            {
                GameId = game.Id,
                GameStateDescription = game.Status.Description(),
                PlayerTurn = game.CurrentPlayer?.Name ?? string.Empty
            };
        }
    }

    public GameState TryGuessMysteryNumber(string playerConnectionId, int number)
    {
        ArgumentException.ThrowIfNullOrEmpty(playerConnectionId);

        var game = _gameRepository.Current(playerConnectionId);

        if (game is null)
        {
            throw new InvalidOperationException("Player hasn't started a game yet.");
        }

        game.AssertValidInProgressGameState();

        //Ignore if not player turn
        if (game.CurrentPlayer!.ConnectionId != playerConnectionId)
        {
            throw new InvalidOperationException("Wrong player turn.");
        }

        var currentPlayerName = game.CurrentPlayer.Name;
        var hiLo = game.ValidateProposedNumber(number);

        if (game.Status == GameStatus.Finished)
        {
            _gameRepository.Remove(game.Id);
        }

        return new GameState()
        {
            GameId = game.Id,
            GameStateDescription = game.Status.Description(),
            PlayerTurn = game.CurrentPlayer?.Name ?? string.Empty,
            Winner = hiLo == 0 ? currentPlayerName : string.Empty,
            LastMove = new GameMove()
            {
                Player = currentPlayerName,
                ProposedNumber = number,
                HiLo = hiLo
            }
        };
    }

    public GameState Abandon(string playerConnectionId)
    {
        ArgumentException.ThrowIfNullOrEmpty(playerConnectionId);

        var game = _gameRepository.Current(playerConnectionId);

        if (game is null)
        {
            throw new InvalidOperationException("Player hasn't started a game yet.");
        }

        game.AssertValidInProgressGameState();

        game.Finish();
        _gameRepository.Remove(game.Id);

        return new GameState()
        {
            GameId = game.Id,
            GameStateDescription = game.Status.Description(),
            Winner = game.Player1?.ConnectionId == playerConnectionId ? game.Player2!.Name : game.Player1!.Name
        };
    }
}
