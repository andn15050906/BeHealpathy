using Contract.Requests.Shared.BaseRequests.Reactions;

namespace Contract.Requests.Community.MessageReactionRequests;

public sealed class DeleteMessageReactionCommand : DeleteReactionCommand
{
    public DeleteMessageReactionCommand(Guid id, Guid userId, bool isCompensating = false) : base(id, userId, isCompensating)
    {
    }
}