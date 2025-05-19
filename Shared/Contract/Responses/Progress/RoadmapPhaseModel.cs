namespace Contract.Responses.Progress;

public sealed class RoadmapPhaseModel
{
    public Guid Id { get; set; }

    public string Title { get; set; }                       = string.Empty;
    public string Description { get; set; }                 = string.Empty;
    public string Introduction { get; set; }                = string.Empty;
    public int Index { get; set; }
    public int TimeSpan { get; set; }                                       // Time span in days
    public bool IsRequiredToAdvance { get; set; }
    public string QuestionsToAdvance { get; set; }          = string.Empty;
    public string VideoUrl { get; set; }                    = string.Empty;

    public IEnumerable<RoadmapMilestoneModel> Milestones { get; set; }
}