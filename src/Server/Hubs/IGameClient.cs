namespace Server.Hubs
{
    public interface IGameClient
    {
        Task BroadcastMove(string name, int number);
        Task Notify(string message);
    }
}
