namespace Contract.Requests.Courses.CourseRequests;

public sealed class DeleteCourseCommand : DeleteCommand
{
    public DeleteCourseCommand(Guid id, Guid userId, bool isCompensating = false)
        : base(id, userId, isCompensating)
    {
    }
}
