namespace Contract.Responses.Progress;

public sealed class RoadmapMilestoneModel
{
    public Guid Id { get; set; }



    public string Title { get; set; }
    public string EventName { get; set; }
    public int RepeatTimesRequired { get; set; }
    public int TimeSpentRequired { get; set; }
    public IEnumerable<RoadmapRecommendationModel> Recommendations { get; set; }
}