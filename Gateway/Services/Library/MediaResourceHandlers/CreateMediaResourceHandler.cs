using Contract.Domain.LibraryAggregate;
using Contract.Domain.LibraryAggregate.Enums;
using Contract.Domain.Shared.MultimediaBase.Enums;
using Contract.Helpers;
using Contract.Requests.Library.MediaRequests;
using Contract.Responses.Library;
using Contract.Responses.Shared;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Library.MediaResourceHandlers;

public class CreateMediaResourceHandler(HealpathyContext context, IAppLogger logger, IEventCache cache)
    : RequestHandler<CreateMediaResourceCommand, MediaResourceModel, HealpathyContext>(context, logger, cache)
{
    public override async Task<Result<MediaResourceModel>> Handle(CreateMediaResourceCommand command, CancellationToken cancellationToken)
    {
        MediaResource entity = Adapt(command);

        try
        {
            var mediaResourceTask = _context.MediaResources.InsertExt(entity);
            var mediaTask = _context.Multimedia.InsertExt(command.Media);
            await Task.WhenAll(mediaResourceTask, mediaTask);
            await _context.SaveChangesAsync(cancellationToken);

            var model = MediaResourceModel.MapFunc(entity);
            model.Media = MultimediaModel.MapFunc(command.Media);
            _cache.Add(command.UserId, new Events.MediaResource_Created(entity.Id));
            return Created(model);
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private static MediaResource Adapt(CreateMediaResourceCommand command)
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