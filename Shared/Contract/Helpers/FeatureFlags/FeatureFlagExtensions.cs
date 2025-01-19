using Microsoft.Extensions.DependencyInjection;

namespace Contract.Helpers.FeatureFlags;

public static class FeatureFlagExtensions
{
    public static IServiceCollection AddFeatureFlags(this IServiceCollection services, FeatureFlagOptions flags)
    {
        services.Configure<FeatureFlagOptions>(options =>
        {
            options.EmailEnabled = flags.EmailEnabled;
        });

        return services;
    }
}
