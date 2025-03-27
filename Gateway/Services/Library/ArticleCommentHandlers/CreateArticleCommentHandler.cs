using Contract.Domain.LibraryAggregate;
using Contract.Helpers;
using Contract.Requests.Library.ArticleCommentRequests;
using Contract.Responses.Shared;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Library.ArticleCommentHandlers;

public sealed class CreateArticleCommentHandler(HealpathyContext context, IAppLogger logger, IEventCache cache)
    : RequestHandler<CreateArticleCommentCommand, CommentModel, HealpathyContext>(context, logger, cache)
{
    public override async Task<Result<CommentModel>> Handle(CreateArticleCommentCommand command, CancellationToken cancellationToken)
    {
        ArticleComment entity = Adapt(command);

        try
        {
            var commentTask = _context.ArticleComments.InsertExt(entity);
            var mediaTask = Task.CompletedTask;
            if (command.Medias is not null)
            {
                var medias = command.Medias.Where(_ => _ is not null);
                if (medias.Any())
                    mediaTask = _context.Multimedia.AddRangeAsync(medias);
            }
            await Task.WhenAll(commentTask, mediaTask);
            await _context.SaveChangesAsync(cancellationToken);

            var model = CommentModel.MapFunc(entity);
            if (command.Medias is not null)
            {
                model.Medias = command.Medias.Select(_ => MultimediaModel.MapFunc(_));
            }

            _cache.Add(command.UserId, new Events.ArticleComment_Created(entity.Id));
            return Created(model);
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private static ArticleComment Adapt(CreateArticleCommentCommand command)
    {
        return new ArticleComment(command.Id, command.UserId, command.Rq.SourceId, command.Rq.Content);
    }
}