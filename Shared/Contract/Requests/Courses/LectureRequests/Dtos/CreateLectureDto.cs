using System.ComponentModel.DataAnnotations;
using Contract.Requests.Shared.BaseDtos.Media;

namespace Contract.Requests.Courses.LectureRequests.Dtos;

public sealed class CreateLectureDto
{
    public Guid? Id { get; set; }                           // not set from client

    [Required]
    [StringLength(100)]
    public string Title { get; set; }                       = string.Empty;

    [Required]
    [StringLength(255)]
    public string Content { get; set; }                     = string.Empty;

    [StringLength(255)]
    public string ContentSummary { get; set; }              = string.Empty;

    public bool IsPreviewable { get; set; }

    public int? Index { get; set; }                         = 0;

    public string? LectureType { get; set; }                = string.Empty;

    public string? MetaData { get; set; }                   = string.Empty;

    public Guid CourseId { get; set; }

    public List<CreateMediaDto>? Medias { get; set; }
}
