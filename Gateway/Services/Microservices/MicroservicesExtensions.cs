using Contract.Messaging.ApiClients.Http;
using Contract.Services.Implementations;
using Gateway.Helpers.AppStart;
using Contract.Services;
using Contract.Services.Contracts.Domain;

namespace Gateway.Services.Microservices;

public static class MicroservicesExtensions
{
    public static IServiceCollection AddMicroservices(this IServiceCollection services)
    {
        //var options = Configurer.ApiClientOptions;

        services
            .AddScoped<HttpClientAuthorizationDelegatingHandler>()
            .AddHttpService<ICourseApiService, CourseHttpService>(Configurer.ApiClientOptions.CoursesPath);

        /*services
            .AddMQService<ICourseApiService, CourseMQService>()
        */

        return services;
    }
}
