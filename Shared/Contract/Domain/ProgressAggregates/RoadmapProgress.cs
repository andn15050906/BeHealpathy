using Contract.Domain.UserAggregate;

namespace Contract.Domain.ProgressAggregates;

public sealed class RoadmapProgress : CreationAuditedDomainObject
{
    public Guid RoadmapPhaseId { get; set; }
    public string MilestonesCompleted { get; set; }

    public User Creator { get; set; }
    public RoadmapPhase RoadmapPhase { get; set; }



#pragma warning disable CS8618
    public RoadmapProgress()
    {
    }

    public RoadmapProgress(Guid creatorId, Guid roadmapPhaseId, string milestonesCompleted)
    {
        CreatorId = creatorId;
        RoadmapPhaseId = roadmapPhaseId;
        MilestonesCompleted = milestonesCompleted;
    }
#pragma warning restore CS8618
}