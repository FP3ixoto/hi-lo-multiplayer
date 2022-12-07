using Server.Domain;

namespace Server.Repositories
{
    public interface IGameRepository
    {
        void Add(Game game);
        void Remove(Guid id);
        Game? FindOpen();
        Game? Current(string playerConnectionId);
    }
}
