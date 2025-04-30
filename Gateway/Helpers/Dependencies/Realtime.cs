using Gateway.Helpers.AppStart;

namespace Gateway.Helpers.Dependencies;

public static class Realtime
{
    public class Options
    {
        public string Enabled { get; set; }
        public string? ConnectionString { get; set; }
    }

    public static IServiceCollection AddRealtimeService(this IServiceCollection services)
    {
        var serverBuilder = services.AddSignalR(_ => _.MaximumReceiveMessageSize = 128000);

        var options = Configurer.RealtimeOptions;
        if (options is null || options.Enabled != "true")
            return services;

        serverBuilder.AddAzureSignalR(_ => {
            _.ClaimsProvider = context => context.User.Claims;
        });
        return services;
    }
}
