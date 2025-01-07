using System.Security.Claims;

namespace Contract.Services.Contracts;

public interface ITokenService
{
    string GenerateAccessToken(string identifier, string role, Guid? instructorId);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}