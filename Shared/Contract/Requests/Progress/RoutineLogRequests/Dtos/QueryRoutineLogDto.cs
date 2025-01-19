namespace Contract.Requests.Progress.RoutineLogRequests.Dtos;

public sealed class QueryRoutineLogDto : PagingQueryDto
{
    public Guid? RoutineId { get; set; }
}
