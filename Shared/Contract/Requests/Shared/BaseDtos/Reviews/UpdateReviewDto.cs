using System.ComponentModel.DataAnnotations;
using Contract.Requests.Shared.BaseDtos.Media;

namespace Contract.Requests.Shared.BaseDtos.Reviews;

public class UpdateReviewDto
{
    public Guid Id { get; set; }

    [StringLength(255)]
    public string? Content { get; set; }

    [RatingValidation]
    public byte? Rating { get; set; }

    public List<CreateMediaDto>? AddedMedias { get; set; }
    public List<Guid>? RemovedMedias { get; set; }
}
