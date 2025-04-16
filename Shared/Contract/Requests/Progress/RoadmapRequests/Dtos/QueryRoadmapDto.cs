namespace Contract.Requests.Progress.RoadmapRequests.Dtos;

public sealed class QueryRoadmapDto : PagingQueryDto
{
    public Guid? CreatorId { get; set; }
}