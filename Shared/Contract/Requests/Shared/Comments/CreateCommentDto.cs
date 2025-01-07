using System.ComponentModel.DataAnnotations;
using Contract.Requests.Shared.Media;

namespace Contract.Requests.Shared.Comments;

public sealed class CreateCommentDto
{
    public Guid SourceId { get; set; }

    [Required]
    [StringLength(255)]
    public string Content { get; set; }

    public List<CreateMediaDto>? Medias { get; set; }
}
