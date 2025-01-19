namespace Contract.Requests.Shared.BaseRequests.Reactions;

public class DeleteReactionCommand : DeleteCommand
{
    public DeleteReactionCommand(Guid id, Guid userId, bool isCompensating = false)
        : base(id, userId, isCompensating)
    {
    }
}