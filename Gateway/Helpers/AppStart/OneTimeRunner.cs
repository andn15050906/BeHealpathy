using Gateway.Services.Background;
using Hangfire;
using Infrastructure.Helpers.Monitoring;
using OfficeOpenXml;

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

    public static void RunWarmUpQuery(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealpathyContext>();
        context.Users.Count();
    }

    public static void ScheduleJobs(this WebApplication app)
    {
#pragma warning disable CS0618
        RecurringJob.AddOrUpdate<JobRunner.CalculateRoadmapProgress>(
            nameof(JobRunner.CalculateRoadmapProgress),
            _ => _.Execute(),
            Cron.MinuteInterval(5)
        );
#pragma warning restore CS0618
    }
}