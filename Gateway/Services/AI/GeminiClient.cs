using Contract.Helpers;
using GenerativeAI.Methods;
using GenerativeAI.Models;

namespace Gateway.Services.AI;

public sealed class GeminiClient : IChatbotClient
{
    public const string ERR_EXCEED_QUOTA = "Exceed quota";

#pragma warning disable CS8618
    public static GeminiClient Instance { get; private set; }

    private static GenerativeModel _generativeModel;
    private static ChatSession ChatSession;                                                     //... static?
    private static IAppLogger _logger;
#pragma warning restore CS8618

    public GeminiClient(GeminiOptions options, IAppLogger logger)
    {
        _generativeModel ??= new Gemini15Flash(options.Key);
        ChatSession ??= _generativeModel.StartChat(new GenerativeAI.Types.StartChatParams());   //...
        _logger = logger;

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
        try
        {
            var response = await ChatSession.SendMessageAsync(message);
            return new Result<string>(200) { Data = response ?? string.Empty };
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return new Result<string>(500);
        }
    }
}