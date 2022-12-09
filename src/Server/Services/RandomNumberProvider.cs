using System.Security.Cryptography;

namespace Server.Services;

public class RandomNumberProvider : IRandomNumberProvider
{
    public int GetInt32(int toExclusive) => RandomNumberGenerator.GetInt32(toExclusive);
}
