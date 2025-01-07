using Contract.Requests.Library.MediaRequests.Dtos;

namespace Contract.Requests.Library.MediaRequests;

public sealed class CreateMediaResourceCommand : CreateCommand
{
    public CreateMediaResourceDto Rq { get; init; }
    public Guid UserId { get; init; }



    public CreateMediaResourceCommand(Guid id, CreateMediaResourceDto rq, Guid userId, bool isCompensating = false)
        : base(id, isCompensating)
    {
        Rq = rq;
        UserId = userId;
    }
}
