using Contract.Domain.Shared.MultimediaBase.Enums;

namespace Contract.Domain.Shared.MultimediaBase;

public class Multimedia : Entity
{
    public Guid SourceId { get; init; }
    public string Identifier { get; init; }                     // Id for cloud storage, Path for local storage
    public MediaType Type { get; init; }
    public string Url { get; init; }                            // viewable url
    public string Title { get; init; }
    // public int LengthInSec { get; init; }



#pragma warning disable CS8618
    public Multimedia()
    {

    }
#pragma warning restore CS8618

    public Multimedia(Guid sourceId, string identifier, MediaType type, string url, string title)
    {
        SourceId = sourceId;
        Identifier = identifier;
        Type = type;
        Url = url;
        Title = title;
    }
}