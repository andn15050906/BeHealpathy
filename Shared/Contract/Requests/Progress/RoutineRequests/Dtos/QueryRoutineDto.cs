namespace Contract.Requests.Progress.RoutineRequests.Dtos;

public sealed class QueryRoutineDto : PagingQueryDto
{
    public Guid? CreatorId { get; set; }
    public string? Title { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}