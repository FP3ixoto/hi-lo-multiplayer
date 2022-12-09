using AutoFixture.Kernel;

namespace Server.Tests;

public class GameServiceTests
{
    private readonly GameService _gameService;
    private readonly Mock<IGameRepository> _gameRepositoryMock;
    private readonly Mock<IRandomNumberProvider> _randomNumberProviderMock;
    private readonly Fixture _fixture = new();

    public GameServiceTests()
    {
        _fixture.Customizations.Add(
            new TypeRelay(
                typeof(IRandomNumberProvider),
                typeof(RandomNumberProvider)));

        _randomNumberProviderMock = new Mock<IRandomNumberProvider>();
        _randomNumberProviderMock.Setup(x => x.GetInt32(It.IsAny<int>())).Returns(50);

        _gameRepositoryMock = new Mock<IGameRepository>();

        _gameService = new(_gameRepositoryMock.Object, _randomNumberProviderMock.Object);
    }

    [Fact]
    public void JoiningGameWithNoPlayersShouldAddAsPlayer1()
    {
        //prepare
        var game = _fixture.Create<Game>();
        _gameRepositoryMock.Setup(x => x.FindJoinable(It.IsAny<string>())).Returns(game);

        //execute
        var result = _gameService.Join("player-name-1", "connection-id-1");

        //assert
        //_gameRepositoryMock.Verify(x => x.)
        Assert.NotNull(game.Player1);
        Assert.Equal("player-name-1", game.Player1.Name);
    }

    [Fact]
    public void JoiningGameWithOnePlayerShouldAddAsPlayer2()
    {
        //prepare
        var game = _fixture.Create<Game>();
        game.AddPlayer("player-name-1", "connection-id-1");
        _gameRepositoryMock.Setup(x => x.FindJoinable(It.IsAny<string>())).Returns(game);

        //execute
        var result = _gameService.Join("player-name-2", "connection-id-2");

        //assert
        Assert.NotNull(game.Player2);
        Assert.Equal("player-name-2", game.Player2.Name);
    }

    [Fact]
    public void JoiningWhenNoGameAvailableShouldCreateNewGameWithPlayer()
    {
        //prepare
        _gameRepositoryMock.Setup(x => x.FindJoinable(It.IsAny<string>())).Returns((Game?)null);

        Game? addedGame = null;
        _gameRepositoryMock.Setup(x => x.Add(It.IsAny<Game>())).Callback<Game>(game => addedGame = game);

        //execute
        var result = _gameService.Join("player-name-1", "connection-id-1");

        //assert
        _gameRepositoryMock.Verify(x => x.Add(It.IsAny<Game>()), Times.Once);
        Assert.NotNull(addedGame);
        Assert.NotNull(addedGame.Player1);
        Assert.Equal("player-name-1", addedGame.Player1.Name);
    }

    [Fact]
    public void AbandonShouldThrowIfPlayerIsNotPlaying()
    {
        //prepare
        _gameRepositoryMock.Setup(x => x.Current(It.IsAny<string>())).Returns((Game?)null);

        //execute & assert
        Assert.Throws<InvalidOperationException>(() => _gameService.Abandon("connection-id-1"));
    }

    [Fact]
    public void AbandonShouldRemoveGameFromRepository()
    {
        //prepare
        var game = _fixture.Create<Game>();
        game.AddPlayer("player-name-1", "connection-id-1");
        game.AddPlayer("player-name-2", "connection-id-2");
        _gameRepositoryMock.Setup(x => x.Current(It.IsAny<string>())).Returns(game);

        //execute
        var result = _gameService.Abandon("connection-id-1");

        //assert
        _gameRepositoryMock.Verify(x => x.Remove(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public void AbandonShouldReturnFinishedStatusGame()
    {
        //prepare
        var game = _fixture.Create<Game>();
        game.AddPlayer("player-name-1", "connection-id-1");
        game.AddPlayer("player-name-2", "connection-id-2");
        _gameRepositoryMock.Setup(x => x.Current(It.IsAny<string>())).Returns(game);

        //execute
        var result = _gameService.Abandon("connection-id-1");

        //assert
        Assert.Equal(GameStatus.Finished.Description(), result.GameStateDescription);
    }

    [Fact]
    public void TryGuessMysteryNumberShouldThrowIfPlayerIsNotPlaying()
    {
        //prepare
        _gameRepositoryMock.Setup(x => x.Current(It.IsAny<string>())).Returns((Game?)null);

        //execute & assert
        Assert.Throws<InvalidOperationException>(() => _gameService.TryGuessMysteryNumber("connection-id-1", 10));
    }

    [Fact]
    public void TryGuessMysteryNumberShouldThrowIfIsNotPlayerTurn()
    {
        //prepare
        var game = _fixture.Create<Game>();
        game.AddPlayer("player-name-1", "connection-id-1");
        game.AddPlayer("player-name-2", "connection-id-2");
        _gameRepositoryMock.Setup(x => x.Current(It.IsAny<string>())).Returns(game);

        //execute & assert
        var playerConnectionId = game.CurrentPlayer!.ConnectionId == game.Player1!.ConnectionId ?
            game.Player2!.ConnectionId :
            game.Player1!.ConnectionId;

        Assert.Throws<InvalidOperationException>(() => _gameService.TryGuessMysteryNumber(playerConnectionId, 10));
    }

    [Fact]
    public void TryGuessMysteryNumberShouldRemoveGameIfFinished()
    {
        //prepare
        var game = new Game(_randomNumberProviderMock.Object);
        game.AddPlayer("player-name-1", "connection-id-1");
        game.AddPlayer("player-name-2", "connection-id-2");
        _gameRepositoryMock.Setup(x => x.Current(It.IsAny<string>())).Returns(game);

        //execute
        _gameService.TryGuessMysteryNumber(game.CurrentPlayer!.ConnectionId, 50);

        //assert
        _gameRepositoryMock.Verify(x => x.Remove(game.Id), Times.Once);
    }
}