using Contract.Domain.Shared.CommentBase;

namespace Contract.Domain.ToolAggregate;

public sealed class ArticleComment : Comment
{
    // Navigations
    public List<ArticleComment> Replies { get; set; }
    public List<ArticleReaction> Reactions { get; set; }






#pragma warning disable CS8618
    public ArticleComment() : base()
    {

    }

    public ArticleComment(Guid id, Guid creatorId, Guid sourceId, string content)
        : base(id, creatorId, sourceId, content)
    {

    }
#pragma warning restore CS8618
}
