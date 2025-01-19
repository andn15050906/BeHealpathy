using Contract.Domain.Shared.MultimediaBase;
using Contract.Requests.Courses.AdvisorRequests.Dtos;

namespace Contract.Requests.Courses.AdvisorRequests;

public sealed class UpdateAdvisorCommand : UpdateCommand
{
    public UpdateAdvisorDto Rq { get; init; }
    public Guid UserId { get; init; }
    public List<Multimedia> AddedMedias { get; init; }
    public List<Guid> RemovedMedias { get; init; }



    public UpdateAdvisorCommand(
        UpdateAdvisorDto rq, Guid userId,
        List<Multimedia> addedMedias, List<Guid> removedMedias, bool isCompensating = false)
        : base(isCompensating)
    {
        Rq = rq;
        UserId = userId;
        AddedMedias = addedMedias;
        RemovedMedias = removedMedias;
    }
}
