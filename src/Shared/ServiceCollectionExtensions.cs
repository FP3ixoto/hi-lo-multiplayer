using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures the DI services dependencies for the Game Client
    /// </summary>
    /// <param name="serviceCollection">The Service Collection</param>
    /// <param name="config">The Configuration</param>
    /// <returns>The <see cref="IServiceCollection"/> to make further consecutive calls</returns>
    public static IServiceCollection AddGameClient(this IServiceCollection serviceCollection, IConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        ArgumentNullException.ThrowIfNull(config);

        serviceCollection.AddOptions<GameClientOptions>().Bind(config.GetSection(GameClientOptions.OptionsKey));
        serviceCollection.AddTransient<IGameClient, GameClient>();

        return serviceCollection;
    }
}
