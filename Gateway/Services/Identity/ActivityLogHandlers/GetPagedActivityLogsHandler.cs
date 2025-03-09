using Contract.Helpers;
using Contract.Requests.Identity.ActivityLogRequests;
using Contract.Requests.Identity.ActivityLogRequests.Dtos;
using Contract.Responses.Identity;
using System.Linq.Expressions;

namespace Gateway.Services.Identity.ActivityLogHandlers;

public sealed class GetPagedActivityLogsHandler : RequestHandler<GetPagedActivityLogsQuery, PagedResult<ActivityLogModel>, HealpathyContext>
{
    public GetPagedActivityLogsHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result<PagedResult<ActivityLogModel>>> Handle(GetPagedActivityLogsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.GetPagingQuery(
                ActivityLogModel.MapExpression,
                GetPredicate(request.Rq),
                request.Rq.PageIndex,
                request.Rq.PageSize,
                false
            );
            var result = await query.ExecuteWithOrderBy(_ => _.CreationTime, ascending: false);
            return ToQueryResult(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private Expression<Func<ActivityLog, bool>>? GetPredicate(QueryActivityLogDto dto)
    {
        if (dto.CreatorId is not null)
            return _ => _.CreatorId == dto.CreatorId;

        return null;
    }
}