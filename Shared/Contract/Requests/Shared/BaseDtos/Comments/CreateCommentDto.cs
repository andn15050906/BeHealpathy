using System.ComponentModel.DataAnnotations;
using Contract.Requests.Shared.BaseDtos.Media;

namespace Contract.Requests.Shared.BaseDtos.Comments;

public sealed class CreateCommentDto
{
    public Guid SourceId { get; set; }

    [StringLength(255)]
    public string Content { get; set; } = string.Empty;

    public List<CreateMediaDto>? Medias { get; set; }

    public TargetFeedbackEntity TargetEntity { get; set; }
}
