namespace Shared.Contracts;

public class GameState
{
    public Guid GameId { get; init; }
    public string GameStateDescription { get; init; } = string.Empty;
    public string PlayerTurn { get; init; } = string.Empty;
    public string Winner { get; init; } = string.Empty;
    public GameMove? LastMove { get; init; }
}
