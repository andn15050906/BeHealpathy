using Contract.Requests.Shared.BaseRequests.Comments;

namespace Contract.Requests.Courses.LectureCommentRequests;

public sealed class DeleteLectureCommentCommand : DeleteCommentCommand
{
    public DeleteLectureCommentCommand(Guid id, Guid userId, bool isCompensating = false) : base(id, userId, isCompensating)
    {
    }
}