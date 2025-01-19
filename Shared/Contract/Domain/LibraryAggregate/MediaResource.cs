using Contract.Domain.LibraryAggregate.Enums;
using Contract.Domain.Shared.MultimediaBase;
using Contract.Domain.UserAggregate;

namespace Contract.Domain.LibraryAggregate;

public sealed class MediaResource : AuditedEntity
{
    // Attributes
    public string Description { get; set; }
    public string Artist { get; set; }

    public string Title { get; set; }
    public MediaResourceType Type { get; set; }

    // Navigations
    public Multimedia Media { get; set; }
    public User Creator { get; set; }

#pragma warning disable CS8618
    public MediaResource()
    {

    }

    public MediaResource(Guid id, Guid creatorId, string description, string artist, string title, MediaResourceType type)
    {
        Id = id;
        CreatorId = creatorId;
        Description = description;
        Artist = artist;
        Title = title;
        Type = type;
    }
#pragma warning restore CS8618
}