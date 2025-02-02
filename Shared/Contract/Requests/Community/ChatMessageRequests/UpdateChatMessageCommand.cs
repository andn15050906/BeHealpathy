using Contract.Domain.Shared.MultimediaBase;
using Contract.Requests.Community.ChatMessageRequests.Dtos;
using Contract.Responses.Community;

namespace Contract.Requests.Community.ChatMessageRequests;

public sealed class UpdateChatMessageCommand : UpdateCommand<ChatMessageModel>
{
    public UpdateChatMessageDto Rq { get; init; }
    public Guid UserId { get; init; }
    public List<Multimedia> AddedMedias { get; init; }
    public List<Guid> RemovedMedias { get; init; }



#pragma warning disable CS8618
    public UpdateChatMessageCommand(bool isCompensating = false) : base(isCompensating)
    {
    }

    public UpdateChatMessageCommand(
        UpdateChatMessageDto rq, Guid userId,
        List<Multimedia> addedMedias, List<Guid> removedMedias, bool isCompensating = false) : base(isCompensating)
    {
        Rq = rq;
        UserId = userId;
        AddedMedias = addedMedias;
        RemovedMedias = removedMedias;
    }
#pragma warning restore CS8618
}
