using Contract.Requests.Community.ChatMessageRequests.Dtos;

namespace Contract.Requests.Community.ChatMessageRequests;

public sealed class CreateChatMessageCommand : CreateCommand
{
    public CreateChatMessageDto Rq { get; init; }
    public Guid UserId { get; init; }



    public CreateChatMessageCommand(Guid id, CreateChatMessageDto rq, Guid userId, bool isCompensating = false)
        : base(id, isCompensating)
    {
        Rq = rq;
        UserId = userId;
    }
}
