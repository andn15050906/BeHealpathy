namespace Contract.Requests.Courses.AdvisorRequests.Dtos;

public sealed class QueryAdvisorDto : PagingQueryDto
{
    public Guid? UserId { get; init; }
}