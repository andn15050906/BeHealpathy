namespace Contract.Requests.Progress.RoadmapRequests.Dtos;

public sealed class CURoadmapMilestoneDto
{
    public string Title { get; set; } = string.Empty;
    public string EventName { get; set; } = string.Empty;
    public int RepeatTimesRequired { get; set; }
    public int TimeSpentRequired { get; set; }



    public List<CURoadmapRecommendationDto> Recommendations { get; set; } = [];
}