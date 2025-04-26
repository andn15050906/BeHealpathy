namespace Contract.Domain.ProgressAggregates;

public sealed class RoadmapMilestone : Entity
{
    public string Title { get; set; }
    public string EventName { get; set; }
    public int RepeatTimesRequired { get; set; }
    public int Index { get; set; }
    public bool IsRequired { get; set; }



    public List<RoadmapRecommendation> Recommendations { get; set; }
}