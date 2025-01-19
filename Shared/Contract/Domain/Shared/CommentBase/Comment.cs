using Contract.Domain.Shared.CommentBase.Enums;
using Contract.Domain.Shared.MultimediaBase;
using Contract.Domain.UserAggregate;

namespace Contract.Domain.Shared.CommentBase;

public abstract class Comment : AuditedEntity
{
    // Attributes
    public string Content { get; set; }
    public CommentStatus Status { get; set; }

    // FKs
    public Guid SourceId { get; set; }

    // Navigations
    public List<Multimedia> Medias { get; set; }
    public User Creator { get; set; }






#pragma warning disable CS8618
    public Comment()
    {

    }

    public Comment(Guid id, Guid creatorId, Guid sourceId, string content)
    {
        Id = id;
        CreatorId = creatorId;

        SourceId = sourceId;
        Content = content;
        Status = CommentStatus.None;
    }
#pragma warning restore CS8618
}
