using Contract.Requests.Identity.Dtos;
using Contract.Responses.Identity;

namespace Contract.Requests.Identity;

public sealed class SignInCommand : IRequest<Result<AuthModel>>
{
    public SignInDto Rq { get; init; }

    public SignInCommand(SignInDto rq)
    {
        Rq = rq;
    }
}
