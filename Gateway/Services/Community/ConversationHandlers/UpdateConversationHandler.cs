using Contract.Domain.CommunityAggregate;
using Contract.Helpers;
using Contract.Requests.Community.ConversationRequests;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Services.Community.ConversationHandlers;

public sealed class UpdateConversationHandler : RequestHandler<UpdateConversationCommand, HealpathyContext>
{
    public UpdateConversationHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result> Handle(UpdateConversationCommand command, CancellationToken cancellationToken)
    {
        var entity = await _context.Conversations.FirstOrDefaultAsync(_ => _.Id == command.Rq.Id);

        if (entity is null)
            return NotFound(string.Empty);
        if (entity.CreatorId != command.UserId)
            return Unauthorized(string.Empty);

        var addedMembers = command.Rq.AddedMembers is not null
            ? _context.ConversationMembers
                .Where(_ => _.ConversationId == command.Rq.Id && command.Rq.AddedMembers.Any(item => item.UserId == _.CreatorId))
                .ToList()
            : null;
        var removedMembers = command.Rq.RemovedMembers is not null
            ? _context.ConversationMembers
                .Where(_ => _.ConversationId == command.Rq.Id && command.Rq.RemovedMembers.Any(item => item == _.CreatorId))
                .ToList()
            : null;

        try
        {
            entity = ApplyChanges(entity, command, addedMembers, removedMembers);

            if (command.Media is not null)
                await _context.Multimedia.AddAsync(command.Media);

            await _context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private Conversation ApplyChanges(
        Conversation entity, UpdateConversationCommand command,
        List<ConversationMember>? addedMembers, List<ConversationMember>? removedMembers)
    {
        if (command.Rq.Title is not null)
            entity.Title = command.Rq.Title;
        if (command.Rq.IsPrivate is not null)
            entity.IsPrivate = (bool)command.Rq.IsPrivate;

        if (addedMembers is not null)
            entity.Members.AddRange(addedMembers);
        if (removedMembers is not null)
            foreach (var member in removedMembers)
                entity.Members.Remove(member);

        return entity;
    }
}