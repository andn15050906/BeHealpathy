using Contract.Domain.Shared.MultimediaBase;
using Contract.Requests.Shared.BaseDtos.Reviews;

namespace Contract.Requests.Shared.BaseRequests.Reviews;

public class UpdateReviewCommand : UpdateCommand
{
    public UpdateReviewDto Rq { get; init; }
    public Guid UserId { get; init; }
    public List<Multimedia> AddedMedias { get; init; }
    public List<Guid> RemovedMedias { get; init; }



    public UpdateReviewCommand(
        UpdateReviewDto rq, Guid userId,
        List<Multimedia> addedMedias, List<Guid> removedMedias, bool isCompensating = false)
        : base(isCompensating)
    {
        Rq = rq;
        UserId = userId;
        AddedMedias = addedMedias;
        RemovedMedias = removedMedias;
    }
}
