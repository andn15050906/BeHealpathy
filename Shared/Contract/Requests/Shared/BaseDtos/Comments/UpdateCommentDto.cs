using System.ComponentModel.DataAnnotations;
using Contract.Requests.Shared.BaseDtos.Media;

namespace Contract.Requests.Shared.BaseDtos.Comments;

public class UpdateCommentDto
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(255)]
    public string Content { get; set; }

    public List<CreateMediaDto>? AddedMedias { get; set; }
    public List<Guid>? RemovedMedias { get; set; }

    public TargetEntity TargetEntity { get; set; }
}
