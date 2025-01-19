using Contract.Messaging.Models;
using Contract.Responses.Identity;
using System.Security.Claims;

namespace Contract.Requests.Identity;

public class ExternalSignInCommand : IRequest<Result<AuthModel>>
{
    public ClaimsPrincipal Principal { get; init; }

    public ExternalSignInCommand(ClaimsPrincipal principal)
    {
        Principal = principal;
    }
}
