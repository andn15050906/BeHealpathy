using Contract.Requests.Courses.CourseRequests.Dtos;
using Contract.Responses.Courses;

namespace Contract.Requests.Courses.CourseRequests;

public sealed class GetPagedCoursesQuery : IRequest<Result<PagedResult<CourseOverviewModel>>>
{
    public QueryCourseDto Rq { get; init; }



    public GetPagedCoursesQuery(QueryCourseDto rq)
    {
        Rq = rq;
    }
}