using System.ComponentModel.DataAnnotations;
using Contract.Requests.Shared.BaseDtos.Media;

namespace Contract.Requests.Courses.LectureRequests.Dtos;

public sealed class CreateLectureDto
{
    public Guid? Id { get; set; }                           // not set from client

    [Required]
    [StringLength(100)]
    public string Title { get; set; }

    [Required]
    [StringLength(255)]
    public string Content { get; set; }

    [StringLength(255)]
    public string ContentSummary { get; set; }

    public bool IsPreviewable { get; set; }

    public List<CreateMediaDto>? Medias { get; set; }
}
