using Contract.Domain.ProgressAggregates;
using Contract.Helpers;
using Contract.Requests.Progress.RoutineRequests;
using Contract.Requests.Progress.RoutineRequests.Dtos;
using Contract.Responses.Progress;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Gateway.Services.Progress.RoutineHandlers;

public sealed class GetPagedRoutinesHandler : RequestHandler<GetPagedRoutinesQuery, PagedResult<RoutineModel>, HealpathyContext>
{
    public GetPagedRoutinesHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result<PagedResult<RoutineModel>>> Handle(GetPagedRoutinesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.GetPagingQuery(
                RoutineModel.MapExpression,
                GetPredicate(request.Rq),
                request.Rq.PageIndex,
                byte.MaxValue,
                false
            );
            var result = await query.ExecuteWithOrderBy(_ => _.LastModificationTime);

            var curr = DateTime.UtcNow.ToLocalTime();

            var expiredRoutines = await _context.Routines
                    .Where(r => !r.IsDeleted && !r.IsClosed && !r.IsCompleted && r.EndDate < curr && r.CreatorId == request.Rq.CreatorId)
                    .ToListAsync();

            if (expiredRoutines is not null)
            {
                foreach (var routine in expiredRoutines)
                {
                    routine.IsClosed = true;
                }

                await _context.SaveChangesAsync();
            }

            return ToQueryResult(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private Expression<Func<Routine, bool>>? GetPredicate(QueryRoutineDto dto)
    {
        if (dto.CreatorId is not null)
        {
            if (dto.From is not null && dto.To is not null)
                return _ => _.CreatorId == dto.CreatorId && !_.IsDeleted && _.StartDate > dto.From && _.EndDate < dto.To;
            return _ => _.CreatorId == dto.CreatorId && !_.IsDeleted;
        }

        if (dto.From is not null && dto.To is not null)
            return _ => _.StartDate > dto.From && _.EndDate < dto.To && !_.IsDeleted;
        if (dto.Title is not null)
            return _ => _.Title.Contains(_.Title) && !_.IsDeleted;

        return _ => !_.IsDeleted;
    }
}
