using Contract.Domain.ToolAggregate;
using Contract.Helpers;
using Contract.Requests.Progress.RoutineLogRequests;
using Contract.Requests.Progress.RoutineLogRequests.Dtos;
using Contract.Responses.Progress;
using System.Linq.Expressions;

namespace Gateway.Services.Progress.RoutineLogHandlers;

public sealed class GetPagedRoutineLogsHandler : RequestHandler<GetPagedRoutineLogsQuery, PagedResult<RoutineLogModel>, HealpathyContext>
{
    public GetPagedRoutineLogsHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result<PagedResult<RoutineLogModel>>> Handle(GetPagedRoutineLogsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.GetPagingQuery(
                RoutineLogModel.MapExpression,
                GetPredicate(request.Rq),
                request.Rq.PageIndex,
                request.Rq.PageSize,
                false
            );
            var result = await query.ExecuteWithOrderBy(_ => _.CreationTime);
            return ToQueryResult(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private Expression<Func<RoutineLog, bool>>? GetPredicate(QueryRoutineLogDto dto)
    {
        if (dto.RoutineId is not null)
            return _ => _.RoutineId == dto.RoutineId && !_.IsDeleted;
        return _ => !_.IsDeleted;
    }
}
