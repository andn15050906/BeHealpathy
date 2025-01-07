namespace Contract.Requests.Community.ChatMessageRequests.Dtos;

public sealed class CreateChatMessageDto
{
    public Guid ConversationId { get; set; }

    public string Content { get; set; }
    public List<string>? Attachments { get; set; }
}
