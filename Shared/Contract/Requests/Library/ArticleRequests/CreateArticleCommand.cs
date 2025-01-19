using Contract.Domain.Shared.MultimediaBase;
using Contract.Requests.Library.ArticleRequests.Dtos;

namespace Contract.Requests.Community.ArticleRequests;

public sealed class CreateArticleCommand : CreateCommand
{
    public CreateArticleDto Rq { get; init; }
    public Guid UserId { get; init; }
    public List<Multimedia>? Medias { get; init; }



    public CreateArticleCommand(Guid id, CreateArticleDto rq, Guid userId, List<Multimedia>? medias, bool isCompensating = false)
        : base(id, isCompensating)
    {
        Rq = rq;
        UserId = userId;
        Medias = medias;
    }
}
