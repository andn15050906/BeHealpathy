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

const string POLICY = "Policy";
var builder = WebApplication.CreateBuilder(args);

OneTimeRunner.InitConfig(builder);
builder.Services
    .AddCors(_ => _.AddPolicy(POLICY, builder =>
        builder.WithOrigins(Configurer.CorsOrigins).AllowCredentials().AllowAnyHeader().AllowAnyMethod())
    )
    .AddMonitoring()
    .AddAppExploration(Configurer.AppInfoOptions)
    .AddMediatR(options => { options.RegisterServicesFromAssembly(typeof(Program).Assembly); })
    .AddDbContextPool<HealpathyContext>(_ => _.UseSqlServer(Configurer.GatewayContextOptions.ConnectionString))
    .AddAuthServices(Configurer.TokenOptions, Configurer.OAuthOptions)
    //AddMQPublisher(Configurer.IsRunningInContainer)
    .AddMicroservices()
    .AddCloudStorage<CloudStorageService>(Configurer.CloudStorageConfig)
    .AddEmailService(Configurer.EmailOptions)
    .AddFeatureFlags(Configurer.FeatureFlags)
    //.AddPaymentService
    .AddHttpContextAccessor()
    //AddRealtimeService()
    .AddControllers();

var app = builder.Build();

//if (app.Environment.IsDevelopment())
    app.UseAppExploration();

app
    .UseSerilogRequestLogging()
    .UseHttpsRedirection()
    .UseCors(POLICY)
    .UseAuthentication()
    .UseAuthorization();

app.MapControllers();
//app.MapHub<AppHub>("/hub");

app.Run();

public partial class Program { }