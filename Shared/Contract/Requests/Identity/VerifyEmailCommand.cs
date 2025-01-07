using Contract.Requests.Identity.Dtos;

namespace Contract.Requests.Identity;

public sealed class VerifyEmailCommand : IRequest<Result>
{
    public VerifyEmailDto Rq { get; set; }

    public VerifyEmailCommand(VerifyEmailDto rq)
    {
        Rq = rq;
    }
}
