using Contract.Requests.Shared.Base;

namespace Contract.Requests.Library.ArticleRequests.Dtos;

public sealed class QueryArticleDto : PagingQueryDto
{
    public string? Content { get; set; }
    public string? Title { get; set; }
    public string? Status { get; set; }
    public Guid? TagId { get; set; }
}
