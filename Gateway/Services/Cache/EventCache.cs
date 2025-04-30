using System.Text.Json;
using Contract.BusinessRules;
using Contract.Helpers.FeatureFlags;
using Contract.Requests.Identity.ActivityLogRequests.Dtos;
using Core.Helpers;
using Microsoft.Extensions.Options;

namespace Gateway.Services.Cache;

public sealed class EventCache : IEventCache
{
    private readonly ICacheBase _cache;
    private readonly HealpathyContext _context;
    private readonly bool _forceUpdateCache;

    private const string JSON_EMPTY_ARRAY = "[]";

    public EventCache(ICacheBase cache, HealpathyContext context, IOptions<FeatureFlagOptions> flags)
    {
        _cache = cache;
        _context = context;
        _forceUpdateCache = flags.Value.ForceUpdateCache;
    }



    public void Add<T>(Guid userId, T content)
        where T : BaseIntegrationEvent
    {
        try
        {
            if (!_forceUpdateCache)
            {
                Task.Run(async () =>
                {
                    var eventType = typeof(T).Name;
                    List<CreateActivityLogDto<T>> batch = await GetList<T>(userId) ?? [];

                    // if Cache is full or old, Trim the cache and Save to the database
                    if (batch.Count > 1000)
                    {
                        await SaveAndTrimCache(batch, userId);
                    }
                    else
                    {
                        var firstUpdate = batch.FirstOrDefault()?.CreationTime ?? TimeHelper.Now;
                        if (firstUpdate.AddHours(1) <= TimeHelper.Now)
                            await SaveAndTrimCache(batch, userId);
                    }

                    batch.Add(new CreateActivityLogDto<T> { Content = JsonSerializer.Serialize(content), CreationTime = TimeHelper.Now });
                    await _cache.Set($"{eventType}_{userId}", JsonSerializer.Serialize(batch));
                });
            }
            else
            {
                var eventType = typeof(T).Name;
                List<CreateActivityLogDto<T>> batch = GetList<T>(userId).Result ?? [];

                // Commented this out for showing redis entries
                //batch.Add(new CreateActivityLogDto<T> { Content = JsonSerializer.Serialize(content), CreationTime = TimeHelper.Now });
                //_cache.Set($"{eventType}", JsonSerializer.Serialize(batch)).Wait();

                SaveAndTrimCache(batch, userId).Wait();
                batch.Add(new CreateActivityLogDto<T> { Content = JsonSerializer.Serialize(content), CreationTime = TimeHelper.Now });
                _cache.Set($"{eventType}_{userId}", JsonSerializer.Serialize(batch)).Wait();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }

    public async Task<List<CreateActivityLogDto<T>>?> GetList<T>(Guid userId)
        where T : BaseIntegrationEvent
    {
        var eventType = typeof(T).Name;

        return JsonSerializer.Deserialize<List<CreateActivityLogDto<T>>>(
            await _cache.Get($"{eventType}_{userId}") ?? JSON_EMPTY_ARRAY
        );
    }

    public void Delete<T>(Guid userId)
        where T : BaseIntegrationEvent
    {
        var eventType = typeof(T).Name;

        Task.Run(async () =>
        {
            await _cache.Delete($"{eventType}_{userId}");
        });
    }



    public void AddCommon<T>(T content)
        where T : BaseIntegrationEvent
    {
        try
        {
            if (!_forceUpdateCache)
            {
                Task.Run(async () =>
                {
                    var eventType = typeof(T).Name;
                    List<CreateActivityLogDto<T>> batch = await GetCommonList<T>() ?? [];

                    // if Cache is full or old, Trim the cache and Save to the database
                    if (batch.Count > 1000)
                    {
                        await SaveAndTrimCache(batch);
                    }
                    else
                    {
                        var firstUpdate = batch.FirstOrDefault()?.CreationTime ?? TimeHelper.Now;
                        if (firstUpdate.AddHours(1) <= TimeHelper.Now)
                            await SaveAndTrimCache(batch);
                    }

                    batch.Add(new CreateActivityLogDto<T> { Content = JsonSerializer.Serialize(content), CreationTime = TimeHelper.Now });
                    await _cache.Set($"{eventType}", JsonSerializer.Serialize(batch));
                });
            }
            else
            {
                var eventType = typeof(T).Name;
                List<CreateActivityLogDto<T>> batch = GetCommonList<T>().Result ?? [];
                SaveAndTrimCache(batch).Wait();
                batch.Add(new CreateActivityLogDto<T> { Content = JsonSerializer.Serialize(content), CreationTime = TimeHelper.Now });
                _cache.Set($"{eventType}", JsonSerializer.Serialize(batch)).Wait();
            }
        }
        catch (Exception) { }
    }

    public async Task<List<CreateActivityLogDto<T>>?> GetCommonList<T>()
        where T : BaseIntegrationEvent
    {
        var eventType = typeof(T).Name;

        return JsonSerializer.Deserialize<List<CreateActivityLogDto<T>>>(
            await _cache.Get($"{eventType}") ?? JSON_EMPTY_ARRAY
        );
    }

    public void DeleteCommon<T>()
        where T : BaseIntegrationEvent
    {
        var eventType = typeof(T).Name;

        Task.Run(async () =>
        {
            await _cache.Delete($"{eventType}");
        });
    }

    public async Task Flush()
    {
        await _cache.Flush();
    }

    private async Task<Result> SaveAndTrimCache<T>(List<CreateActivityLogDto<T>> batch, Guid? userId = null)
        where T : BaseIntegrationEvent
    {
        if (batch is null || batch.Count == 0)
            return new Result(400);

        var dtos = batch.Select(_ => new CreateActivityLogDto<T>()
        {
            Content = _.Content,
            CreationTime = TimeHelper.Now
        }).ToList();
        var eventType = batch.First().GenericType;

        try
        {
            await _context.AddRangeAsync(
                dtos.Select(_ => new ActivityLog(
                        Guid.NewGuid(),
                        userId ?? PreSet.SystemUserId,
                        _.CreationTime ?? TimeHelper.Now,
                        JsonSerializer.Serialize(new { _.Content, _.GenericType })
                    )
                )
            );

            var dbTask = _context.SaveChangesAsync();
            var deleteTask = _cache.Delete(userId is not null ? $"{eventType}_{userId}" : $"{eventType}");
            await Task.WhenAll(dbTask, deleteTask);
            batch.Clear();

            return new(201);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            return new(500);
        }
    }
}