using Contract.Domain.Shared.MultimediaBase;

namespace Contract.Domain.CourseAggregate;

public sealed class Lecture : AuditedEntity
{
    // Attributes
    public string Title { get; set; }
    public string Content { get; set; }
    public string ContentSummary { get; set; }
    public bool IsPreviewable { get; set; }
    public int Index { get; set; }
    public string LectureType { get; set; }         // video, document
    public string MetaData { get; set; }
    
    // FKs
    public Guid CourseId { get; set; }

    // Navigations
    public List<Multimedia> Materials { get; set; }
    public List<LectureComment> Comments { get; set; }







#pragma warning disable CS8618
    public Lecture()
    {

    }

    public Lecture(
        Guid id, Guid creatorId,
        string title, string? content, string? contentSummary, bool isPreviewable,
        int? index, string? lectureType, string? metaData,
        Guid courseId)
    {
        Id = id;
        CreatorId = creatorId;

        Title = title;
        Content = content ?? string.Empty;
        ContentSummary = contentSummary ?? string.Empty;
        IsPreviewable = isPreviewable;

        Index = index ?? 0;
        LectureType = lectureType ?? string.Empty;
        MetaData = metaData ?? string.Empty;

        CourseId = courseId;
    }
#pragma warning restore CS8618
}