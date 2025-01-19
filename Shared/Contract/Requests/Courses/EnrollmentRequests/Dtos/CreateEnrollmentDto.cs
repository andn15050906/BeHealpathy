namespace Contract.Requests.Courses.EnrollmentRequests.Dtos;

public sealed class CreateEnrollmentDto
{
    public Guid CourseId { get; set; }
    public Guid? BillId { get; set; }
    public bool IsGranted { get; set; }
}
