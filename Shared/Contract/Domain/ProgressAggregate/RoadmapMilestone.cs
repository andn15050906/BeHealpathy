namespace Contract.Domain.ProgressAggregate;

public sealed class RoadmapMilestone : Entity
{
    public Guid RoadmapPhaseId { get; set; }

    public string Title { get; set; }
    public string EventName { get; set; }
    public string Content { get; set; }                     // might be JSON

    public bool IsCompleted { get; set; }
    public bool IsSkipped { get; set; }
    public string Feedback { get; set; }                    // multiple choice or self input
}