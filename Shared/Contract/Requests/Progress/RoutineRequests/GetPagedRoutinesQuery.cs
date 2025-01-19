using Contract.Requests.Progress.RoutineRequests.Dtos;
using Contract.Responses.Progress;

namespace Contract.Requests.Progress.RoutineRequests;

public sealed class GetPagedRoutinesQuery : IRequest<Result<PagedResult<RoutineModel>>>
{
    public QueryRoutineDto Rq { get; init; }
    public Guid UserId { get; init; }



    public GetPagedRoutinesQuery(QueryRoutineDto rq, Guid userId)
    {
        Rq = rq;
        UserId = userId;
    }
}
