using Contract.Requests.Identity.UserRequests.Dtos;

namespace Contract.Requests.Identity.UserRequests;

public sealed class VerifyEmailCommand : IRequest<Result>
{
    public VerifyEmailDto Rq { get; set; }

    public VerifyEmailCommand(VerifyEmailDto rq)
    {
        Rq = rq;
    }
}
