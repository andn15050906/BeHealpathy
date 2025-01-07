namespace Contract.Helpers;

public sealed class TokenOptions
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public short Lifetime { get; set; }               //in seconds
}
