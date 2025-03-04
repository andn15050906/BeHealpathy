using Contract.Helpers;
using StackExchange.Redis;

namespace Gateway.Services.Cache;

public sealed class CacheService : ICacheService
{
    public static CacheService Instance { get; private set; }
    private IDatabase _database;
    private IAppLogger _logger;



    public CacheService(CacheOptions options, IAppLogger logger)
    {
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(
            new ConfigurationOptions
            {
                EndPoints = { { options.Endpoints.First().Url, options.Endpoints.First().Port } },
                User = options.User,
                Password = options.Password
            }
        );

        _database = redis.GetDatabase();
        _logger = logger;
        Instance ??= this;
    }

    public async Task Set(string key, string value)
    {
        await _database.StringSetAsync(key, value);
    }

    public async Task<string?> Get(string key)
    {
        return await _database.StringGetAsync(key);
    }

    public async Task Delete(string key)
    {
        await _database.KeyDeleteAsync(key);
    }

    public async Task Flush()
    {
        await _database.ExecuteAsync("FLUSHDB");
    }
}