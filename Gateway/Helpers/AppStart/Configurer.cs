﻿using System.Text.Json;
using Contract.Helpers;
using Contract.Helpers.AppExploration;
using Contract.Helpers.FeatureFlags;
using Contract.Helpers.Storage;
using Core.Helpers;
using Gateway.Services.AI.ChatBot;
using Gateway.Services.Background;
using Gateway.Services.Cache;
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
    public const string ENV_CACHE = "ENV_CACHE";
    public const string ENV_FEATURE_FLAG = "ENV_FEATURE_FLAG";

    private static ConfigurationManager? _configuration;
    public static bool IsRunningInContainer;

#pragma warning disable CS8618
    public static string[] CorsOrigins;
    public static AppInfoOptions AppInfoOptions;
    public static TokenOptions TokenOptions;

    public static ApiClientOptions ApiClientOptions;
    /*public static MongoOptions IdentityContextOptions;
    public static MongoOptions NotificationContextOptions;*/
    public static SqlServerOptions GatewayContextOptions;
    public static CloudStorageConfig CloudStorageConfig;
    public static Dependencies.Realtime.Options RealtimeOptions;

    public static BackgroundOptions BackgroundOptions;
    public static EmailOptions EmailOptions;
    public static OAuthOptions OAuthOptions;
    public static GeminiOptions GeminiOptions;
    public static CacheOptions CacheOptions;
    public static FeatureFlagOptions FeatureFlags;

    private static CookieConfigOptions _authCookieOptions;
#pragma warning restore CS8618






    public static void Init(ConfigurationManager configuration)
    {
        _configuration ??= configuration;
        IsRunningInContainer = bool.TryParse(Environment.GetEnvironmentVariable(DOTNET_RUNNING_IN_CONTAINER), out var inContainer) && inContainer;

        CorsOrigins = Get<string[]>("CORS", ENV_CORS)!;
        AppInfoOptions = Get<AppInfoOptions>("AppInfo", ENV_APP_INFO)!;
        TokenOptions = Get<TokenOptions>("JwtOptions")!;

        ApiClientOptions = Get<ApiClientOptions>("ServicePaths")!;
        GatewayContextOptions = Get<SqlServerOptions>("GatewayContext:SqlServer:Local", ENV_GATEWAY)!;
        CloudStorageConfig = Get<CloudStorageConfig>("External:Cloudinary")!;
        RealtimeOptions = Get<Dependencies.Realtime.Options>("Azure:SignalR")!;

        BackgroundOptions = Get<BackgroundOptions>("Hangfire:Remote")!;
        EmailOptions = Get<EmailOptions>("External:Gmail", ENV_EMAIL)!;
        OAuthOptions = Get<OAuthOptions>("External:OAuth:Google", ENV_OAUTH)!;
        GeminiOptions = Get<GeminiOptions>("External:AI:Gemini")!;
        CacheOptions = Get<CacheOptions>("External:Cache:Redis", ENV_CACHE)!;
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
