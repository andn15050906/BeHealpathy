using Contract.Domain.ToolAggregate.Enums;

namespace Contract.Requests.Progress.RoutineRequests.Dtos;

public sealed class UpdateRoutineDto
{
    public Guid Id { get; set; }

    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Objective { get; set; }
    public int? Tag { get; set; }
    public Frequency? Repeater { get; set; }
    public Guid? RepeaterSequenceId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? IsCompleted { get; set; }
    public bool? IsClosed { get; set; }
}
