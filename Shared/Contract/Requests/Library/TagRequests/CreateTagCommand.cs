using Contract.Requests.Library.TagRequests.Dtos;

namespace Contract.Requests.Library.TagRequests;

public sealed class CreateTagCommand : CreateCommand
{
    public CreateTagDto Rq { get; init; }



    public CreateTagCommand(Guid id, CreateTagDto rq, bool isCompensating = false)
        : base(id, isCompensating)
    {
        Rq = rq;
    }
}
