using Contract.Domain.UserAggregate;

namespace Contract.Domain.ProgressAggregates;

public sealed class RoadmapProgress : CreationAuditedDomainObject
{
    public Guid RoadmapPhaseId { get; set; }
    public Guid Milestone { get; set; }

    public User Creator { get; set; }
    public RoadmapPhase RoadmapPhase { get; set; }



#pragma warning disable CS8618
    public RoadmapProgress()
    {
    }

    public RoadmapProgress(Guid creatorId, Guid roadmapPhaseId, Guid milestone)
    {
        CreatorId = creatorId;
        RoadmapPhaseId = roadmapPhaseId;
        Milestone = milestone;
    }

    public RoadmapProgress(Guid creatorId, Guid roadmapPhaseId, Guid milestone, DateTime creationTime)
    {
        CreatorId = creatorId;
        RoadmapPhaseId = roadmapPhaseId;
        Milestone = milestone;
        CreationTime = creationTime;
    }
#pragma warning restore CS8618
}