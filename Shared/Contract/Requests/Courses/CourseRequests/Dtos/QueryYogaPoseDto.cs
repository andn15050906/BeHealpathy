namespace Contract.Requests.Courses.CourseRequests.Dtos;

public sealed class QueryYogaPoseDto : PagingQueryDto
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Level { get; set; }
}
