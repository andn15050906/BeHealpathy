namespace Contract.Domain.UserAggregate;

public sealed class UserStatistics : CreationAuditedEntity
{
    // Attributes
    public string Content { get; set; }

#pragma warning disable CS8618
    public UserStatistics(Guid id, Guid creatorId, DateTime creationTime, string content)
    {
        Id = id;
        CreatorId = creatorId;

        CreationTime = creationTime;
        Content = content;
    }
#pragma warning restore CS8618
}