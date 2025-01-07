using Contract.Domain.Shared.MultimediaBase;

namespace Contract.Domain.Shared.ReviewBase;

public abstract class Review : AuditedEntity
{
    // Attributes
    public string Content { get; set; }
    public byte Rating { get; set; }

    // FKs
    public Guid SourceId { get; set; }

    // Navigations
    public List<Multimedia> Medias { get; set; }






#pragma warning disable CS8618
    public Review()
    {

    }
#pragma warning restore CS8618

    public Review(Guid id, Guid creatorId, Guid sourceId, string content, byte rating, List<Multimedia> medias)
    {
        Id = id;
        CreatorId = creatorId;

        SourceId = sourceId;
        Content = content;
        Rating = rating;
        Medias = medias;
    }
}