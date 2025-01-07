using Contract.Helpers;
using Contract.Helpers.AppExploration;
using Contract.Helpers.Storage;
using Core.Helpers;
using Gateway.Services.Microservices;
using Infrastructure.DataAccess.SQLServer;
using Infrastructure.Helpers.Email;

namespace Gateway.Helpers.AppStart;

public class Configurer
{
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

    private static CookieConfigOptions _authCookieOptions;






    public static void Init(ConfigurationManager configuration)
    {
        _configuration ??= configuration;
        IsRunningInContainer = bool.TryParse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), out var inContainer) && inContainer;

        CorsOrigins = Get<string[]>("CORS");
        AppInfoOptions = Get<AppInfoOptions>("AppInfo");
        TokenOptions = Get<TokenOptions>("JwtOptions");

        ApiClientOptions = Get<ApiClientOptions>("ServicePaths")!;
        //IdentityContextOptions = Get<MongoOptions>(IsRunningInContainer ? "IdentityContext:Container" : "IdentityContext:Local")!;
        //NotificationContextOptions = Get<MongoOptions>(IsRunningInContainer ? "NotificationContext:Container" : "NotificationContext:Local")!;
        GatewayContextOptions = Get<SqlServerOptions>("GatewayContext:SqlServer:Local")!;
        CloudStorageConfig = Get<CloudStorageConfig>("External:Cloudinary");
        RealtimeOptions = Get<Dependencies.Realtime.Options>("Azure:SignalR");

        EmailOptions = Get<EmailOptions>("External:Gmail");
        OAuthOptions = Get<OAuthOptions>("External:OAuth:Google");

        _authCookieOptions = Get<CookieConfigOptions>("CookieOptions");
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






    private static T Get<T>(string key)
    {
        return _configuration!.GetSection(key).Get<T>();
    }
}
