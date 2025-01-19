using Contract.Domain.Shared.MultimediaBase;
using Contract.Requests.Library.MediaRequests.Dtos;

namespace Contract.Requests.Library.MediaRequests;

public sealed class UpdateMediaResourceCommand : UpdateCommand
{
    public UpdateMediaResourceDto Rq { get; init; }
    public Guid UserId { get; init; }
    public Multimedia Media { get; init; }



    public UpdateMediaResourceCommand(UpdateMediaResourceDto rq, Guid userId, Multimedia media, bool isCompensating = false)
        : base(isCompensating)
    {
        Rq = rq;
        UserId = userId;
        Media = media;
    }
}