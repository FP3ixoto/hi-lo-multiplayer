using Microsoft.AspNetCore.SignalR;
using Server.Services;
using Shared;

namespace Server.Hubs;

public class GameHub : Hub
{
    private readonly ILogger<GameHub> _logger;
    private readonly IGameService _gameService;

    public GameHub(ILogger<GameHub> logger, IGameService gameService)
    {
        _logger = logger;
        _gameService = gameService;
    }

    public async Task GuessNumber(int number)
    {
        var gameState = _gameService.TryGuessMysteryNumber(Context.ConnectionId, number);
        await Clients.Group(gameState.GameId.ToString()).SendAsync(Messages.UPDATEGAMESTATE, gameState);
    }

    public async Task Register(string name)
    {
        var gameState = _gameService.Join(name, Context.ConnectionId);
        await Groups.AddToGroupAsync(Context.ConnectionId, gameState.GameId.ToString());
        await Clients.Group(gameState.GameId.ToString()).SendAsync(Messages.UPDATEGAMESTATE, gameState);
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("Connected: {ConnectionId}", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        //If game is complete delete it
        var gameState = _gameService.Abandon(Context.ConnectionId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameState.GameId.ToString());
        await Clients.Group(gameState.GameId.ToString()).SendAsync(Messages.UPDATEGAMESTATE, gameState);

        _logger.LogInformation("Disconnected: {ConnectionId}", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}
