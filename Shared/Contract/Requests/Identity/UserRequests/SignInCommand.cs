using Contract.Requests.Identity.UserRequests.Dtos;
using Contract.Responses.Identity;

namespace Contract.Requests.Identity.UserRequests;

public sealed class SignInCommand : IRequest<Result<AuthModel>>
{
    public SignInDto Rq { get; init; }

    public SignInCommand(SignInDto rq)
    {
        Rq = rq;
    }
}
