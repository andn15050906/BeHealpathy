namespace Contract.Services.Contracts;

public interface ICacheBase
{
    Task Set(string key, string value);
    Task<string?> Get(string key);
    Task Delete(string key);
    Task Flush();
}