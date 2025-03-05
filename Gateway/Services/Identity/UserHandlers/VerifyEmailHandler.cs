using Contract.Helpers;
using Contract.Requests.Identity.UserRequests;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Services.Identity.UserHandlers;

public sealed class VerifyEmailHandler : RequestHandler<VerifyEmailCommand, HealpathyContext>
{
    public VerifyEmailHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Users.FirstOrDefaultAsync(_ => _.Email == request.Rq.Email);
        if (entity is null)
            return Unauthorized(string.Empty);

        if (entity.Token == request.Rq.Token)
            entity.Verify();
        await _context.SaveChangesAsync();
        return Ok();
    }
}
