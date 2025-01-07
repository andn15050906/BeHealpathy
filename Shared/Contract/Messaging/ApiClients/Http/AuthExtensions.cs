using Contract.Helpers;
using Contract.Services.Contracts;
using Contract.Services.Implementations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;

namespace Contract.Messaging.ApiClients.Http;

public static class AuthExtensions
{
    private const string SCHEME = "Scheme";

    public static IServiceCollection AddAuthServices(
        this IServiceCollection services, TokenOptions tokenOptions, OAuthOptions? oAuthOptions = null)
    {
        services.Configure<TokenOptions>(options =>
        {
            options.Issuer = tokenOptions.Issuer;
            options.Audience = tokenOptions.Audience;
            options.Secret = tokenOptions.Secret;
            options.Lifetime = tokenOptions.Lifetime;
        });

        services.AddTransient<ITokenService, TokenService>();

        var builder = services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultScheme = SCHEME;
            })
            .AddPolicyScheme(SCHEME, SCHEME, options =>
            {
                options.ForwardDefaultSelector = context =>
                {
                    //if (context.Request.Path.StartsWithSegments("/hub"))
                    //    return JwtBearerDefaults.AuthenticationScheme;

                    if ((context.Request.Path.Value ?? string.Empty).Contains("signin-google", StringComparison.CurrentCultureIgnoreCase))
                        return CookieAuthenticationDefaults.AuthenticationScheme;

                    if (context.Request.Headers.TryGetValue("Authorization", out _))
                        return JwtBearerDefaults.AuthenticationScheme;

                    return context.Request.Cookies[HttpContextExtensions.BEARER] is not null
                        ? JwtBearerDefaults.AuthenticationScheme
                        : CookieAuthenticationDefaults.AuthenticationScheme;
                };
            })
            .AddJwtBearer(GetOptions(tokenOptions))
            .AddCookie(options =>
            {
                //options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
                options.Cookie.IsEssential = true;
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return Task.CompletedTask;
                };
            });

        if (oAuthOptions is not null)
        {
            builder
                .AddGoogle(options =>
                {
                    options.ClientId = oAuthOptions.ClientId;
                    options.ClientSecret = oAuthOptions.ClientSecret;

                    options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
                    //options.ClaimActions.MapJsonKey("urn:google:locale", "locale", "string");
                });
        }

        return services;
    }

    private static Action<JwtBearerOptions> GetOptions(TokenOptions config) => options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = config.Audience,
            ValidIssuer = config.Issuer,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Secret))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies[HttpContextExtensions.BEARER];
                if (string.IsNullOrWhiteSpace(context.Token))
                    context.Token = context.Request.Headers.Authorization.First()?.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
                return Task.CompletedTask;
            }
        };
    };
}