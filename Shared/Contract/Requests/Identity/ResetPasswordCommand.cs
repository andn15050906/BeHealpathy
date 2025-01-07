using Contract.Requests.Identity.Dtos;

namespace Contract.Requests.Identity;

public class ResetPasswordCommand : IRequest<Result>
{
    public ResetPasswordDto Dto { get; init; }

    public ResetPasswordCommand(ResetPasswordDto dto)
    {
        Dto = dto;
    }
}
