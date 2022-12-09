using Server.Hubs;
using Server.Repositories;
using Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

builder.Services.AddSingleton<IGameRepository, GameRepository>();
builder.Services.AddTransient<IGameService, GameService>();

var app = builder.Build();

app.MapHub<GameHub>("/ws");

await app.RunAsync();
