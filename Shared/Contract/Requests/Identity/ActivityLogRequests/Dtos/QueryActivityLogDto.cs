namespace Contract.Requests.Identity.ActivityLogRequests.Dtos;

public sealed class QueryActivityLogDto : PagingQueryDto
{
    public Guid? CreatorId { get; set; }
}