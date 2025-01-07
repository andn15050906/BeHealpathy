using Contract.Requests.Community.ConversationRequests.Dtos;

namespace Contract.Requests.Community.ConversationRequests;

public sealed class CreateConversationCommand : CreateCommand
{
    public CreateConversationDto Rq { get; init; }
    public Guid UserId { get; init; }



    public CreateConversationCommand(Guid id, CreateConversationDto rq, Guid userId, bool isCompensating = false)
        : base(id, isCompensating)
    {
        Rq = rq;
        UserId = userId;
    }
}
