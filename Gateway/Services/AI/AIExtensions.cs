using Infrastructure.Helpers.Monitoring;

namespace Gateway.Services.AI;

public static class AIExtensions
{
    public static IServiceCollection AddAI(this IServiceCollection services, GeminiOptions geminiOptions)
    {
        var geminiClient = new GeminiClient(geminiOptions, new Logger());
        services.AddSingleton<IChatbotClient>(geminiClient);
        return services;
    }
}