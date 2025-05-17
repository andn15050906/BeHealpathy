using Contract.Requests.Shared.BaseDtos.Media;

namespace Contract.Requests.Courses.LectureRequests.Dtos;

public sealed class UpdateLectureDto
{
    public Guid Id { get; set; }

    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? ContentSummary { get; set; }
    public bool? IsPreviewable { get; set; }

    public int? Index { get; set; }
    public string? LectureType { get; set; }
    public string? MetaData { get; set; }

    public Guid CourseId { get; set; }
    public List<CreateMediaDto>? AddedMedias { get; set; }
    public List<Guid>? RemovedMedias { get; set; }
}
