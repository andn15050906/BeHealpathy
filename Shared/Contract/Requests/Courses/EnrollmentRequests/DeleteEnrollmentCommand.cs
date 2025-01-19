namespace Contract.Requests.Courses.EnrollmentRequests;

public sealed class DeleteEnrollmentCommand : DeleteCommand
{
    public DeleteEnrollmentCommand(Guid id, Guid userId, bool isCompensating = false)
        : base(id, userId, isCompensating)
    {
    }
}