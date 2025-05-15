namespace Contract.Responses.Progress;

public sealed class RoadmapMilestoneModel
{
    public Guid Id { get; set; }

    public string Title { get; set; }               = string.Empty;
    public string EventName { get; set; }           = string.Empty;
    public string Content { get; set; }             = string.Empty;

    public bool IsCompleted { get; set; }
    public bool IsSkipped { get; set; }
    public string Feedback { get; set; }            = string.Empty;
}