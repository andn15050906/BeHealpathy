namespace Contract.Domain.ProgressAggregates;

public sealed class RoutineLog : CreationAuditedEntity
{
    public Guid RoutineId { get; set; }
    public string Content { get; set; }
}
