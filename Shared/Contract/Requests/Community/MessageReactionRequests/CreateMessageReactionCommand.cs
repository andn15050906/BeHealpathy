using Contract.Requests.Shared.BaseDtos.Reactions;
using Contract.Requests.Shared.BaseRequests.Reactions;

namespace Contract.Requests.Community.MessageReactionRequests;

public sealed class CreateMessageReactionCommand : CreateReactionCommand
{
    public CreateMessageReactionCommand(Guid id, CreateReactionDto rq, Guid userId, bool isCompensating = false) : base(id, rq, userId, isCompensating)
    {
    }
}