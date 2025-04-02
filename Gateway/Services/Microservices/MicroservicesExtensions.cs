using Contract.Messaging.ApiClients.Http;
using Contract.Services.Implementations;
using Gateway.Helpers.AppStart;
using Contract.Services;

namespace Gateway.Services.Microservices;

public static class MicroservicesExtensions
{
    public static IServiceCollection AddMicroservices(this IServiceCollection services)
    {
        //var options = Configurer.ApiClientOptions;

        services
            .AddScoped<HttpClientAuthorizationDelegatingHandler>()
            .AddHttpService<ICalculationApiService, CalculationHttpService>(Configurer.ApiClientOptions.CalculationPath);

        return services;
    }
}
