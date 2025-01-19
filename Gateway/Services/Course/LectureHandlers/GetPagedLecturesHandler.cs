using System.Linq.Expressions;
using Contract.Domain.CourseAggregate;
using Contract.Helpers;
using Contract.Requests.Courses.LectureRequests;
using Contract.Requests.Courses.LectureRequests.Dtos;
using Contract.Responses.Courses;

namespace Gateway.Services.Course.LectureHandlers;

public sealed class GetPagedLecturesHandler : RequestHandler<GetPagedLecturesQuery, PagedResult<LectureModel>, HealpathyContext>
{
    public GetPagedLecturesHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }

    public override async Task<Result<PagedResult<LectureModel>>> Handle(GetPagedLecturesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.GetPagingQuery(
                LectureModel.MapExpression,
                GetPredicate(request.Rq),
                request.Rq.PageIndex,
                request.Rq.PageSize
            );

            var result = await query.ExecuteWithOrderBy(_ => _.LastModificationTime, ascending: false);

            return ToQueryResult(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private Expression<Func<Lecture, bool>>? GetPredicate(QueryLectureDto dto)
    {
        // GetLectureById
        if (dto.Id is not null)
            return _ => _.Id == dto.Id;
        if (dto.CourseId is not null)
            return _ => _.CourseId == (Guid)dto.CourseId;

        /*if (dto.SectionId is not null)
            return _ => _.SectionId == dto.SectionId;*/
        return _ => true;
    }
}