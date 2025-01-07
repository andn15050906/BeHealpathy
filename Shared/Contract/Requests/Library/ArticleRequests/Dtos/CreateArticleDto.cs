namespace Contract.Requests.Library.ArticleRequests.Dtos;

public sealed class CreateArticleDto
{
    public string Content { get; set; }
    public string Title { get; set; }
    public string Status { get; set; }
    public bool IsCommentDisabled { get; set; }

    public List<Guid> Tags { get; set; }
}
