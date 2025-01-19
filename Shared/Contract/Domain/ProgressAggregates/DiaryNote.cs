using Contract.Domain.Shared.MultimediaBase;
using Contract.Domain.UserAggregate;

namespace Contract.Domain.ProgressAggregates;

public sealed class DiaryNote : AuditedEntity
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string Mood { get; set; }
    public string Theme { get; set; }

    // Navigators
    public List<Multimedia> Attachments { get; set; }
    public User Creator { get; set; }



#pragma warning disable CS8618
    public DiaryNote()
    {

    }

    public DiaryNote(Guid id, Guid creatorId, string title, string content, string mood, string theme)
    {
        Id = id;
        CreatorId = creatorId;

        Title = title;
        Content = content;
        Mood = mood;
        Theme = theme;
    }
#pragma warning restore CS8618
}
