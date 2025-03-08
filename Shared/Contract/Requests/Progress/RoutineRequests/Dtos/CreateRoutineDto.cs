using Contract.Domain.ProgressAggregates.Enums;

namespace Contract.Requests.Progress.RoutineRequests.Dtos;

public sealed class CreateRoutineDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Objective { get; set; } = string.Empty;
    public Frequency Repeater { get; set; }
    public Guid? RepeaterSequenceId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsClosed { get; set; }
    public int Tag { get; set; }
}
