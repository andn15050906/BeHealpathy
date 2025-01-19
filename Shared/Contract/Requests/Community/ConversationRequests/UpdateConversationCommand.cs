using Contract.Domain.Shared.MultimediaBase;
using Contract.Requests.Community.ConversationRequests.Dtos;

namespace Contract.Requests.Community.ConversationRequests;

public sealed class UpdateConversationCommand : UpdateCommand
{
    public UpdateConversationDto Rq { get; init; }
    public Guid UserId { get; init; }
    public Multimedia? Media { get; init; }



    public UpdateConversationCommand(UpdateConversationDto rq, Guid userId, Multimedia? media, bool isCompensating = false)
        : base(isCompensating)
    {
        Rq = rq;
        UserId = userId;
        Media = media;
    }
}