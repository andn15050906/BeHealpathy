namespace Contract.Requests.Progress.RoadmapRequests.Dtos;

public sealed class CURoadmapMilestoneDto
{
    public Guid? Id { get; set; }

    public string? Title { get; set; }
    public string? EventName { get; set; }
    public string? Content { get; set; }

    public bool? IsCompleted { get; set; }
    public bool? IsSkipped { get; set; }
    public string? Feedback { get; set; }
}