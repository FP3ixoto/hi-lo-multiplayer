namespace Server.Tests;

public class GameTests
{
    private readonly Mock<IRandomNumberProvider> _randomNumberProviderMock;
    private readonly Game _game;

    public GameTests()
    {
        _randomNumberProviderMock = new Mock<IRandomNumberProvider>();
        _randomNumberProviderMock.Setup(x => x.GetInt32(It.IsAny<int>())).Returns(50);

        _game = new(_randomNumberProviderMock.Object);
    }

    [Fact]
    public void ShouldAllowOnePlayerToJoin()
    {
        //execute
        var exception = Record.Exception(() => _game.AddPlayer("player-1", "connection-id-1"));

        //assert
        Assert.Null(exception);
    }

    [Fact]
    public void ShouldAllowTwoPlayersToJoin()
    {
        //prepare
        _game.AddPlayer("player-1", "connection-id-1");

        //execute
        var exception = Record.Exception(() => _game.AddPlayer("player-2", "connection-id-2"));

        //assert
        Assert.Null(exception);
    }

    [Fact]
    public void ShouldNotAllowThreePlayersToJoin()
    {
        //prepare
        _game.AddPlayer("player-1", "connection-id-1");
        _game.AddPlayer("player-2", "connection-id-2");

        //assert
        Assert.Throws<InvalidOperationException>(() => _game.AddPlayer("player-3", "connection-id-3"));
    }

    [Fact]
    public void ShouldNotStartBeforeAllPlayersJoined()
    {
        //prepare
        _game.AddPlayer("player-1", "connection-id-1");

        //assert
        Assert.Equal(GameStatus.AwaitingPlayers, _game.Status);
    }

    [Fact]
    public void ShouldStartOnSecondPlayerJoined()
    {
        //prepare
        _game.AddPlayer("player-1", "connection-id-1");
        _game.AddPlayer("player-2", "connection-id-2");

        //assert
        Assert.Equal(GameStatus.InProgress, _game.Status);
    }

    [Fact]
    public void ShouldFinishGameOnMysteryNumberGuessed()
    {
        //prepare
        _game.AddPlayer("player-1", "connection-id-1");
        _game.AddPlayer("player-2", "connection-id-2");

        var playerTurn = _game.CurrentPlayer;

        //execute
        _game.ValidateProposedNumber(50);

        //assert
        Assert.Equal(GameStatus.Finished, _game.Status);
    }

    [Fact]
    public void ShouldNotFinishGameOnMysteryNumberNotGuessed()
    {
        //prepare
        _game.AddPlayer("player-1", "connection-id-1");
        _game.AddPlayer("player-2", "connection-id-2");

        var playerTurn = _game.CurrentPlayer;

        //execute
        _game.ValidateProposedNumber(20);

        //assert
        Assert.Equal(GameStatus.InProgress, _game.Status);
    }

    [Fact]
    public void ShouldUpdateCurrentPlayerOnMysteryNumberNotGuessed()
    {
        //prepare
        _game.AddPlayer("player-1", "connection-id-1");
        _game.AddPlayer("player-2", "connection-id-2");

        var playerTurn = _game.CurrentPlayer;

        //execute
        _game.ValidateProposedNumber(20);

        //assert
        Assert.NotEqual(playerTurn, _game.CurrentPlayer);
    }
}