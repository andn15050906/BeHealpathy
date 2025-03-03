using System.Text.Json;

namespace Gateway.Services.AI.Translator;

public class TranslateClient : ITranslateClient
{
    private const string BASE_URL = "https://translate.googleapis.com/translate_a/single";

    public async Task<string> Translate(string content, string sourceLang, string destinationLang)
    {
        using var client = new HttpClient();

        var url = string.Format("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}", sourceLang, destinationLang, content);
        var response = await client.GetStringAsync(url);

        var obj = JsonSerializer.Deserialize<dynamic>(response);
        obj = JsonSerializer.Deserialize<dynamic>(obj is null ? string.Empty : obj[0].ToString());

        int i;
        string result = string.Empty;
        for (i = 0; i < obj.GetArrayLength(); i++)
            result += obj[i][0];
        return result;
    }
}