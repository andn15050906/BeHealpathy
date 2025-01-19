namespace Contract.Requests.Courses.LectureRequests;

public sealed class DeleteLectureCommand : DeleteCommand
{
    public DeleteLectureCommand(Guid id, Guid userId, bool isCompensating = false)
        : base(id, userId, isCompensating)
    {
    }
}