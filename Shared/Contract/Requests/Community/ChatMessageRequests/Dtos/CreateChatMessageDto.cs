using Contract.Requests.Shared.BaseDtos.Media;

namespace Contract.Requests.Community.ChatMessageRequests.Dtos;

public sealed class CreateChatMessageDto
{
    public Guid ConversationId { get; set; }

    public string Content { get; set; }
    public List<CreateMediaDto>? Medias { get; set; }
}
