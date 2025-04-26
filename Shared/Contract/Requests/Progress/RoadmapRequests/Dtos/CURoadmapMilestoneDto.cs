namespace Contract.Requests.Progress.RoadmapRequests.Dtos;

public sealed class CURoadmapMilestoneDto
{
    public Guid? Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string EventName { get; set; } = string.Empty;
    public int RepeatTimesRequired { get; set; }
    public int Index { get; set; } = 0;
    public bool IsRequired { get; set; } = false;



    public List<CURoadmapRecommendationDto> Recommendations { get; set; } = [];
}