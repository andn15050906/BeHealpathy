using Contract.Requests.Shared.BaseRequests.Reactions;

namespace Contract.Requests.Library.ArticleReactionRequests;

public sealed class DeleteArticleReactionCommand : DeleteReactionCommand
{
    public DeleteArticleReactionCommand(Guid id, Guid userId, bool isCompensating = false) : base(id, userId, isCompensating)
    {
    }
}
