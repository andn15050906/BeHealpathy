using Contract.Domain.Shared.MultimediaBase;
using Contract.Requests.Shared.BaseDtos.Reviews;

namespace Contract.Requests.Shared.BaseRequests.Reviews;

public class CreateReviewCommand : CreateCommand
{
    public CreateReviewDto Rq { get; init; }
    public Guid UserId { get; init; }
    public List<Multimedia> Medias { get; init; }



    public CreateReviewCommand(Guid id, CreateReviewDto rq, Guid userId, List<Multimedia> medias, bool isCompensating = false)
        : base(id, isCompensating)
    {
        Rq = rq;
        UserId = userId;
        Medias = medias;
    }
}
