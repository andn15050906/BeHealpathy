using Contract.Domain.CourseAggregate;
using Contract.Helpers;
using Contract.Requests.Courses.CourseRequests;
using Contract.Requests.Courses.CourseRequests.Dtos;
using Contract.Responses.Courses;
using Core.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Courses.Services.Courses;

public sealed class GetPagedCoursesHandler : RequestHandler<GetPagedCoursesQuery, PagedResult<CourseOverviewModel>, HealpathyContext>
{
    private const int TREND_MONTHS = 6;

    public GetPagedCoursesHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result<PagedResult<CourseOverviewModel>>> Handle(GetPagedCoursesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Rq.ByTrend)
                return await HandleTrendingQuery(request);

            var query = _context.GetPagingQuery(
                CourseOverviewModel.MapExpression,
                GetPredicate(request.Rq),
                request.Rq.PageIndex,
                request.Rq.PageSize
            );

            PagedResult<CourseOverviewModel> result;
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

    private Expression<Func<Course, object?>>[]? GetIncludedProperties(QueryCourseDto dto)
    {
        //if (dto.CategoryId is not null)
        //    return new Expression<Func<Course, object?>>[] { _ => _.LeafCategory };
        return null;
    }

    private async Task<Result<PagedResult<CourseOverviewModel>>> HandleTrendingQuery(GetPagedCoursesQuery request)
    {
        var unorderedQuery = _context.Enrollments
            .Where(_ => _.CreationTime >= TimeHelper.Now.AddMonths(-TREND_MONTHS));
        var total = await unorderedQuery.CountAsync();

        var items = await unorderedQuery
            .OrderByDescending(_ => _.CreationTime)
            .Join(_context.Courses, enrollment => enrollment.CourseId, course => course.Id, (enrollment, _) =>
                new CourseOverviewModel
                {
                    Id = _.Id,
                    CreatorId = _.CreatorId,
                    LastModifierId = _.LastModifierId,
                    CreationTime = _.CreationTime,
                    LastModificationTime = _.LastModificationTime,

                    Title = _.Title,
                    ThumbUrl = _.ThumbUrl,
                    //public string Intro { get; set; },
                    //public string Description { get; set; },
                    Status = _.Status,
                    Price = _.Price,
                    Discount = _.Discount,
                    DiscountExpiry = _.DiscountExpiry,
                    Level = _.Level,
                    //public string Outcomes { get; set; },
                    //public string Requirements { get; set; },
                    LectureCount = _.LectureCount,
                    LearnerCount = _.LearnerCount,
                    RatingCount = _.RatingCount,
                    TotalRating = _.TotalRating,
                    LeafCategoryId = _.LeafCategoryId,
                    InstructorId = _.InstructorId,
                    //public UserMinModel Creator { get; set; }
                }
            )
            .Skip(request.Rq.PageIndex * request.Rq.PageSize)
            .Take(request.Rq.PageSize)
            .ToListAsync();

        var result = new PagedResult<CourseOverviewModel>(total, request.Rq.PageSize, (byte)request.Rq.PageIndex, items);
        return ToQueryResult(result);
    }

    /*public override async Task<Result<PagedResult<CourseOverviewModel>>> Handle(GetPagedCoursesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            Expression<Func<Course, object>> orderBy;
            bool isAscending = false;

            if (request.Rq.ByPrice)
            {
                orderBy = _ => _.Price;
                isAscending = true;
            }
            else if (request.Rq.ByDiscount)
                orderBy = _ => _.Discount;
            else if (request.Rq.ByLearnerCount)
                orderBy = _ => _.LearnerCount;
            else if (request.Rq.ByAvgRating)
                orderBy = _ => _.TotalRating / _.RatingCount;
            else
                orderBy = _ => _.LastModificationTime;

            var result = await _context.FindPaged(
                Builders<Course>.Filter.Empty,
                Builders<Course>.Projection.Expression(CourseOverviewModel.MapExpression),
                orderBy, isAscending, request.Rq.PageIndex, request.Rq.PageSize);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }*/
}
