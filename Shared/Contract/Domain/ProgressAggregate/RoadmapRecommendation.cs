namespace Contract.Domain.ProgressAggregate;

public sealed class RoadmapRecommendation : Entity
{
    public Guid RoadmapPhaseId { get; set; }

    public Guid TargetEntityId { get; set; }
    public string EntityType { get; set; }
    public string? Trait { get; set; }
    public string? TraitDescription { get; set; }
}