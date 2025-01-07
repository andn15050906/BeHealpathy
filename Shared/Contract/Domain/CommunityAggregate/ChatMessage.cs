using Contract.Domain.CommunityAggregate.Enums;

namespace Contract.Domain.CommunityAggregate;

public sealed class ChatMessage : AuditedEntity
{
    // Attributes
    public string Content { get; set; }
    public MessageStatus Status { get; set; }

    // FKs
    public Guid ConversationId { get; set; }

    // Navigations
    public List<MessageReaction> Reactions { get; set; }






#pragma warning disable CS8618
    public ChatMessage()
    {

    }

    public ChatMessage(Guid id, Guid creatorId, string content, Guid conversationId)
    {
        Id = id;
        CreatorId = creatorId;

        Content = content;
        Status = MessageStatus.Sent;
        ConversationId = conversationId;
    }
#pragma warning restore CS8618
}