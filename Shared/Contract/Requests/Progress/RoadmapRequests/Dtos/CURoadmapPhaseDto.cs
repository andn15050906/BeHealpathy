namespace Contract.Requests.Progress.RoadmapRequests.Dtos;

public sealed class CURoadmapPhaseDto
{
    public Guid? Id { get; set; }
    public int Index { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int TimeSpan { get; set; }



    public List<CURoadmapMilestoneDto> Milestones { get; set; } = [];
}