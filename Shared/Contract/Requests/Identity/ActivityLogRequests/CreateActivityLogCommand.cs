using Contract.Requests.Identity.ActivityLogRequests.Dtos;

namespace Contract.Requests.Identity.ActivityLogRequests;

// Not used in mediators
public sealed class CreateActivityLogCommand : CreateCommand
{
    public List<CreateActivityLogDto> Dtos { get; init; }
    public Guid? UserId { get; init; }



    public CreateActivityLogCommand(Guid id, List<CreateActivityLogDto> dtos, Guid? userId, bool isCompensating = false) : base(id, isCompensating)
    {
        Dtos = dtos;
        UserId = userId;
    }
}