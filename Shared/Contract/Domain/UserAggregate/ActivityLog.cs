namespace Contract.Domain.UserAggregate;

public sealed class ActivityLog : CreationAuditedEntity
{
    // Attributes
    public string Content { get; set; }

    // Navigations
    public User Creator { get; set; }

#pragma warning disable CS8618
    public ActivityLog(Guid id, Guid creatorId, DateTime creationTime, string content)
    {
        Id = id;
        CreatorId = creatorId;

        CreationTime = creationTime;
        Content = content;
    }
#pragma warning restore CS8618
}