namespace Server.Domain;

public enum GameStatus
{
    AwaitingPlayers, InProgress, Finished
}

public static class GameStateExtensions
{
    public static string Description(this GameStatus gameState) =>
        gameState switch
        {
            GameStatus.AwaitingPlayers => "Awaiting players",
            GameStatus.InProgress => "In Progress",
            GameStatus.Finished => "Finished",
            _ => throw new NotImplementedException(),
        };
}
