using Contract.Domain.Shared.MultimediaBase;
using Contract.Requests.Shared.BaseDtos.Comments;

namespace Contract.Requests.Shared.BaseRequests.Comments;

public class UpdateCommentCommand : UpdateCommand
{
    public UpdateCommentDto Rq { get; init; }
    public Guid UserId { get; init; }
    public List<Multimedia> AddedMedias { get; init; }
    public List<Guid> RemovedMedias { get; init; }



    public UpdateCommentCommand(
        UpdateCommentDto rq, Guid userId,
        List<Multimedia> addedMedias, List<Guid> removedMedias, bool isCompensating = false)
        : base(isCompensating)
    {
        Rq = rq;
        UserId = userId;
        AddedMedias = addedMedias;
        RemovedMedias = removedMedias;
    }
}
