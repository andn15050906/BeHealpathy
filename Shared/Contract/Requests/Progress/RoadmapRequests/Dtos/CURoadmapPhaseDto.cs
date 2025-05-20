namespace Contract.Requests.Progress.RoadmapRequests.Dtos;

public sealed class CURoadmapPhaseDto
{
    public Guid? Id { get; set; }

    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Introduction { get; set; }
    public int? Index { get; set; }
    public int? TimeSpan { get; set; }                               // Time span in days
    public bool? IsRequiredToAdvance { get; set; }
    public string? QuestionsToAdvance { get; set; }
    public string? VideoUrl { get; set; }

    public List<CURoadmapRecommendationDto> Recommendations { get; set; } = [];
}