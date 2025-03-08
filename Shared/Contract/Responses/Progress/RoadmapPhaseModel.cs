namespace Contract.Responses.Progress;

public sealed class RoadmapPhaseModel
{
    public Guid Id { get; set; }



    public string Title { get; set; }
    public string Description { get; set; }
    public int TimeSpan { get; set; }
    public IEnumerable<RoadmapMilestoneModel> Milestones { get; set; }
}