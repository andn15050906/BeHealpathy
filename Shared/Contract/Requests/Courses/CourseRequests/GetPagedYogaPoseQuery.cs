using Contract.Requests.Courses.CourseRequests.Dtos;
using Contract.Requests.Courses.LectureRequests.Dtos;
using Contract.Responses.Courses;

namespace Contract.Requests.Courses.CourseRequests;

public sealed class GetPagedYogaPoseQuery : IRequest<Result<PagedResult<YogaPoseModel>>>
{
    public QueryYogaPoseDto Rq { get; init; }
    public Guid UserId { get; init; }

    public GetPagedYogaPoseQuery(QueryYogaPoseDto rq, Guid userId)
    {
        Rq = rq;
        UserId = userId;
    }
}
