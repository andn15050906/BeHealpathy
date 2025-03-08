namespace Contract.Domain.ProgressAggregates;

public sealed class RoadmapRecommendation : Entity
{
    public Guid TargetEntityId { get; set; }
    public string EntityType { get; set; }
    public Guid? MilestoneId { get; set; }
    public string? Trait { get; set; }
    public string? TraitDescription { get; set; }



    // Navigations
    public RoadmapMilestone? Milestone { get; set; }
}