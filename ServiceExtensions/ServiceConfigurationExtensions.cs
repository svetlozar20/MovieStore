using MovieStore.Models.Configurations;

namespace MovieStore.ServiceExtensions;

public static class ServiceConfigurationExtensions
{
    public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        return services.Configure<MongoDbConfiguration>(configuration.GetSection(nameof(MongoDbConfiguration)));
    }
}