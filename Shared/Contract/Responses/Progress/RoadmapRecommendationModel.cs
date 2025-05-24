namespace Contract.Responses.Progress;

public sealed class RoadmapRecommendationModel
{
    public Guid Id { get; set; }

    public Guid RoadmapPhaseId { get; set; }

    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? Description { get; set; }
    public bool IsAction { get; set; }

    public int? Duration { get; set; }
    public string? MoodTags { get; set; }
    public bool? IsGeneralTip { get; set; }
    public string? Source { get; set; }

    public Guid? TargetEntityId { get; set; }
    public string? EntityType { get; set; }
}