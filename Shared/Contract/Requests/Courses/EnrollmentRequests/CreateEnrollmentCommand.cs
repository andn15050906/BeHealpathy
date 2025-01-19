using Contract.Requests.Courses.EnrollmentRequests.Dtos;

namespace Contract.Requests.Courses.EnrollmentRequests;

public sealed class CreateEnrollmentCommand : CreateCommand
{
    public CreateEnrollmentDto Rq { get; init; }
    public Guid UserId { get; init; }



    public CreateEnrollmentCommand(Guid id, CreateEnrollmentDto rq, Guid userId, bool isCompensating = false)
        : base(id, isCompensating)
    {
        Rq = rq;
        UserId = userId;
    }
}