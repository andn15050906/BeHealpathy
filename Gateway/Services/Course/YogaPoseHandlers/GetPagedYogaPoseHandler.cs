using Contract.Domain.CourseAggregate;
using Contract.Helpers;
using Contract.Requests.Courses.CourseRequests;
using Contract.Requests.Courses.CourseRequests.Dtos;
using Contract.Responses.Courses;
using System.Linq.Expressions;

namespace Gateway.Services.Course.YogaPoseHandlers;

public sealed class GetPagedYogaPoseHandler : RequestHandler<GetPagedYogaPoseQuery, PagedResult<YogaPoseModel>, HealpathyContext>
{
    public GetPagedYogaPoseHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result<PagedResult<YogaPoseModel>>> Handle(GetPagedYogaPoseQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.GetPagingQuery(
                YogaPoseModel.MapExpression,
                GetPredicate(request.Rq),
                request.Rq.PageIndex,
                request.Rq.PageSize,
                false
            );
            var result = await query.ExecuteWithOrderBy(_ => _.LastModificationTime, ascending: false);

            return ToQueryResult(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private Expression<Func<YogaPose, bool>>? GetPredicate(QueryYogaPoseDto dto)
    {
        if (dto.Id is not null)
            return _ => _.Id == dto.Id;
        if (dto.Name is not null)
            return _ => _.Name == dto.Name;
        if (dto.Level is not null)
            return _ => _.Level == dto.Level;
        return _ => true;
    }
}