using Contract.Domain.Shared.MultimediaBase;
using Contract.Requests.Courses.AdvisorRequests.Dtos;

namespace Contract.Requests.Courses.AdvisorRequests;

public sealed class CreateAdvisorCommand : CreateCommand
{
    public CreateAdvisorDto Rq { get; set; }
    public Guid UserId { get; set; }
    public List<Multimedia> Medias { get; set; }



    public CreateAdvisorCommand(Guid id, CreateAdvisorDto rq, Guid userId, List<Multimedia> medias, bool isCompensating = false)
        : base(id, isCompensating)
    {
        Rq = rq;
        UserId = userId;
        Medias = medias;
    }
}
