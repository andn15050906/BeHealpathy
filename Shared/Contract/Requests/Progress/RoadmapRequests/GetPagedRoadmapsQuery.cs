using Contract.Requests.Progress.RoadmapRequests.Dtos;
using Contract.Responses.Progress;

namespace Contract.Requests.Progress.RoadmapRequests;

public sealed class GetPagedRoadmapsQuery : IRequest<Result<PagedResult<RoadmapModel>>>
{
    public QueryRoadmapDto Rq { get; init; }



    public GetPagedRoadmapsQuery(QueryRoadmapDto rq)
    {
        Rq = rq;
    }
}