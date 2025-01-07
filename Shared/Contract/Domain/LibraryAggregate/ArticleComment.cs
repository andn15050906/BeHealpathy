using Contract.Domain.Shared.CommentBase;
using Contract.Domain.Shared.MultimediaBase;

namespace Contract.Domain.LibraryAggregate;

public sealed class ArticleComment : Comment
{
    // Navigations
    public List<ArticleComment> Replies { get; set; }
    public List<ArticleReaction> Reactions { get; set; }






#pragma warning disable CS8618
    public ArticleComment() : base()
    {

    }

    public ArticleComment(Guid id, Guid creatorId, Guid sourceId, string content, List<Multimedia> medias)
        : base(id, creatorId, sourceId, content, medias)
    {

    }
#pragma warning restore CS8618
}
