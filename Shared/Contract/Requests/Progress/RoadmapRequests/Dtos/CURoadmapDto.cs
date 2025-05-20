using Contract.Requests.Shared.BaseDtos.Media;

namespace Contract.Requests.Progress.RoadmapRequests.Dtos;

public sealed class CURoadmapDto
{
    public Guid? Id { get; set; }

    public string? Title { get; set; }
    public string? IntroText { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public CreateMediaDto? Thumb { get; set; }

    public double? Price { get; set; }
    public double? Discount { get; set; }
    public DateTime? DiscountExpiry { get; set; }
    public string? Coupons { get; set; }



    public List<CURoadmapPhaseDto> Phases { get; set; } = [];
}