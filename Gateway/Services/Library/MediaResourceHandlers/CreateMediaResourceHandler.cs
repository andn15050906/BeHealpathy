using Contract.Domain.LibraryAggregate;
using Contract.Domain.LibraryAggregate.Enums;
using Contract.Domain.Shared.MultimediaBase.Enums;
using Contract.Helpers;
using Contract.Requests.Library.MediaRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Library.MediaResourceHandlers;

public class CreateMediaResourceHandler : RequestHandler<CreateMediaResourceCommand, HealpathyContext>
{
    public CreateMediaResourceHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result> Handle(CreateMediaResourceCommand command, CancellationToken cancellationToken)
    {
        MediaResource entity = Adapt(command);

        try
        {
            var mediaResourceTask = _context.MediaResources.InsertExt(entity);
            var mediaTask = _context.Multimedia.InsertExt(command.Media);
            await Task.WhenAll(mediaResourceTask, mediaTask);
            await _context.SaveChangesAsync(cancellationToken);
            return Created();
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private MediaResource Adapt(CreateMediaResourceCommand command)
    {
        var type = command.Media.Type switch
        {
            MediaType.Image => MediaResourceType.Image,
            MediaType.Video => MediaResourceType.Video,
            _ => MediaResourceType.Audio,
        };

        return new MediaResource(
            command.Id, command.UserId,
            command.Rq.Description, command.Rq.Artist, command.Media.Title, type
        );
    }
}