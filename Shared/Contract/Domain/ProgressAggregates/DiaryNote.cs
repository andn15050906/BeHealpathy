using Contract.Domain.Shared.MultimediaBase;

namespace Contract.Domain.ProgressAggregates;

public sealed class DiaryNote : AuditedEntity
{
    public string Title { get; set; }
    public string Content { get; set; }
    public List<Multimedia> Attachments { get; set; }
    public string Mood { get; set; }
    public string Theme { get; set; }
}
