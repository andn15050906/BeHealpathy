using System.Text.Json;
using Contract.Helpers;
using Contract.Helpers.AppExploration;
using Contract.Helpers.FeatureFlags;
using Contract.Helpers.Storage;
using Core.Helpers;
using Gateway.Services.Microservices;
using Infrastructure.DataAccess.SQLServer;
using Infrastructure.Helpers.Email;

namespace Gateway.Helpers.AppStart;

public class Configurer
{
    public const string DOTNET_RUNNING_IN_CONTAINER = "DOTNET_RUNNING_IN_CONTAINER";
    public const string ENV_CORS = "ENV_CORS";
    public const string ENV_APP_INFO = "ENV_APP_INFO";
    public const string ENV_GATEWAY = "ENV_GATEWAY";
    public const string ENV_EMAIL = "ENV_EMAIL";
    public const string ENV_OAUTH = "ENV_OAUTH";
    public const string ENV_FEATURE_FLAG = "ENV_FEATURE_FLAG";

    private static ConfigurationManager? _configuration;
    public static bool IsRunningInContainer;

    public static string[] CorsOrigins;
    public static AppInfoOptions AppInfoOptions;
    public static TokenOptions TokenOptions;

    public static ApiClientOptions ApiClientOptions;
    /*public static MongoOptions IdentityContextOptions;
    public static MongoOptions NotificationContextOptions;*/
    public static SqlServerOptions GatewayContextOptions;
    public static CloudStorageConfig CloudStorageConfig;
    public static Dependencies.Realtime.Options RealtimeOptions;

    public static EmailOptions EmailOptions;
    public static OAuthOptions OAuthOptions;
    public static FeatureFlagOptions FeatureFlags;

    private static CookieConfigOptions _authCookieOptions;






    public static void Init(ConfigurationManager configuration)
    {
        _configuration ??= configuration;
        IsRunningInContainer = bool.TryParse(Environment.GetEnvironmentVariable(DOTNET_RUNNING_IN_CONTAINER), out var inContainer) && inContainer;

        CorsOrigins = Get<string[]>("CORS", ENV_CORS)!;
        AppInfoOptions = Get<AppInfoOptions>("AppInfo", ENV_APP_INFO)!;
        TokenOptions = Get<TokenOptions>("JwtOptions")!;

        ApiClientOptions = Get<ApiClientOptions>("ServicePaths")!;
        //IdentityContextOptions = Get<MongoOptions>(IsRunningInContainer ? "IdentityContext:Container" : "IdentityContext:Local")!;
        //NotificationContextOptions = Get<MongoOptions>(IsRunningInContainer ? "NotificationContext:Container" : "NotificationContext:Local")!;
        GatewayContextOptions = Get<SqlServerOptions>("GatewayContext:SqlServer:Local", ENV_GATEWAY)!;
        CloudStorageConfig = Get<CloudStorageConfig>("External:Cloudinary")!;
        RealtimeOptions = Get<Dependencies.Realtime.Options>("Azure:SignalR")!;

        EmailOptions = Get<EmailOptions>("External:Gmail", ENV_EMAIL)!;
        OAuthOptions = Get<OAuthOptions>("External:OAuth:Google", ENV_OAUTH)!;
        FeatureFlags = Get<FeatureFlagOptions>("FeatureFlags", ENV_FEATURE_FLAG)!;

        _authCookieOptions = Get<CookieConfigOptions>("CookieOptions")!;
    }






    public static CookieOptions GetAuthOptions() => new()
    {
        SameSite = SameSiteMode.None,
        Secure = _authCookieOptions.Secure,
        Expires = TimeHelper.Now.AddMinutes(_authCookieOptions.Expires)
    };

    public static CookieOptions GetExpiredOptions() => new()
    {
        SameSite = SameSiteMode.None,
        Secure = _authCookieOptions.Secure,
        Expires = TimeHelper.Now
    };






    private static T? Get<T>(string key, string? envKey = null)
    {
        T? option = _configuration!.GetSection(key).Get<T>();

        if (envKey is not null)
        {
            var env = Environment.GetEnvironmentVariable(envKey);
            if (env is not null)
                option = JsonSerializer.Deserialize<T>(env);
        }
        return option;
    }
}
