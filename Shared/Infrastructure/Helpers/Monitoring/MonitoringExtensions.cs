using Contract.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Infrastructure.Helpers.Monitoring;

public static class MonitoringExtensions
{
    public static void ConfigLogger(WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.File(
                "logs/log-.txt",
                rollingInterval: RollingInterval.Day,
                retainedFileTimeLimit: TimeSpan.FromDays(7),
                fileSizeLimitBytes: 536870912)                  // 512 MB
            .CreateLogger();

        Log.Information("__Starting web host");

        builder.Host.UseSerilog();
    }

    public static IServiceCollection AddMonitoring(this IServiceCollection services)
    {
        services.AddSingleton<IAppLogger, Logger>();
        return services;
    }
}