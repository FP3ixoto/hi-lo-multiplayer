using Microsoft.AspNetCore.SignalR;
using Server.Services;

namespace Server.Hubs
{
    public class GameHub : Hub<IGameClient>
    {
        private readonly ILogger<GameHub> _logger;
        private readonly IGameService _gameService;

        public GameHub(ILogger<GameHub> logger, IGameService gameService)
        {
            _logger = logger;
            _gameService = gameService;
        }

        public async Task MakeMove(string name, int number)
        {

            _logger.LogInformation("Guessed number: {Number}", number);
            await Clients.All.BroadcastMove(name, number);
        }

        public async Task OnConnected(string name)
        {
            var game = _gameService.Join(name, Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, game.Id.ToString());

            if (game.State == Domain.GameState.AwaitingPlayers)
            {
                await Clients.Group(game.Id.ToString()).Notify("Awaiting another player to start...");
            }
            else
            {
                await Clients.Group(game.Id.ToString()).Notify($"Game started. {game?.CurrentPlayer?.Name}'s turn!");
            }

        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            //If game is complete delete it
            var game = _gameService.FindCurrent(Context.ConnectionId);

            if (game is not null)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, game.Id.ToString());
                //TODO
                //await Clients.Group(game.Id.ToString()).Concede();
                _gameService.Abandon(game.Id);
            }
        }
    }
}
