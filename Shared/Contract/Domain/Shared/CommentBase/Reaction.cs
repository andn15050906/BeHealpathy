namespace Contract.Domain.Shared.CommentBase;

public abstract class Reaction : CreationAuditedEntity
{
    // Part of PK
    public Guid SourceId { get; set; }

    // Attributes
    public string Content { get; set; }






#pragma warning disable CS8618
    public Reaction()
    {

    }

    public Reaction(Guid creatorId, Guid sourceId, string content)
    {
        CreatorId = creatorId;
        SourceId = sourceId;
        Content = content;
    }
#pragma warning restore CS8618
}
