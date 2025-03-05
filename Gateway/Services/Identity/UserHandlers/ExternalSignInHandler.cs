using Contract.Domain.UserAggregate.Enums;
using Contract.Helpers;
using Contract.Requests.Identity.UserRequests;
using Contract.Responses.Identity;
using Infrastructure.DataAccess.SQLServer.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Gateway.Services.Identity.UserHandlers;

public class ExternalSignInHandler : RequestHandler<ExternalSignInCommand, AuthModel, HealpathyContext>
{
    private readonly ITokenService _tokenService;

    public ExternalSignInHandler(HealpathyContext context, IAppLogger logger, ITokenService tokenService) : base(context, logger)
    {
        _tokenService = tokenService;
    }

    public override async Task<Result<AuthModel>> Handle(ExternalSignInCommand request, CancellationToken cancellationToken)
    {
        if (request.Principal.Identity is null)
            return Unauthorized(BusinessMessages.User.INVALID_SIGN_IN);

        // Requires email
        string? email = request.Principal.FindFirstValue(ClaimTypes.Email);
        if (email is null)
            return Unauthorized(BusinessMessages.User.INVALID_SIGN_IN);

        User? entity = await _context.Users.Include(_ => _.UserLogins).FirstOrDefaultAsync(_ => _.Email == email);
        var identity = request.Principal.Identity as ClaimsIdentity;
        if (identity is null)
            return Unauthorized(BusinessMessages.User.INVALID_SIGN_IN);
        Claim? providerIdentifier = identity.FindFirst(ClaimTypes.NameIdentifier);
        if (providerIdentifier is null)
            return Unauthorized(BusinessMessages.User.INVALID_SIGN_IN);

        if (entity is null)
        {
            // Register if the user does not have an account
            entity = new User(Guid.NewGuid(), identity.AuthenticationType!, providerIdentifier.Value, email, identity.Name!, Role.Member);
            await _context.Users.InsertExt(entity);
        }
        else
        {
            // Update user if the user has an account
            var existingLogin = entity.UserLogins.FirstOrDefault(_ => _.LoginProvider == identity.AuthenticationType);

            if (existingLogin is null)
            {
                // Add another login provider to the user with existing email
                entity.UserLogins.Add(new UserLogin(identity.AuthenticationType!, providerIdentifier.Value));
            }
            else
            {
                if (existingLogin.ProviderKey != providerIdentifier.Value)
                    return Unauthorized(BusinessMessages.User.INVALID_SIGN_IN);
            }
            entity.ResetAccessFailedCount();
        }

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }

        // SignIn
        if (entity.IsNotApproved())
            return Forbidden(BusinessMessages.User.FORBIDDEN_NOT_APPROVED);

        var customClaims = identity.Claims.ToList();
        customClaims.Add(new Claim(ClaimTypes.NameIdentifier, entity.Id.ToString()));
        customClaims.Add(new Claim(ClaimTypes.Role, entity.Role.ToString()));
        var newPrinciple = new ClaimsPrincipal(new ClaimsIdentity(customClaims, identity.AuthenticationType));
        //...
        return Ok(new AuthModel(UserFullModel.From(entity), newPrinciple));
    }
}
