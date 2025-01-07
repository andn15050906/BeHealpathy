namespace Contract.Domain.CommunityAggregate;

public sealed class ConversationMember : CreationAuditedDomainObject
{
    // Part of PK
    public Guid ConversationId { get; set; }

    // Attributes
    public bool IsAdmin { get; set; }
    public DateTime LastVisit { get; set; }                 // Not LastModification






    public ConversationMember()
    {

    }

    public ConversationMember(Guid creatorId, Guid conversationId, bool isAdmin)
    {
        CreatorId = creatorId;
        ConversationId = conversationId;

        IsAdmin = isAdmin;
    }
}