﻿using Contract.Domain.LibraryAggregate;
using Contract.Helpers;
using Contract.Requests.Library.ArticleCommentRequests;
using Core.Helpers;
using Infrastructure.DataAccess.SQLServer.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Services.Library.ArticleCommentHandlers;

public sealed class UpdateArticleCommentHandler : RequestHandler<UpdateArticleCommentCommand, HealpathyContext>
{
    public UpdateArticleCommentHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result> Handle(UpdateArticleCommentCommand command, CancellationToken cancellationToken)
    {
        var entity = await _context.ArticleComments.FirstOrDefaultAsync(_ => _.Id == command.Rq.Id);

        if (entity is null)
            return NotFound(string.Empty);
        if (entity.CreatorId != command.UserId)
            return Unauthorized(string.Empty);

        try
        {
            entity = ApplyChanges(entity, command);

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

    private ArticleComment ApplyChanges(ArticleComment entity, UpdateArticleCommentCommand command)
    {
        if (command.Rq.Content is not null)
            entity.Content = command.Rq.Content;
        entity.LastModifierId = command.UserId;
        entity.LastModificationTime = TimeHelper.Now;

        return entity;
    }
}