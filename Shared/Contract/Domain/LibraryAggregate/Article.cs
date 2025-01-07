namespace Contract.Domain.LibraryAggregate;

public sealed class Article : AuditedEntity
{
    // Attributes
    public string Content { get; set; }
    public string Title { get; set; }
    public string Status { get; set; }
    public bool IsCommentDisabled { get; set; }
    public int CommentCount { get; set; }
    public int ViewCount { get; set; }

    // Navigations
    public List<Tag> Tags { get; set; }
    public List<ArticleComment> Comments { get; set; }
    public List<ArticleReaction> Reactions { get; set; }

#pragma warning disable CS8618
    public Article()
    {

    }
#pragma warning restore CS8618
}
