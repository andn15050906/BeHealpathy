using Contract.Requests.Identity.UserRequests.Dtos;

namespace Contract.Requests.Identity.UserRequests;

public class ResetPasswordCommand : IRequest<Result>
{
    public ResetPasswordDto Dto { get; init; }

    public ResetPasswordCommand(ResetPasswordDto dto)
    {
        Dto = dto;
    }
}
