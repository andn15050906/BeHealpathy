namespace Contract.Requests.Progress.RoutineLogRequests.Dtos;

public sealed class CreateRoutineLogDto
{
    public Guid RoutineId { get; set; }

    public string Content { get; set; }
}
