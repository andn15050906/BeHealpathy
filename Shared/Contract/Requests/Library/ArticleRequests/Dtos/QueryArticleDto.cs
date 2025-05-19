namespace Contract.Requests.Library.ArticleRequests.Dtos;

public sealed class QueryArticleDto : PagingQueryDto
{
    public Guid? CreatorId { get; init; }

    public string? Title { get; set; }
    public string? Status { get; set; }

    public List<Guid>? Tags { get; set; }
}
