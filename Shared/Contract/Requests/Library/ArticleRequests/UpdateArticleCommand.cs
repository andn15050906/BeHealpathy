using Contract.Requests.Library.ArticleRequests.Dtos;

namespace Contract.Requests.Library.ArticleRequests;

public sealed class UpdateArticleCommand : UpdateCommand
{
    public UpdateArticleDto Rq { get; init; }
    public Guid UserId { get; init; }



    public UpdateArticleCommand(UpdateArticleDto rq, Guid userId, bool isCompensating = false)
        : base(isCompensating)
    {
        Rq = rq;
        UserId = userId;
    }
}