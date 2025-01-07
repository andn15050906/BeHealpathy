using Infrastructure.DataAccess.MongoDB;
using Infrastructure.DataAccess.SQLServer;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Helpers.WebAPI;

public static class ContextExtensions
{
    public static IServiceCollection AddContext<T, TOptions>(this IServiceCollection services, MongoOptions options)
        where T : BaseContext, IConfiguredContext<TOptions>
        where TOptions : MongoOptions
    {
        services.Configure<TOptions>(_ =>
        {
            _.ConnectionString = options.ConnectionString;
            _.Database = options.Database;
        });

        services.AddSingleton<T>();

        return services;
    }
}