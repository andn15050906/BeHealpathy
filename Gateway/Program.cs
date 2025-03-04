using Serilog;
using Microsoft.EntityFrameworkCore;
using Contract.Helpers.Storage;
using Contract.Services.Implementations;
using Contract.Messaging.ApiClients.Http;
using Infrastructure.MessageQueue;
using Gateway.Helpers.AppStart;
using Gateway.Services.Microservices;
using Infrastructure.Helpers.Monitoring;
using Contract.Helpers.AppExploration;
using Infrastructure.Helpers.Email;
using Contract.Helpers.FeatureFlags;
using Gateway.Helpers.Dependencies;
using WisNet.Gateway.Realtime.Interface;
using Gateway.Services.AI;
using Gateway.Services.Cache;

const string POLICY = "Policy";
var builder = WebApplication.CreateBuilder(args);

OneTimeRunner.InitConfig(builder);
builder.Services
    .AddCors(_ => _.AddPolicy(POLICY, builder =>
        builder.WithOrigins(Configurer.CorsOrigins).AllowCredentials().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("WWW-Authenticate"))
    )
    .AddMonitoring()
    .AddAppExploration(Configurer.AppInfoOptions)
    .AddMediatR(options => { options.RegisterServicesFromAssembly(typeof(Program).Assembly); })
    .AddDbContextPool<HealpathyContext>(_ => _.UseSqlServer(Configurer.GatewayContextOptions.ConnectionString))
    .AddAuthServices(Configurer.TokenOptions, Configurer.OAuthOptions)
    //AddMQPublisher(Configurer.IsRunningInContainer)
    .AddMicroservices()
    .AddEmailService(Configurer.EmailOptions)
    .AddAI(Configurer.GeminiOptions)
    //.AddPaymentService
    .AddCloudStorage<CloudStorageService>(Configurer.CloudStorageConfig)
    .AddCache(Configurer.CacheOptions)
    .AddFeatureFlags(Configurer.FeatureFlags)
    .AddHttpContextAccessor()
    .AddRealtimeService()
    .AddControllers();

var app = builder.Build();

app.UseAppExploration();

app
    .UseSerilogRequestLogging()
    .UseHttpsRedirection()
    .UseCors(POLICY)
    .UseAuthentication()
    .UseAuthorization();

app.MapControllers();
app.MapHub<AppHub>("/hub");

app.RunWarmUpQuery();

app.Run();

public partial class Program { }