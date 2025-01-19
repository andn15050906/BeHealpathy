using Contract.Domain.Shared.MultimediaBase;
using Contract.Requests.Library.ArticleRequests.Dtos;

namespace Contract.Requests.Library.ArticleRequests;

public sealed class UpdateArticleCommand : UpdateCommand
{
    public UpdateArticleDto Rq { get; init; }
    public Guid UserId { get; init; }
    public List<Multimedia> AddedMedias { get; init; }
    public List<Guid> RemovedMedias { get; init; }



    public UpdateArticleCommand(
        UpdateArticleDto rq, Guid userId,
        List<Multimedia> addedMedias, List<Guid> removedMedias, bool isCompensating = false)
        : base(isCompensating)
    {
        Rq = rq;
        UserId = userId;
        AddedMedias = addedMedias;
        RemovedMedias = removedMedias;
    }
}