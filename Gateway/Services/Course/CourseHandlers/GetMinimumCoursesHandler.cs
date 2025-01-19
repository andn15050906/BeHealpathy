using System.Linq.Expressions;
using Contract.Domain.CourseAggregate;
using Contract.Helpers;
using Contract.Requests.Courses.CourseRequests;
using Contract.Requests.Courses.CourseRequests.Dtos;
using Contract.Responses.Courses;
using Core.Helpers;

namespace Courses.Services.Courses;

public sealed class GetMinimumCoursesHandler : RequestHandler<GetMinimumCoursesQuery, PagedResult<CourseMinModel>, HealpathyContext>
{
    public GetMinimumCoursesHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result<PagedResult<CourseMinModel>>> Handle(GetMinimumCoursesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.GetPagingQuery(
                CourseMinModel.MapExpression,
                GetPredicate(request.Rq),
                request.Rq.PageIndex,
                request.Rq.PageSize
            );

            PagedResult<CourseMinModel> result;
            if (request.Rq.ByPrice is true)
                result = await query.ExecuteWithOrderBy(_ => _.Price);
            else if (request.Rq.ByDiscount is true)
                result = await query.ExecuteWithOrderBy(_ => _.Discount, ascending: false);
            else if (request.Rq.ByLearnerCount is true)
                result = await query.ExecuteWithOrderBy(_ => _.LearnerCount, ascending: false);
            else if (request.Rq.ByAvgRating is true)
                result = await query.ExecuteWithOrderBy(_ => _.TotalRating / _.RatingCount, ascending: false, isAnsiWarningTransaction: true);
            else
                result = await query.ExecuteWithOrderBy(_ => _.LastModificationTime, ascending: false);

            return ToQueryResult(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private Expression<Func<Course, bool>>? GetPredicate(QueryCourseDto dto)
    {
        if (dto.Title is not null)
            return _ => _.MetaTitle.Contains(TextHelper.Normalize(dto.Title));
        if (dto.Status is not null)
            return _ => _.Status == dto.Status;
        if (dto.Level is not null)
            return _ => _.Level == dto.Level;
        if (dto.CategoryId is not null)
            return _ => _.LeafCategoryId == dto.CategoryId;
        if (dto.InstructorId is not null)
            return _ => _.InstructorId == dto.InstructorId;
        return null;
    }

    /*public override async Task<Result<PagedResult<CourseMinModel>>> Handle(GetMinimumCoursesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _context.FindPaged(
                Builders<Course>.Filter.Empty,
                Builders<Course>.Projection.Expression(CourseMinModel.MapExpression),
                _ => _.LastModificationTime, false, request.Rq.PageIndex, request.Rq.PageSize
            );

            return Ok(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }*/
}
