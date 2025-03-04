using Infrastructure.Helpers.Monitoring;

namespace Gateway.Services.Cache;

public static class CacheExtensions
{
    public static IServiceCollection AddCache(this IServiceCollection services, CacheOptions cacheOptions)
    {
        var cacheService = new CacheService(cacheOptions, new Logger());
        services.AddSingleton<ICacheService>(cacheService);
        return services;
    }
}