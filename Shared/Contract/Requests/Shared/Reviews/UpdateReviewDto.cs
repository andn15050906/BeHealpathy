using System.ComponentModel.DataAnnotations;
using Contract.Requests.Shared.Media;

namespace Contract.Requests.Shared.Reviews;

public class UpdateReviewDto
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(255)]
    public string Content { get; set; }

    [Required]
    [RatingValidation]
    public byte Rating { get; set; }

    public List<CreateMediaDto>? AddedMedias { get; set; }
    public List<string>? RemovedIdentifiers { get; set; }
}
