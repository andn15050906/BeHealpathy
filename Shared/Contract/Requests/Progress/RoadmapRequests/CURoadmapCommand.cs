using Contract.Domain.Shared.MultimediaBase;
using Contract.Requests.Progress.RoadmapRequests.Dtos;
using Contract.Responses.Progress;

namespace Contract.Requests.Progress.RoadmapRequests;

public sealed class CURoadmapCommand : IRequest<Result<RoadmapModel>>
{
    public Guid Id { get; init; }
    public bool IsCompensating { get; set; }
    public CURoadmapDto Rq { get; init; }
    public Guid UserId { get; init; }
    public List<Multimedia> Medias { get; init; }



    public CURoadmapCommand(
        Guid id, CURoadmapDto rq, Guid userId,
        List<Multimedia> medias, bool isCompensating = false)
    {
        Id = id;
        IsCompensating = isCompensating;

        Rq = rq;
        UserId = userId;
        Medias = medias;
    }
}