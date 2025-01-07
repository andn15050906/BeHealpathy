using System.ComponentModel.DataAnnotations;
using Contract.Requests.Shared.Media;

namespace Contract.Requests.Shared.Comments;

public class UpdateCommentDto
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(255)]
    public string Content { get; set; }

    public List<CreateMediaDto>? AddedMedias { get; set; }
    public List<string>? RemovedIdentifiers { get; set; }
}
