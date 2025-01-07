using Contract.Requests.Community.ConversationRequests.Dtos;
using Contract.Requests.Shared.Base;

namespace Contract.Requests.Community.ConversationRequests;

public sealed class UpdateConversationCommand : UpdateCommand
{
    public UpdateConversationDto Rq { get; init; }
    public Guid UserId { get; init; }



    public UpdateConversationCommand(UpdateConversationDto rq, Guid userId, bool isCompensating = false)
        : base(isCompensating)
    {
        Rq = rq;
        UserId = userId;
    }
}
