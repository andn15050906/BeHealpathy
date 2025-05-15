using Contract.Helpers;
using Contract.Requests.Identity.UserRequests;
using Contract.Responses.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Services.Identity.UserHandlers;

public sealed class GetUserByIdHandler : RequestHandler<GetUserByIdQuery, UserFullModel, HealpathyContext>
{
    public GetUserByIdHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result<UserFullModel>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _context.Users
                /*
                .Include(_ => _.Preferences).Include(_ => _.Settings)
                */
                .Where(_ => _.Id == request.Id && !_.IsDeleted)
                .Select(UserFullModel.MapExpression)
                .FirstOrDefaultAsync(cancellationToken);

            return ToQueryResult(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }
}
