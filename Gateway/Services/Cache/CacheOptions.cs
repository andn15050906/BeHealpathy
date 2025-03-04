namespace Gateway.Services.Cache;

public sealed class CacheOptions
{
    public List<CacheEndpoint> Endpoints { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
}

public sealed class CacheEndpoint
{
    public string Url { get; set; }
    public int Port { get; set; }
}