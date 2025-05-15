using Contract.Domain.CourseAggregate;

namespace Contract.Domain.ProgressAggregate;

public sealed class Roadmap : AuditedEntity
{
    public Guid AdvisorId { get; set; }
    public string Title { get; set; }
    public string IntroText { get; set; }



    public List<RoadmapPhase> Phases { get; set; }
    public Advisor Advisor { get; set; }
}