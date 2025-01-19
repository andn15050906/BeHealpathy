using Contract.Domain.LibraryAggregate;
using Contract.Helpers;
using Contract.Requests.Library.TagRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Library.TagHandlers;

public class CreateTagHandler : RequestHandler<CreateTagCommand, HealpathyContext>
{
    public CreateTagHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result> Handle(CreateTagCommand command, CancellationToken cancellationToken)
    {
        Tag entity = Adapt(command);

        try
        {
            await _context.Tags.InsertExt(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Created();
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private Tag Adapt(CreateTagCommand command)
    {
        return new Tag(command.Rq.Title);
    }
}