using Contract.Domain.Shared.MultimediaBase;
using Contract.Requests.Library.MediaRequests.Dtos;

namespace Contract.Requests.Library.MediaRequests;

public sealed class CreateMediaResourceCommand : CreateCommand
{
    public CreateMediaResourceDto Rq { get; init; }
    public Guid UserId { get; init; }
    public Multimedia Media { get; init; }



    public CreateMediaResourceCommand(Guid id, CreateMediaResourceDto rq, Guid userId, Multimedia media, bool isCompensating = false)
        : base(id, isCompensating)
    {
        Rq = rq;
        UserId = userId;
        Media = media;
    }
}
