namespace Contract.Domain.Shared.ProgressBase;

public abstract class CheckItem : CreationAuditedDomainObject
{
    // Attributes
    public bool IsCompleted { get; set; }
    public string Content { get; set; }

    // FKs
    public Guid SourceId { get; set; }



#pragma warning disable CS8618
    public CheckItem()
    {

    }
#pragma warning restore CS8618

    public CheckItem(Guid creatorId, string content)
    {
        CreatorId = creatorId;
        Content = content;
    }
}