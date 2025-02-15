namespace Gateway.Services.AI;

public interface IChatbotClient
{
    Task<Result<string>> Prompt(IEnumerable<string> context);
    Task<Result<string>> Prompt(string message);
}