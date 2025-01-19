using Contract.Responses.Courses;

namespace Contract.Requests.Courses.EnrollmentRequests;

public sealed class GetEnrolledCoursesQuery : IRequest<Result<List<EnrollmentModel>>>
{
    public Guid UserId { get; init; }



    public GetEnrolledCoursesQuery(Guid userId)
    {
        UserId = userId;
    }
}