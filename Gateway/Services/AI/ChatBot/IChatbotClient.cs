namespace Gateway.Services.AI.ChatBot;

public interface IChatbotClient
{
    Task<Result<string>> Prompt(IEnumerable<string> context);
    Task<Result<string>> Prompt(string message);
}