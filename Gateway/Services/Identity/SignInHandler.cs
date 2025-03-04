using Contract.Helpers;
using Microsoft.EntityFrameworkCore;
using Contract.Responses.Identity;
using Contract.Requests.Identity.UserRequests;

namespace Gateway.Services.Identity;

public class SignInHandler : RequestHandler<SignInCommand, AuthModel, HealpathyContext>
{
    private const int ALLOWED_ACCESS_FAILED_COUNT = 5;
    private readonly ITokenService _tokenService;

    public SignInHandler(HealpathyContext context, IAppLogger logger, ITokenService tokenService) : base(context, logger)
    {
        _tokenService = tokenService;
    }



    public override async Task<Result<AuthModel>> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        User? entity;
        if (request.Rq.UserName is not null)
        {
            entity = await _context.Users
                .Include(_ => _.Preferences).Include(_ => _.Settings)
                .FirstOrDefaultAsync(_ => _.UserName == request.Rq.UserName);
        }
        else if (request.Rq.Email is not null)
        {
            entity = await _context.Users
                .Include(_ => _.Preferences).Include(_ => _.Settings)
                .FirstOrDefaultAsync(_ => _.Email == request.Rq.Email);
        }
        else
        {
            return BadRequest(BusinessMessages.User.INVALID_EMAILPHONE_MISSING);
        }

        if (entity is null)
            return Unauthorized(BusinessMessages.User.UNAUTHORIZED_SIGNIN);
        //...
        /*if (entity.IsNotApproved())
            return Forbidden(BusinessMessages.User.FORBIDDEN_NOT_APPROVED);*/
        if (entity.AccessFailedCount > ALLOWED_ACCESS_FAILED_COUNT)
            return Forbidden(BusinessMessages.User.FORBIDDEN_FAILED_EXCEED);

        if (!User.IsMatchPasswords(request.Rq.Password, entity.Password))
        {
            entity.IncreaseAccessFailedCount();
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Warn(ex.Message);
                return ServerError(ex.Message);
            }
            return Unauthorized(BusinessMessages.User.UNAUTHORIZED_SIGNIN);
        }


        entity.ResetAccessFailedCount();
        AuthModel authDTO = UpdateToken(_tokenService, entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Ok(authDTO);
    }

    private AuthModel UpdateToken(ITokenService tokenService, User user)
    {
        string accessToken = tokenService.GenerateAccessToken(user.Id.ToString(), user.Role.ToString(), user.AdvisorId);
        string refreshToken = tokenService.GenerateRefreshToken();

        user.SetRefreshToken(refreshToken);
        return new AuthModel(UserFullModel.From(user), accessToken, refreshToken);
    }
}
