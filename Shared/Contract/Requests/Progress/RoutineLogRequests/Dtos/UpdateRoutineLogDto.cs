namespace Contract.Requests.Progress.RoutineLogRequests.Dtos;

public sealed class UpdateRoutineLogDto
{
    public Guid Id { get; set; }

    public string Content { get; set; }
}
