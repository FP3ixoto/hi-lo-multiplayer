using AutoFixture.Kernel;

namespace Server.Tests;

public class GameRepositoryTests
{
    private readonly GameRepository _gameRepository = new();
    private readonly Fixture _fixture = new();

    public GameRepositoryTests()
    {
        _fixture.Customizations.Add(
            new TypeRelay(
                typeof(IRandomNumberProvider),
                typeof(RandomNumberProvider)));
    }

    [Fact]
    public void ShouldAllowFirstPlayerToJoin()
    {
        //prepare
        var game = _fixture.Create<Game>();
        _gameRepository.Add(game);

        //execute
        var result = _gameRepository.FindJoinable("player-name");

        //assert
        Assert.NotNull(result);
        Assert.Equal(result.Id, game.Id);
    }

    [Fact]
    public void ShouldAllowSecondPlayerToJoin()
    {
        //prepare
        var game = _fixture.Create<Game>();
        _gameRepository.Add(game);
        game.AddPlayer("player-name-1", "connection-id-1");

        //execute
        var result = _gameRepository.FindJoinable("player-name-2");

        //assert
        Assert.NotNull(result);
        Assert.Equal(result.Id, game.Id);
    }

    [Fact]
    public void ShouldNotAllowNewPlayerWithSameNameToJoin()
    {
        //prepare
        var game = _fixture.Create<Game>();
        game.AddPlayer("player-name", "connection-id");
        _gameRepository.Add(game);

        //execute
        var result = _gameRepository.FindJoinable("player-name");

        //assert
        Assert.Null(result);
    }

    [Fact]
    public void ShouldFindCurrentGameForPlayer()
    {
        //prepare
        var game = _fixture.Create<Game>();
        game.AddPlayer("player1", "connection-id");
        _gameRepository.Add(game);

        //execute
        var result = _gameRepository.Current(game.Player1!.ConnectionId);

        //assert
        Assert.NotNull(result);
        Assert.Equal(result.Id, game.Id);
    }

    [Fact]
    public void ShouldNotFindCurrentGameForNotRegistedPlayer()
    {
        //prepare
        var game = _fixture.Create<Game>();
        _gameRepository.Add(game);

        //execute
        var result = _gameRepository.Current("connection-id");

        //assert
        Assert.Null(result);
    }
}