namespace Contract.Domain.ProgressAggregates;

public sealed class RoadmapPhase : Entity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int TimeSpan { get; set; }



    public List<RoadmapMilestone> Milestones { get; set; }
}