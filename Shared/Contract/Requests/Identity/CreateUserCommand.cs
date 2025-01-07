using Contract.Requests.Identity.Dtos;

namespace Contract.Requests.Identity;

public sealed class CreateUserCommand : CreateCommand<string>
{
    public CreateUserDto Rq { get; set; }



    public CreateUserCommand(Guid id, CreateUserDto dto, bool isCompensating = false)
        : base(id, isCompensating)
    {
        Rq = dto;
    }
}
