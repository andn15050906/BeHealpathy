using Contract.Responses.Courses;

namespace Contract.Requests.Courses.CourseRequests;

public sealed class GetCourseByIdQuery : GetByIdQuery<CourseModel>
{
    public GetCourseByIdQuery(Guid id) : base(id)
    {
    }
}
