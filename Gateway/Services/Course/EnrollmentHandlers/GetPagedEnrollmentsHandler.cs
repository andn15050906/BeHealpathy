using System.Linq.Expressions;
using Contract.Domain.CourseAggregate;
using Contract.Helpers;
using Contract.Requests.Courses.EnrollmentRequests;
using Contract.Responses.Courses;

namespace Gateway.Services.Course.EnrollmentHandlers;

public class GetPagedEnrollmentsHandler : RequestHandler<GetPagedEnrollmentsQuery, PagedResult<CourseProgressModel>, HealpathyContext>
{
    public GetPagedEnrollmentsHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }

    public override async Task<Result<PagedResult<CourseProgressModel>>> Handle(GetPagedEnrollmentsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.GetPagingQuery(
                CourseProgressModel.MapExpression,
                GetPredicate(request),
                request.Rq.PageIndex,
                request.Rq.PageSize
            );

            PagedResult<CourseProgressModel> result = await query.ExecuteWithOrderBy(_ => _.CreationTime);

            return ToQueryResult(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private Expression<Func<CourseProgress, bool>>? GetPredicate(GetPagedEnrollmentsQuery query)
    {
        if (query.Rq.CourseId is not null)
            return _ => _.CourseId == query.Rq.CourseId && !_.IsDeleted;
        if (query.Rq.IsGetEnrolledCourse == true)
            return _ => _.CreatorId == query.UserId && !_.IsDeleted;
        return _ => true;
    }
}
