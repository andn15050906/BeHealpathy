using Hangfire;
using Infrastructure.Helpers.Monitoring;
using OfficeOpenXml;
using static Gateway.Services.Background.JobRunner;

namespace Gateway.Helpers.AppStart;

public static class OneTimeRunner
{
    private static bool _initedConfig;

    public static void InitConfig(WebApplicationBuilder builder)
    {
        if (_initedConfig)
            return;

        MonitoringExtensions.ConfigLogger(builder);
        Configurer.Init(builder.Configuration);
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        _initedConfig = true;
    }

    public static async Task RunWarmUpQuery(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealpathyContext>();

        ReadContext.Init(context);
        await ReadContext.RefreshAllCaches();

        var calculationApiService = scope.ServiceProvider.GetRequiredService<ICalculationApiService>();
        await new CalculateSentiment(context, calculationApiService).Execute();
    }

    public static void ScheduleJobs(this WebApplication app)
    {
#pragma warning disable CS0618
        /*
        RecurringJob.AddOrUpdate<CalculateRoadmapProgress>(
            nameof(CalculateRoadmapProgress),
            _ => _.Execute(),
            Cron.MinuteInterval(1)
        );
        */
#pragma warning restore CS0618
    }
}