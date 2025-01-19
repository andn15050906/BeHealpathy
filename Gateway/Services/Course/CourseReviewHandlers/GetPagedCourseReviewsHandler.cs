using System.Linq.Expressions;
using Contract.Domain.Shared.ReviewBase;
using Contract.Helpers;
using Contract.Requests.Library.CourseReviewRequests;
using Contract.Requests.Shared.BaseDtos.Reviews;
using Contract.Responses.Shared;

namespace Gateway.Services.Course.CourseReviewHandlers;

public sealed class GetPagedCourseReviewsHandler : RequestHandler<GetPagedCourseReviewsQuery, PagedResult<ReviewModel>, HealpathyContext>
{
    public GetPagedCourseReviewsHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }

    public override async Task<Result<PagedResult<ReviewModel>>> Handle(GetPagedCourseReviewsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.GetPagingQuery(
                ReviewModel.MapExpression,
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

    private Expression<Func<Review, bool>>? GetPredicate(QueryReviewDto dto)
    {
        if (dto.SourceId is not null)
            return _ => _.SourceId == dto.SourceId && !_.IsDeleted; ;
        if (dto.CreatorId is not null)
            return _ => _.CreatorId == dto.CreatorId && !_.IsDeleted;;
        return _ => !_.IsDeleted;
    }
}