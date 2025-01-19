using Contract.Requests.Progress.RoutineLogRequests.Dtos;
using Contract.Responses.Progress;

namespace Contract.Requests.Progress.RoutineLogRequests;

public sealed class GetPagedRoutineLogsQuery : IRequest<Result<PagedResult<RoutineLogModel>>>
{
    public QueryRoutineLogDto Rq { get; init; }
    public Guid UserId { get; init; }



    public GetPagedRoutineLogsQuery(QueryRoutineLogDto rq, Guid userId)
    {
        Rq = rq;
        UserId = userId;
    }
}