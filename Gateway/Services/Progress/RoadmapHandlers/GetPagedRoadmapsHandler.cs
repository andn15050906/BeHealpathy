using Contract.Domain.ProgressAggregates;
using System.Linq.Expressions;
using Contract.Helpers;
using Contract.Requests.Progress.RoadmapRequests;
using Contract.Requests.Progress.RoadmapRequests.Dtos;
using Contract.Responses.Progress;

namespace Gateway.Services.Progress.RoadmapHandlers;

public class GetPagedRoadmapsHandler(HealpathyContext context, IAppLogger logger)
    : RequestHandler<GetPagedRoadmapsQuery, PagedResult<RoadmapModel>, HealpathyContext>(context, logger)
{
    public override async Task<Result<PagedResult<RoadmapModel>>> Handle(GetPagedRoadmapsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.GetPagingQuery(
                RoadmapModel.MapExpression,
                GetPredicate(request.Rq),
                request.Rq.PageIndex,
                request.Rq.PageSize,
                false
            );
            var result = await query.ExecuteWithOrderBy(_ => _.LastModificationTime);

            return ToQueryResult(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private Expression<Func<Roadmap, bool>>? GetPredicate(QueryRoadmapDto dto)
    {
        return _ => !_.IsDeleted;
    }
}