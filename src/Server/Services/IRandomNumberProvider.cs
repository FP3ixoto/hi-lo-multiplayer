namespace Server.Services;

public interface IRandomNumberProvider
{
    int GetInt32(int toExclusive);
}
