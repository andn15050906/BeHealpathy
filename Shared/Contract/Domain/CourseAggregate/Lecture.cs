using Contract.Domain.Shared.MultimediaBase;

namespace Contract.Domain.CourseAggregate;

public sealed class Lecture : AuditedEntity
{
    // Attributes
    public string Title { get; set; }
    public string Content { get; set; }
    public string ContentSummary { get; set; }
    public bool IsPreviewable { get; set; }
    
    //public Guid SectionId { get; set; }
    public Guid CourseId { get; set; }

    // Navigations
    public List<Multimedia> Materials { get; set; }
    public List<LectureComment> Comments { get; set; }







#pragma warning disable CS8618
    public Lecture()
    {

    }

    public Lecture(Guid id, Guid creatorId, string title, string content, string contentSummary, bool isPreviewable)
    {
        Id = id;
        CreatorId = creatorId;
        Title = title;
        Content = content;
        ContentSummary = contentSummary;
        IsPreviewable = isPreviewable;
    }
#pragma warning restore CS8618
}