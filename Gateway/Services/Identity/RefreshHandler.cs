using Contract.Helpers;
using Infrastructure.DataAccess.SQLServer.Helpers;
using System.Security.Claims;
using Contract.Responses.Identity;
using Contract.Requests.Identity.UserRequests;

namespace Gateway.Services.Identity;

public sealed class RefreshHandler : RequestHandler<RefreshCommand, AuthModel, /*GatewayContext*/ HealpathyContext>
{
    private readonly ITokenService _tokenService;

    public RefreshHandler(HealpathyContext context, IAppLogger logger, ITokenService tokenService) : base(context, logger)
    {
        _tokenService = tokenService;
    }

    public override async Task<Result<AuthModel>> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        string? accessToken = request.AccessToken;
        string? refreshToken = request.RefreshToken;

        if (accessToken is null || refreshToken is null)
            return Unauthorized("Missing credentials");

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
        // not expired -> invalid
        if (principal is null)
            return Unauthorized("Invalid access token");

        User? entity = await FindByClaims(principal);
        if (entity is null)
            return Unauthorized("Invalid access token");
        if (entity.RefreshToken != refreshToken)
            return Unauthorized("Invalid refresh token");

        AuthModel authDTO = RefreshAuthData(entity);
        //await _context.Update(entity.Id, Builders<User>.Update.Set(_ => _.RefreshToken, authDTO.RefreshToken));
        ApplyChanges(entity, request);
        await _context.SaveChangesAsync(cancellationToken);
        return Ok(authDTO);
    }



    private async Task<User?> FindByClaims(ClaimsPrincipal userClaim)
    {
        Guid? id = GetIdentifier(userClaim);
        if (id is null)
            return null;
        //return await _context.Find<User>((Guid)id);
        return await _context.Users.FindExt(id);
    }

    private static Guid? GetIdentifier(ClaimsPrincipal userClaim)
    {
        foreach (Claim claim in userClaim.Claims)
            if (claim.Type == ClaimTypes.NameIdentifier)
                return Guid.TryParse(claim.Value, out Guid result) ? result : null;
        return null;
    }

    private AuthModel RefreshAuthData(User user)
    {
        string accessToken = _tokenService.GenerateAccessToken(user.Id.ToString(), user.Role.ToString(), user.AdvisorId);
        string refreshToken = _tokenService.GenerateRefreshToken();

        user.SetRefreshToken(refreshToken);
        return new AuthModel(UserFullModel.From(user), accessToken, refreshToken);
    }

    private void ApplyChanges(User user, RefreshCommand command)
    {
        user.RefreshToken = command.RefreshToken!;
    }
}
