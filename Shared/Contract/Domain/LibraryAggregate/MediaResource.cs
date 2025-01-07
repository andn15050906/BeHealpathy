using Contract.Domain.LibraryAggregate.Enums;
using Contract.Domain.Shared.MultimediaBase;

namespace Contract.Domain.LibraryAggregate;

public sealed class MediaResource : AuditedEntity
{
    // Attributes
    public string Description { get; set; }
    public string Title { get; set; }
    public string Artist { get; set; }
    public MediaResourceType Type { get; set; }

    // Navigations
    public List<Multimedia> Medias { get; set; }

#pragma warning disable CS8618
    public MediaResource()
    {

    }
#pragma warning restore CS8618
}