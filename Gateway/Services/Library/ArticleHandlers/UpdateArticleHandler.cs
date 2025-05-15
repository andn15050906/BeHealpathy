using Contract.Domain.ToolAggregate;
using Contract.Helpers;
using Contract.Requests.Library.ArticleRequests;
using Core.Helpers;
using Infrastructure.DataAccess.SQLServer.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Services.Library.ArticleHandlers;

public class UpdateArticleHandler : RequestHandler<UpdateArticleCommand, HealpathyContext>
{
    public UpdateArticleHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result> Handle(UpdateArticleCommand command, CancellationToken cancellationToken)
    {
        IQueryable<Article> query = _context.Articles;
        if (command.Rq.AddedTags is not null || command.Rq.RemovedTags is not null)
            query = query.Include(_ => _.Tags);
        var entity = await query.FirstOrDefaultAsync(_ => _.Id == command.Rq.Id);

        if (entity is null)
            return NotFound(string.Empty);
        if (entity.CreatorId != command.UserId)
            return Unauthorized(string.Empty);

        var addedTags = command.Rq.AddedTags is not null
            ? _context.Tags.Where(_ => command.Rq.AddedTags.Contains(_.Id)).ToList()
            : null;
        var removedTags = command.Rq.RemovedTags is not null
            ? _context.Tags.Where(_ => command.Rq.RemovedTags.Contains(_.Id)).ToList()
            : null;

        try
        {
            entity = ApplyChanges(entity, command, addedTags, removedTags);

            if (command.AddedMedias is not null && command.AddedMedias.Count > 0)
                _context.Multimedia.AddRange(command.AddedMedias);
            if (command.RemovedMedias is not null && command.RemovedMedias.Count > 0)
                await _context.Multimedia.DeleteExt(command.RemovedMedias);

            await _context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private Article ApplyChanges(Article entity, UpdateArticleCommand command, List<Tag>? addedTags, List<Tag>? removedTags)
    {
        if (command.Rq.Title is not null)
            entity.Title = command.Rq.Title;
        if (command.Rq.Status is not null)
            entity.Status = command.Rq.Status;
        if (command.Rq.IsCommentDisabled is not null)
            entity.IsCommentDisabled = (bool)command.Rq.IsCommentDisabled;
        entity.LastModifierId = command.UserId;
        entity.LastModificationTime = TimeHelper.Now;

        if (addedTags is not null)
            entity.Tags.AddRange(addedTags);
        if (removedTags is not null)
            foreach (var tag in removedTags)
                entity.Tags.Remove(tag);

        if (command.Rq.Sections is not null)
        {
            foreach (var section in command.Rq.Sections)
            {
                var updatedSection = _context.ArticleSections.FirstOrDefault(_ => _.Id == section.Id);

                if (updatedSection is not null)
                {
                    if (section.Header is not null)
                    {
                        updatedSection.Header = section.Header;
                    }
                    if (section.Content is not null)
                    {
                        updatedSection.Content = section.Content;
                    }
                }
            }
        }
        return entity;
    }
}