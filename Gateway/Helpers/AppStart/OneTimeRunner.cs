using Infrastructure.Helpers.Monitoring;

namespace Gateway.Helpers.AppStart;

public class OneTimeRunner
{
    private static bool _initedConfig;

    public static void InitConfig(WebApplicationBuilder builder)
    {
        if (_initedConfig)
            return;

        MonitoringExtensions.ConfigLogger(builder);
        Configurer.Init(builder.Configuration);

        _initedConfig = true;
    }
}
