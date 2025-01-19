using System.ComponentModel.DataAnnotations;
using Contract.Requests.Shared.BaseDtos.Media;

namespace Contract.Requests.Shared.BaseDtos.Reviews;

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
