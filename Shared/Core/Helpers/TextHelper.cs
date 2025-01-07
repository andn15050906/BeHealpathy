using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;

namespace Core.Helpers;

public sealed class TextHelper
{
    public static string Normalize(string original)
    {
        string normalized = original.Trim();

        // from Space to Slash in ASCII
        for (byte i = 32; i < 48; i++)
            normalized = normalized.Replace(((char)i).ToString(), " ");

        normalized = normalized.Normalize(NormalizationForm.FormD);

        Regex regex = new(@"\p{IsCombiningDiacriticalMarks}+");
        normalized = regex.Replace(normalized, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
        while (normalized.Contains('?', StringComparison.CurrentCulture))
        {
            normalized = normalized.Remove(normalized.IndexOf("?"), 1);
        }

        return normalized.Replace("--", "-").ToLower();
    }

    internal static string GenerateAlphanumeric(byte length)
    {
        string charList = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        Random rd = new();
        StringBuilder builder = new();
        byte i;

        for (i = 0; i < 6; i++)
            builder.Append(charList[rd.Next(charList.Length)]);
        return builder.ToString();
    }






    private const string PATTERN_GUID = @"\b[0-9A-Fa-f]{8}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{12}\b";

    public static List<Guid> GetGuidsFromString(string input, int count)
    {
        List<Guid> guids = new();

        MatchCollection matches = Regex.Matches(input, PATTERN_GUID);

        foreach (Match match in matches.Cast<Match>())
        {
            if (Guid.TryParse(match.Value, out Guid result))
            {
                guids.Add(result);
                if (guids.Count == count)
                    break;
            }
        }
        return guids;
    }

    public static Guid? GetFirstGuidFromString(string input)
    {
        Match match = Regex.Match(input, PATTERN_GUID);

        if (match.Success && Guid.TryParse(match.Value, out Guid result))
            return result;
        return null;
    }






    /// <summary>
    /// Uses the SHA256 algorithm
    /// </summary>
    public static string Hash(string input)
    {
        using SHA256 sha256Hash = SHA256.Create();
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        StringBuilder builder = new();
        for (int i = 0; i < bytes.Length; i++)
            builder.Append(bytes[i].ToString("x2"));
        return builder.ToString();
    }
}