using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Contract.Helpers.AppExploration;

public static class AppExplorationExtensions
{
    private const string VERSION = "v1";
    private const string TITLE = "Healpathy's APIs";
    private const string DESCRIPTION = "You are viewing the API documentation of Healpathy";
    private const string SECURITY_DEFINITION_NAME = "Bearer";
    private const string SECURITY_SCHEME = SECURITY_DEFINITION_NAME;
    private const SecuritySchemeType SECURITY_SCHEME_TYPE = SecuritySchemeType.Http;
    private const string SECURITY_SCHEME_NAME = "Authorization";
    private const string SECURITY_SCHEME_DESCRIPTION = "Enter your JWT Bearer";
    private const ParameterLocation SECURITY_LOCATION = ParameterLocation.Cookie;



    public static IServiceCollection AddAppExploration(this IServiceCollection services, AppInfoOptions appInfoOptions)
    {
        return services
            .AddAppInfo(appInfoOptions)
            .AddDocumentation();
    }

    public static void UseAppExploration(this WebApplication app)
    {
        app.UseSwagger().UseSwaggerUI();
    }






    private static IServiceCollection AddAppInfo(this IServiceCollection services, AppInfoOptions appInfoOptions)
    {
        services.Configure<AppInfoOptions>(options =>
        {
            options.AppName = appInfoOptions.AppName;
            options.MainFrontendApp = appInfoOptions.MainFrontendApp;
            options.MainBackendApp = appInfoOptions.MainBackendApp;
        });

        return services;
    }

    private static IServiceCollection AddDocumentation(this IServiceCollection services)
    {
        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(options =>
            {
                options.SwaggerDoc(VERSION, new OpenApiInfo
                {
                    Version = VERSION,
                    Title = TITLE,
                    Description = DESCRIPTION
                });
                options.AddSecurityDefinition(SECURITY_DEFINITION_NAME, new OpenApiSecurityScheme
                {
                    Scheme = SECURITY_SCHEME,
                    Name = SECURITY_SCHEME_NAME,
                    Description = SECURITY_SCHEME_DESCRIPTION,
                    In = SECURITY_LOCATION,
                    Type = SECURITY_SCHEME_TYPE
                });
            });

        return services;
    }
}
