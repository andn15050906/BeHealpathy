namespace Contract.Responses.Progress;

public sealed class RoadmapMilestoneModel
{
    public Guid Id { get; set; }



    public string Title { get; set; }
    public string EventName { get; set; }
    public int RepeatTimesRequired { get; set; }
    public int Index { get; set; }
    public bool IsRequired { get; set; }
    public IEnumerable<RoadmapRecommendationModel> Recommendations { get; set; } = [];
}