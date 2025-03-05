using Contract.IntegrationEvents;
using Contract.Requests.Identity.ActivityLogRequests.Dtos;

namespace Contract.Services.Contracts;

public interface IEventCache
{
    /// <summary>
    /// Do not await this method
    /// </summary>
    void Add<T>(Guid userId, T content) where T : BaseIntegrationEvent;
    Task<List<CreateActivityLogDto<T>>?> GetList<T>(Guid userId) where T : BaseIntegrationEvent;
    void Delete<T>(Guid userId) where T : BaseIntegrationEvent;


    /// <summary>
    /// Do not await this method
    /// </summary>
    void AddCommon<T>(T content) where T : BaseIntegrationEvent;
    Task<List<CreateActivityLogDto<T>>?> GetCommonList<T>() where T : BaseIntegrationEvent;
    void DeleteCommon<T>() where T : BaseIntegrationEvent;

    Task Flush();
}