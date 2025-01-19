using Contract.Domain.UserAggregate;

namespace Contract.Domain.Shared.ReactionBase;

public abstract class Reaction : CreationAuditedEntity
{
    // Part of PK
    public Guid SourceId { get; set; }

    // Attributes
    public string Content { get; set; }
    public User Creator { get; set; }






#pragma warning disable CS8618
    public Reaction()
    {

    }

    public Reaction(Guid id, Guid creatorId, Guid sourceId, string content)
    {
        Id = id;
        CreatorId = creatorId;
        SourceId = sourceId;
        Content = content;
    }
#pragma warning restore CS8618
}
