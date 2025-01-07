using Contract.Helpers;
using Contract.Requests.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Services.Identity;

public sealed class ResetPasswordHandler : RequestHandler<ResetPasswordCommand, HealpathyContext>
{
    public ResetPasswordHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        User? user = await _context.Users.FirstOrDefaultAsync(_ => _.Email == request.Dto.Email);
        if (user is null || user.Token != request.Dto.Token)
            return BadRequest(BusinessMessages.User.INVALID_RESETPASSWORD_ATTEMPT);

        try
        {
            user.SetPassword(request.Dto.NewPassword);
            user.GenerateToken();
            await _context.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }
}
