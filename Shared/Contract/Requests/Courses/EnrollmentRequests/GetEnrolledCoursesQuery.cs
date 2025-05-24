using Contract.Responses.Courses;

namespace Contract.Requests.Courses.EnrollmentRequests;

public sealed class GetEnrolledCoursesQuery : IRequest<Result<List<CourseProgressModel>>>
{
    public Guid UserId { get; init; }



    public GetEnrolledCoursesQuery(Guid userId)
    {
        UserId = userId;
    }
}