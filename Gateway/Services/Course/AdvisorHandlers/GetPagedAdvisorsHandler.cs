using System.Linq.Expressions;
using Contract.Domain.CourseAggregate;
using Contract.Helpers;
using Contract.Requests.Courses.AdvisorRequests;
using Contract.Requests.Courses.AdvisorRequests.Dtos;
using Contract.Responses.Courses;

namespace Gateway.Services.Course.AdvisorHandlers;

public class GetPagedAdvisorsHandler : RequestHandler<GetPagedAdvisorsQuery, PagedResult<AdvisorModel>, HealpathyContext>
{
    public GetPagedAdvisorsHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }

    public override async Task<Result<PagedResult<AdvisorModel>>> Handle(GetPagedAdvisorsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.GetPagingQuery(
                AdvisorModel.MapExpression,
                GetPredicate(request.Rq),
                request.Rq.PageIndex,
                request.Rq.PageSize
            );
            var result = await query.ExecuteWithOrderBy(_ => _.LastModifierId);
            return ToQueryResult(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private Expression<Func<Advisor, bool>>? GetPredicate(QueryAdvisorDto dto)
    {
        if (dto.UserId is not null)
            return _ => _.CreatorId == dto.UserId;
        return _ => true;
    }
}
