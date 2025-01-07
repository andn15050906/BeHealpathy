using Contract.Domain.Shared.CommentBase.Enums;
using Contract.Domain.Shared.MultimediaBase;

namespace Contract.Domain.Shared.CommentBase;

public abstract class Comment : AuditedEntity
{
    // Attributes
    public string Content { get; set; }
    public CommentStatus Status { get; set; }
    public List<Multimedia> Medias { get; set; }

    // FKs
    public Guid SourceId { get; set; }






#pragma warning disable CS8618
    public Comment()
    {

    }

    public Comment(Guid id, Guid creatorId, Guid sourceId, string content, List<Multimedia> medias)
    {
        Id = id;
        CreatorId = creatorId;

        SourceId = sourceId;
        Content = content;
        Status = CommentStatus.None;
        Medias = medias;
    }
#pragma warning restore CS8618
}
