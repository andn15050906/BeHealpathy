using Contract.Responses.Identity.UserModels;
using System.Security.Claims;

namespace Contract.Responses.Identity.TempModels;

public sealed class AuthModel
{
    public UserFullModel? User { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public ClaimsPrincipal? Principle { get; set; }

    public AuthModel(UserFullModel user, string accessToken, string refreshToken)
    {
        User = user;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }

    public AuthModel(UserFullModel user, ClaimsPrincipal principal)
    {
        User = user;
        AccessToken = string.Empty;
        RefreshToken = string.Empty;
        Principle = principal;
    }
}
