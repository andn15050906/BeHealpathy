using Contract.Domain.Shared.MultimediaBase;
using Contract.Requests.Shared.BaseDtos.Comments;
using Contract.Requests.Shared.BaseRequests.Comments;

namespace Contract.Requests.Library.ArticleCommentRequests;

public sealed class UpdateArticleCommentCommand : UpdateCommentCommand
{
    public UpdateArticleCommentCommand(
        UpdateCommentDto rq, Guid userId,
        List<Multimedia> addedMedias, List<Guid> removedMedias, bool isCompensating = false)
        : base(rq, userId, addedMedias, removedMedias, isCompensating)
    {
    }
}
