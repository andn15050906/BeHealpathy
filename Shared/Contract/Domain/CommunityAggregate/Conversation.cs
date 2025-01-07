namespace Contract.Domain.CommunityAggregate;

public sealed class Conversation : CreationAuditedEntity
{
    // Attributes
    public string Title { get; set; }
    public bool IsPrivate { get; set; }
    public string AvatarUrl { get; set; }

    // Navigations
    public List<ConversationMember> Members { get; set; }
    public List<ChatMessage> ChatMessages { get; set; }






#pragma warning disable CS8618
    public Conversation()
    {

    }

    public Conversation(Guid id, Guid creatorId, string title, bool isPrivate, string avatarUrl, List<ConversationMember> members)
    {
        Id = id;
        CreatorId = creatorId;

        Title = title;
        IsPrivate = isPrivate;
        AvatarUrl = avatarUrl;
        Members = members;
    }
#pragma warning restore CS8618
}
