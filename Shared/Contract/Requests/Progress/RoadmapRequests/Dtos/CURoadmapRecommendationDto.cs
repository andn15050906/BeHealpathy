namespace Contract.Requests.Progress.RoadmapRequests.Dtos;

public sealed class CURoadmapRecommendationDto
{
    public Guid TargetEntityId { get; set; }
    public string EntityType { get; set; }
    public Guid? MilestoneId { get; set; }
    public string? Trait { get; set; }
    public string? TraitDescription { get; set; }
}