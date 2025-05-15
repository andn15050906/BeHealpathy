using Contract.Domain.ToolAggregate;
using Contract.Helpers;
using Contract.Requests.Community.ArticleRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Library.ArticleHandlers;

public sealed class CreateArticleHandler(HealpathyContext context, IAppLogger logger, IEventCache cache)
    : RequestHandler<CreateArticleCommand, HealpathyContext>(context, logger, cache)
{
    public override async Task<Result> Handle(CreateArticleCommand command, CancellationToken cancellationToken)
    {
        var tags = _context.Tags.Where(_ => command.Rq.Tags.Contains(_.Id)).ToList();
        Article entity = Adapt(command, tags);

        try
        {
            var articleTask = _context.Articles.InsertExt(entity);
            var mediaTask = command.Medias is not null
                ? _context.Multimedia.AddRangeAsync(command.Medias.Where(_ => _ is not null))
                : Task.CompletedTask;
            await Task.WhenAll(articleTask, mediaTask);
            await _context.SaveChangesAsync(cancellationToken);

            _cache.Add(command.UserId, new Events.Article_Created(entity.Id));
            return Created();
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private Article Adapt(CreateArticleCommand command, List<Tag> tags)
    {
        var sections = command.Rq.Sections
            .Select(_ => new ArticleSection(_.Id, command.Id, _.Header, _.Content))
            .ToList();

        return new Article(
            command.Id, command.UserId,
            command.Rq.Title, command.Rq.Status, command.Rq.IsCommentDisabled, sections, tags
        );
    }
}