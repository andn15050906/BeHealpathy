using System.ComponentModel.DataAnnotations;

namespace Contract.Requests.Shared.BaseDtos.Reactions;

public sealed class CreateReactionDto
{
    public Guid SourceId { get; set; }

    [Required]
    [StringLength(50)]
    public string Content { get; set; }
}
