using Contract.Requests.Shared.Base;

namespace Contract.Requests.Courses.EnrollmentRequests.Dtos;

public sealed class QueryEnrollmentDto : PagingQueryDto
{
    public Guid? CourseId { get; init; }
    public bool? IsGetEnrolledCourse { get; init; }
}
