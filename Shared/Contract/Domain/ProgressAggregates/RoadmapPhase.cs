namespace Contract.Domain.ProgressAggregates;

public sealed class RoadmapPhase : Entity
{
    public int Index { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int TimeSpan { get; set; }                               // Time span in days



    public List<RoadmapMilestone> Milestones { get; set; }
}