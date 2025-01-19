using Contract.Domain.LibraryAggregate;
using Contract.Domain.LibraryAggregate.Enums;
using Contract.Domain.Shared.MultimediaBase.Enums;
using Contract.Helpers;
using Contract.Requests.Library.MediaRequests;
using Core.Helpers;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Library.MediaResourceHandlers;

public class UpdateMediaResourceHandler : RequestHandler<UpdateMediaResourceCommand, HealpathyContext>
{
    public UpdateMediaResourceHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result> Handle(UpdateMediaResourceCommand command, CancellationToken cancellationToken)
    {
        var entity = await _context.MediaResources.FindExt(command.Rq.Id);

        if (entity is null)
            return NotFound(string.Empty);
        if (entity.CreatorId != command.UserId)
            return Unauthorized(string.Empty);

        try
        {
            entity = ApplyChanges(entity, command);

            if (command.Media is not null)
                _context.Multimedia.Add(command.Media);
            //_context.Multimedia.Remove()

            await _context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private MediaResource ApplyChanges(MediaResource entity, UpdateMediaResourceCommand command)
    {
        if (command.Rq.Description is not null)
            entity.Description = command.Rq.Description;
        if (command.Rq.Artist is not null)
            entity.Artist = command.Rq.Artist;

        if (command.Media is not null)
        {
            entity.Title = command.Media.Title;
            entity.Type = command.Media.Type switch
            {
                MediaType.Image => MediaResourceType.Image,
                MediaType.Video => MediaResourceType.Video,
                _ => MediaResourceType.Audio,
            };
        }
        entity.LastModifierId = command.UserId;
        entity.LastModificationTime = TimeHelper.Now;

        return entity;
    }
}