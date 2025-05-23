﻿using Contract.Domain.Shared.MultimediaBase;
using Contract.Requests.Community.ChatMessageRequests.Dtos;
using Contract.Responses.Community;

namespace Contract.Requests.Community.ChatMessageRequests;

public sealed class CreateChatMessageCommand : CreateCommand<ChatMessageModel>
{
    public CreateChatMessageDto Rq { get; init; }
    public Guid UserId { get; init; }
    public List<Multimedia> Medias { get; init; }



    public CreateChatMessageCommand(Guid id, CreateChatMessageDto rq, Guid userId, List<Multimedia> medias, bool isCompensating = false)
        : base(id, isCompensating)
    {
        Rq = rq;
        UserId = userId;
        Medias = medias;
    }
}
