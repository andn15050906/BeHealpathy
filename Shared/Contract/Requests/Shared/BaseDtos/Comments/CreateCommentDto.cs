using System.ComponentModel.DataAnnotations;
using Contract.Requests.Shared.BaseDtos.Media;

namespace Contract.Requests.Shared.BaseDtos.Comments;

public sealed class CreateCommentDto
{
    public Guid SourceId { get; set; }

    [Required]
    [StringLength(255)]
    public string Content { get; set; }

    public List<CreateMediaDto>? Medias { get; set; }

    public TargetEntity TargetEntity { get; set; }
}
