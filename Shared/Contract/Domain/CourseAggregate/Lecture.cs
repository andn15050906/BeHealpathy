using Contract.Domain.Shared.MultimediaBase;

namespace Contract.Domain.CourseAggregate;

public sealed class Lecture : AuditedEntity
{
    // Attributes
    public string Title { get; set; }
    public string Content { get; set; }
    public string ContentSummary { get; set; }
    public bool IsPreviewable { get; set; }

    // FKs
    public Guid SectionId { get; set; }

    // Navigations
    public List<Multimedia> Materials { get; set; }
    public List<LectureComment> Comments { get; set; }







#pragma warning disable CS8618
    public Lecture()
    {

    }
#pragma warning restore CS8618

    public Lecture(Guid id, Guid creatorId, Guid sectionId, string title, string content, string contentSummary, bool isPreviewable, List<Multimedia> materials)
    {
        Id = id;
        CreatorId = creatorId;
        SectionId = sectionId;
        Title = title;
        Content = content;
        ContentSummary = contentSummary;
        IsPreviewable = isPreviewable;
        Materials = materials;
    }
}