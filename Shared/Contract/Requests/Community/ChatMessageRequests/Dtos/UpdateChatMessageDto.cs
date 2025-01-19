using Contract.Domain.CommunityAggregate.Enums;
using Contract.Requests.Shared.BaseDtos.Media;

namespace Contract.Requests.Community.ChatMessageRequests.Dtos;

public sealed class UpdateChatMessageDto
{
    public Guid Id { get; set; }

    public string Content { get; set; }
    public MessageStatus Status { get; set; }

    public List<CreateMediaDto>? AddedMedias { get; set; }
    public List<Guid>? RemovedMedias { get; set; }
}
