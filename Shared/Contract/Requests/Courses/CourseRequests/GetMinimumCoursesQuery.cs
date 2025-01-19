using Contract.Requests.Courses.CourseRequests.Dtos;
using Contract.Responses.Courses;

namespace Contract.Requests.Courses.CourseRequests;

public sealed class GetMinimumCoursesQuery : IRequest<Result<PagedResult<CourseMinModel>>>
{
    public QueryCourseDto Rq { get; init; }



    public GetMinimumCoursesQuery(QueryCourseDto rq)
    {
        Rq = rq;
    }
}