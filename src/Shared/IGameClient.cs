namespace Shared;

public interface IGameClient : IAsyncDisposable
{
    event GameStateUpdateEventHandler? GameStateUpdateReceived;

    Task Start(string playerName);
    Task Stop();
    Task GuessMysteryNumber(int number);
}
