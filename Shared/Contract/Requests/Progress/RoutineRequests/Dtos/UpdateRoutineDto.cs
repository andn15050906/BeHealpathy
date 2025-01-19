using Contract.Domain.ProgressAggregates.Enums;

namespace Contract.Requests.Progress.RoutineRequests.Dtos;

public sealed class UpdateRoutineDto
{
    public Guid Id { get; set; }

    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Objective { get; set; }
    public Frequency? Frequency { get; set; }
}
