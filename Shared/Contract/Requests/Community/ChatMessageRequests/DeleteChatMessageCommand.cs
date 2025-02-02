using Contract.Responses.Community;

namespace Contract.Requests.Community.ChatMessageRequests;

public sealed class DeleteChatMessageCommand : DeleteCommand<ChatMessageModel>
{
    public DeleteChatMessageCommand(Guid id, Guid userId, bool isCompensating = false)
        : base(id, userId, isCompensating)
    {
    }
}