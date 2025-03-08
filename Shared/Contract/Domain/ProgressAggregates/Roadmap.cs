namespace Contract.Domain.ProgressAggregates;

public sealed class Roadmap : AuditedEntity
{
    public string Title { get; set; }
    public string IntroText { get; set; }



    public List<RoadmapPhase> Phases { get; set; }
}