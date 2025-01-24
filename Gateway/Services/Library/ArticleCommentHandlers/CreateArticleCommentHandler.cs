using Contract.Domain.LibraryAggregate;
using Contract.Helpers;
using Contract.Requests.Library.ArticleCommentRequests;
using Contract.Responses.Shared;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Library.ArticleCommentHandlers;

public sealed class CreateArticleCommentHandler : RequestHandler<CreateArticleCommentCommand, CommentModel, HealpathyContext>
{
    public CreateArticleCommentHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result<CommentModel>> Handle(CreateArticleCommentCommand command, CancellationToken cancellationToken)
    {
        ArticleComment entity = Adapt(command);

        try
        {
            var commentTask = _context.ArticleComments.InsertExt(entity);
            var mediaTask = command.Medias is not null
                ? _context.Multimedia.AddRangeAsync(command.Medias.Where(_ => _ is not null))
                : Task.CompletedTask;
            await Task.WhenAll(commentTask, mediaTask);
            await _context.SaveChangesAsync(cancellationToken);

            var model = CommentModel.MapFunc(entity);
            if (command.Medias is not null)
            {
                model.Medias = command.Medias.Select(_ => MultimediaModel.MapFunc(_));
            }
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