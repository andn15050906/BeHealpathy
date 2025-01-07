namespace Contract.Requests.Library.ArticleRequests.Dtos;

public sealed class UpdateArticleDto
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public string Title { get; set; }
    public string Status { get; set; }
    public bool IsCommentDisabled { get; set; }

    public List<Guid>? RemovedTags { get; set; }
    public List<Guid>? AddedTags { get; set; }
}
