using Contract.Requests.Identity.ActivityLogRequests.Dtos;
using Contract.Responses.Identity;

namespace Contract.Requests.Identity.ActivityLogRequests;

public sealed class GetPagedActivityLogsQuery : IRequest<Result<PagedResult<ActivityLogModel>>>
{
    public QueryActivityLogDto Rq { get; init; }
    public Guid UserId { get; init; }



    public GetPagedActivityLogsQuery(QueryActivityLogDto rq, Guid userId)
    {
        Rq = rq;
        UserId = userId;
    }
}