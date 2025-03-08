namespace Contract.Domain.ProgressAggregates;

public sealed class RoadmapMilestone : Entity
{
    public string Title { get; set; }
    public string EventName { get; set; }
    public int RepeatTimesRequired { get; set; }
    public int TimeSpentRequired { get; set; }



    public List<RoadmapRecommendation> Recommendations { get; set; }
}