using Contract.Requests.Shared.BaseRequests.Comments;

namespace Contract.Requests.Library.ArticleCommentRequests;

public sealed class DeleteArticleCommentCommand : DeleteCommentCommand
{
    public DeleteArticleCommentCommand(Guid id, Guid userId, bool isCompensating = false) : base(id, userId, isCompensating)
    {
    }
}
