using Contract.Requests.Shared.BaseDtos.Reactions;
using Contract.Requests.Shared.BaseRequests.Reactions;

namespace Contract.Requests.Library.ArticleReactionRequests;

public sealed class CreateArticleReactionCommand : CreateReactionCommand
{
    public CreateArticleReactionCommand(Guid id, CreateReactionDto rq, Guid userId, bool isCompensating = false) : base(id, rq, userId, isCompensating)
    {
    }
}
