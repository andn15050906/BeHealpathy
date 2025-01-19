using Contract.Domain.Shared.ReactionBase;

namespace Contract.Domain.LibraryAggregate;

public sealed class ArticleReaction : Reaction
{
    public ArticleReaction() : base()
    {

    }

    public ArticleReaction(Guid id, Guid creatorId, Guid sourceEntityId, string content) : base(id, creatorId, sourceEntityId, content)
    {

    }
}
