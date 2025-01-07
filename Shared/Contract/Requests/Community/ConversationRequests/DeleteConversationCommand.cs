using Contract.Requests.Shared.Base;

namespace Contract.Requests.Community.ConversationRequests;

public sealed class DeleteConversationCommand : DeleteCommand
{
    public DeleteConversationCommand(Guid id, Guid userId, bool isCompensating = false)
        : base(id, userId, isCompensating)
    {
    }
}
