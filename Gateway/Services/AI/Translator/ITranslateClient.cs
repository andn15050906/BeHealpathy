namespace Gateway.Services.AI.Translator;

public interface ITranslateClient
{
    Task<string> Translate(string content, string sourceLang, string destinationLang);
}