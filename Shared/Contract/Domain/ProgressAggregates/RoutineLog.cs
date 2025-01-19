namespace Contract.Domain.ProgressAggregates;

public sealed class RoutineLog : CreationAuditedEntity
{
    public Guid RoutineId { get; set; }
    public string Content { get; set; }



#pragma warning disable CS8618
    public RoutineLog()
    {

    }
#pragma warning restore CS8618

    public RoutineLog(Guid id, Guid creatorId, Guid routineId, string content)
    {
        Id = id;
        CreatorId = creatorId;
        RoutineId = routineId;
        Content = content;
    }
}
