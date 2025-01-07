using Contract.Domain.ProgressAggregates.Enums;

namespace Contract.Domain.ProgressAggregates;

public sealed class Routine : AuditedEntity
{
    // Attributes
    public string Description { get; set; }
    public string Objective { get; set; }
    public Frequency Frequency { get; set; }

    // Navigations
    public List<RoutineLog> Logs { get; set; }
}
