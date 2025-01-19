using Contract.Requests.Courses.LectureRequests.Dtos;
using Contract.Responses.Courses;

namespace Contract.Requests.Courses.LectureRequests;

public sealed class GetPagedLecturesQuery : IRequest<Result<PagedResult<LectureModel>>>
{
    public QueryLectureDto Rq { get; init; }
    public Guid UserId { get; init; }



    public GetPagedLecturesQuery(QueryLectureDto rq, Guid userId)
    {
        Rq = rq;
        UserId = userId;
    }
}