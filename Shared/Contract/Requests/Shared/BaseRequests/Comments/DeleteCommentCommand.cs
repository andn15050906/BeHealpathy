namespace Contract.Requests.Shared.BaseRequests.Comments;

public class DeleteCommentCommand : DeleteCommand
{
    public DeleteCommentCommand(Guid id, Guid userId, bool isCompensating = false)
        : base(id, userId, isCompensating)
    {
    }
}
