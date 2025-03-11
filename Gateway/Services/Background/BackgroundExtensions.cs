using Hangfire;

namespace Gateway.Services.Background;

public static class BackgroundExtensions
{
    public static IServiceCollection AddBackgroundServices(this IServiceCollection services, BackgroundOptions options)
    {
        services.AddHangfire(_ => _
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(options.ConnectionString)
        );
        services.AddHangfireServer();
        return services;
    }
}
