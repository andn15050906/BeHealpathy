namespace Contract.Domain.ProgressAggregate;

public sealed class RoadmapPhase : Entity
{
    public Guid RoadmapId { get; set; }

    public string Title { get; set; }
    public string? Description { get; set; }
    public string? Introduction { get; set; }
    public int Index { get; set; }
    public int TimeSpan { get; set; }                               // Time span in days
    public bool IsRequiredToAdvance { get; set; }
    public string? QuestionsToAdvance { get; set; }
    public string? VideoUrl { get; set; }

    public List<RoadmapMilestone> Milestones { get; set; }
    public List<RoadmapRecommendation> Recommendations { get; set; }
}