using Contract.Helpers.FeatureFlags;
using Infrastructure.Helpers.Monitoring;
using Microsoft.Extensions.Options;

namespace Gateway.Services.Cache;

public static class CacheExtensions
{
    public static IServiceCollection AddCache(this IServiceCollection services, CacheOptions cacheOptions)
    {
        var cacheBase = new CacheBase(cacheOptions, new Logger());
        services.AddSingleton<ICacheBase>(cacheBase);

        /*var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();
        var options = provider.GetRequiredService<IOptions<FeatureFlagOptions>>();
        services.AddSingleton<IEventCache>(new EventCache(cacheBase, mediator, options));*/
        services.AddScoped<IEventCache, EventCache>();
        return services;
    }
}