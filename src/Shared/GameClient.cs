using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using Shared.Contracts;

namespace Shared;

public sealed class GameClient : IGameClient
{
    public const string HUBURL = "/ws";

    private readonly HubConnection _hubConnection;
    private bool _started = false;

    /// <summary>
    /// Ctor: create a new client for the given hub URL
    /// </summary>
    /// <param name="options">The options model with the base URL for the site, e.g. https://localhost:1234 </param>
    public GameClient(IOptions<GameClientOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var hubUrl = options.Value.Url.TrimEnd('/') + HUBURL;
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl, config =>
            {
                config.SkipNegotiation = true;
                config.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets;
            })
            .Build();

        // add handler for receiving game state update
        _hubConnection.On<GameState>(Messages.UPDATEGAMESTATE, HandleGameStateUpdate);
    }

    public async ValueTask DisposeAsync()
    {
        await Stop();
        await _hubConnection.DisposeAsync();
    }

    /// <summary>
    /// Start the SignalR client 
    /// </summary>
    public async Task Start(string playerName)
    {
        if (!_started)
        {
            // start the connection
            await _hubConnection.StartAsync();
            _started = true;

            // register the player
            await _hubConnection.SendAsync(Messages.REGISTER, playerName);
        }
    }

    public async Task Stop()
    {
        if (_started)
        {
            // disconnect the client
            await _hubConnection.StopAsync();
            _started = false;
        }
    }

    public async Task GuessMysteryNumber(int number)
    {
        if (!_started)
        {
            throw new InvalidOperationException("Client must be started before any action.");
        }

        await _hubConnection.SendAsync(Messages.GUESSNUMBER, number);
    }

    public event GameStateUpdateEventHandler? GameStateUpdateReceived;
    private void HandleGameStateUpdate(GameState gameState)
    {
        // raise an event to subscribers
        GameStateUpdateReceived?.Invoke(this, new GameStateUpdateEventHandlerArgs(gameState));
    }
}


public delegate void GameStateUpdateEventHandler(object sender, GameStateUpdateEventHandlerArgs e);

public class GameStateUpdateEventHandlerArgs : EventArgs
{
    public GameState GameState { get; set; }

    public GameStateUpdateEventHandlerArgs(GameState gameState)
    {
        GameState = gameState;
    }
}