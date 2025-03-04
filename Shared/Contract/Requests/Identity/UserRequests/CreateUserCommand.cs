using Contract.Requests.Identity.UserRequests.Dtos;

namespace Contract.Requests.Identity.UserRequests;

public sealed class CreateUserCommand : CreateCommand<string>
{
    public CreateUserDto Rq { get; set; }



    public CreateUserCommand(Guid id, CreateUserDto dto, bool isCompensating = false)
        : base(id, isCompensating)
    {
        Rq = dto;
    }
}