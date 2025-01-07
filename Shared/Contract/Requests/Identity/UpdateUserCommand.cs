using Contract.Requests.Identity.Dtos;
using Contract.Requests.Shared.Base;

namespace Contract.Requests.Identity;

public sealed class UpdateUserCommand : UpdateCommand
{
    public UpdateUserDto Rq { get; init; }
    public Guid UserId { get; init; }



    public UpdateUserCommand(UpdateUserDto rq, Guid userId, bool isCompensating = false)
        : base(isCompensating)
    {
        Rq = rq;
        UserId = userId;
    }
}
