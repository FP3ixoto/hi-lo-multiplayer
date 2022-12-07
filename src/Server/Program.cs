using Server.Hubs;
using Server.Repositories;
using Server.Services;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddCors(
//    options => options.AddDefaultPolicy(
//        policy =>
//        {
//            policy.WithOrigins("null").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
//        }));

builder.Services.AddSignalR();

builder.Services.AddSingleton<IGameRepository, GameRepository>();
builder.Services.AddTransient<IGameService, GameService>();

var app = builder.Build();

//app.UseWebSockets();

//app.MapGet("/", () => "Hello World!");

//app.UseRouting();
app.MapHub<GameHub>("/ws");

//app.UseCors();

await app.RunAsync();
