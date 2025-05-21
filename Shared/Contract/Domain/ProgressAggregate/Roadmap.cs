namespace Contract.Domain.ProgressAggregate;

public sealed class Roadmap : AuditedEntity
{
    public string Title { get; set; }
    public string IntroText { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string ThumbUrl { get; set; }

    public double? Price { get; set; }
    public double? Discount { get; set; }
    public DateTime? DiscountExpiry { get; set; }
    public string? Coupons { get; set; }



    public List<RoadmapPhase> Phases { get; set; }
}