using Contract.Requests.Library.ArticleRequests.Dtos;

namespace Contract.Requests.Community.ArticleRequests;

public sealed class CreateArticleCommand : CreateCommand
{
    public CreateArticleDto Rq { get; init; }
    public Guid UserId { get; init; }



    public CreateArticleCommand(Guid id, CreateArticleDto rq, Guid userId, bool isCompensating = false)
        : base(id, isCompensating)
    {
        Rq = rq;
        UserId = userId;
    }
}
