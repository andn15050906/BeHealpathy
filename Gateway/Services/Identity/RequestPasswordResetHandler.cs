using Contract.Helpers;
using Contract.Requests.Identity;
using Infrastructure.Helpers.Email;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Services.Identity;

public sealed class RequestPasswordResetHandler : RequestHandler<RequestPasswordResetCommand, string, HealpathyContext>
{
    public RequestPasswordResetHandler(HealpathyContext context, EmailService emailService, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result<string>> Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken)
    {
        User? user = await _context.Users.FirstOrDefaultAsync(_ => _.Email == request.Email);
        if (user is null)
            return Unauthorized(BusinessMessages.User.INVALID_EMAIL);

        try
        {
            user.GenerateToken();
            await _context.SaveChangesAsync();
            return Ok(user.Token);
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }
}
