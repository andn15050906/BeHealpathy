using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Contract.Messaging.ApiClients.Http;

public static class HttpContextExtensions
{
    public const string BEARER = "Bearer";
    public const string REFRESH = "Refresh";
    public const string INSTRUCTOR_CLAIM = "InstructorClaim";

    public static Guid? GetClientId(this HttpContext httpContext)
    {
        foreach (Claim claim in httpContext.User.Claims)
            if (claim.Type == ClaimTypes.NameIdentifier)
                // there might be multiple NameIdentifiers
                if (Guid.TryParse(claim.Value, out Guid result))
                    return result;
        return null;
    }

    public static Guid? GetInstructorId(this HttpContext httpContext)
    {
        foreach (Claim claim in httpContext.User.Claims)
            if (claim.Type == INSTRUCTOR_CLAIM)
                // there might be multiple NameIdentifiers
                if (Guid.TryParse(claim.Value, out Guid result))
                    return result;
        return null;
    }
}