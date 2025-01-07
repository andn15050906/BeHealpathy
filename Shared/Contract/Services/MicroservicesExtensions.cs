using Contract.Messaging.ApiClients.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Contract.Services;

public static class MicroservicesExtensions
{
    public static IServiceCollection AddMQService<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    {
        services
            .AddScoped<TService, TImplementation>();

        return services;
    }

    public static IServiceCollection AddHttpService<TService, TImplementation>(this IServiceCollection services, string path)
        where TService : class
        where TImplementation : class, TService
    {
        services
            .AddScoped<TService, TImplementation>()
            .AddHttpClient<TService, TImplementation>(_ => _.BaseAddress = new Uri(path))
            .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();

        return services;
    }
}
