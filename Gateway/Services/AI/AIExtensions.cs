namespace Gateway.Services.AI;

public static class AIExtensions
{
    public static IServiceCollection AddAI(this IServiceCollection services, GeminiOptions geminiOptions)
    {
        var geminiClient = new GeminiClient(geminiOptions);
        services.AddSingleton<IChatbotClient>(geminiClient);
        return services;
    }
}