using Contract.Domain.UserAggregate.Enums;
using Infrastructure.DataAccess.SQLServer.Helpers;
using Microsoft.EntityFrameworkCore;
using Contract.Helpers;
using Contract.Requests.Identity.UserRequests;

namespace Gateway.Services.Identity.UserHandlers;

public sealed class CreateUserHandler : RequestHandler<CreateUserCommand, string, HealpathyContext>
{
    public CreateUserHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result<string>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        if (await _context.Users.AsNoTracking().AnyAsync(_ => _.Email == command.Rq.Email))
            return Conflict(BusinessMessages.User.CONFLICT_EMAIL);
        if (await _context.Users.AsNoTracking().AnyAsync(_ => _.UserName == command.Rq.UserName))
            return Conflict(BusinessMessages.User.CONFLICT_USERNAME);

        User entity = Adapt(command, Role.Member);
        try
        {
            await _context.Users.InsertExt(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Created(entity.Token);
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private User Adapt(CreateUserCommand command, Role role)
    {
        return new User(command.Id, command.Rq.UserName, command.Rq.Password, command.Rq.Email, role);
    }
}
