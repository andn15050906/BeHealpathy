using Contract.Helpers;
using Contract.Requests.Community.ConversationRequests;
using Contract.Responses.Community;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Services.Community.ConversationHandlers;

public class GetConversationByIdHandler : RequestHandler<GetConversationByIdQuery, ConversationModel, HealpathyContext>
{
    public GetConversationByIdHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result<ConversationModel>> Handle(GetConversationByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _context.Conversations
                .Include(_ => _.Members)
                .Where(_ => _.Id == request.Id && !_.IsDeleted)
                .Select(ConversationModel.MapExpression)
                .FirstOrDefaultAsync(cancellationToken);

            return ToQueryResult(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }
}