using System.ComponentModel.DataAnnotations;
using Contract.Requests.Shared.Media;

namespace Contract.Requests.Shared.Reviews;

public sealed class CreateReviewDto
{
    public Guid SourceId { get; set; }

    [Required]
    [RatingValidation]
    public byte Rating { get; set; }

    [Required]
    [StringLength(255)]
    public string Content { get; set; }

    public List<CreateMediaDto>? Medias { get; set; }
}
