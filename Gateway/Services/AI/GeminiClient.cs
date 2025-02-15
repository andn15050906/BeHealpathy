using GenerativeAI.Methods;
using GenerativeAI.Models;

namespace Gateway.Services.AI;

public sealed class GeminiClient : IChatbotClient
{
    public const string ERR_EXCEED_QUOTA = "Exceed quota";

#pragma warning disable CS8618
    public static GeminiClient Instance { get; private set; }

    private static GenerativeModel GenerativeModel;

    // static?
    private static ChatSession ChatSession;
#pragma warning restore CS8618

    public GeminiClient(GeminiOptions options)
    {
        GenerativeModel ??= new GenerativeModel(options.Key);
        // ?
        ChatSession ??= GenerativeModel.StartChat(new GenerativeAI.Types.StartChatParams());

        Instance ??= this;
    }



    public async Task<Result<string>> Prompt(IEnumerable<string> context)
    {
        if (!context.Any())
            return new Result<string>(400, string.Empty);

        //...
        return await Prompt(context.Last());
    }

    public async Task<Result<string>> Prompt(string message)
    {
        var response = await ChatSession.SendMessageAsync(message);
        return new Result<string>(200) { Data = response ?? string.Empty };
    }
}