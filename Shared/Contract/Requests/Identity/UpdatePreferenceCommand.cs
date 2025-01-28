using Contract.Requests.Identity.Dtos;

namespace Contract.Requests.Identity;

public sealed class UpdatePreferenceCommand : UpdateCommand
{
    public UpdatePreferenceDto Rq { get; set; }
    public Guid UserId { get; init; }



    public UpdatePreferenceCommand(UpdatePreferenceDto rq, Guid userId, bool isCompensating = false) : base(isCompensating)
    {
        Rq = rq;
        UserId = userId;
    }
}