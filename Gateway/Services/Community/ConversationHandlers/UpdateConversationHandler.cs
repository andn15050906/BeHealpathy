using Contract.Domain.CommunityAggregate;
using Contract.Helpers;
using Contract.Requests.Community.ConversationRequests;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Services.Community.ConversationHandlers;

public sealed class UpdateConversationHandler(HealpathyContext context, IAppLogger logger, IEventCache cache)
    : RequestHandler<UpdateConversationCommand, HealpathyContext>(context, logger, cache)
{
    public override async Task<Result> Handle(UpdateConversationCommand command, CancellationToken cancellationToken)
    {
        var entity = await _context.Conversations.Include(_ => _.Members).FirstOrDefaultAsync(_ => _.Id == command.Rq.Id);

        if (entity is null)
            return NotFound(string.Empty);
        if (entity.CreatorId != command.UserId)
            return Unauthorized(string.Empty);

        var currentMemberIds = await _context.ConversationMembers
        .Where(_ => _.ConversationId == command.Rq.Id)
        .Select(_ => _.CreatorId)
        .ToListAsync();

        var addedUserIds = command.Rq.AddedMembers?
            .Select(m => m.UserId)
            .Where(userId => !currentMemberIds.Contains(userId))
            .ToList();

        var removedUserIds = command.Rq.RemovedMembers;

        var addedMembers = addedUserIds is not null && addedUserIds.Count > 0
            ? addedUserIds.Select(userId => new ConversationMember
            {
                ConversationId = command.Rq.Id,
                CreatorId = userId
            }).ToList()
            : null;

        var removedMembers = removedUserIds is not null && removedUserIds.Count > 0
            ? await _context.ConversationMembers
                .Where(_ => _.ConversationId == command.Rq.Id && removedUserIds.Contains(_.CreatorId))
                .ToListAsync()
            : null;

        try
        {
            entity = ApplyChanges(entity, command, addedMembers, removedMembers);

            if (command.Media is not null)
                await _context.Multimedia.AddAsync(command.Media);

            await _context.SaveChangesAsync(cancellationToken);

            if (addedMembers is not null)
            {
                foreach (var member in addedMembers)
                    _cache.Add(member.CreatorId, new Events.Conversation_Joined(entity.Id));
            }
            if (removedMembers is not null)
            {
                foreach (var member in removedMembers)
                    _cache.Add(member.CreatorId, new Events.Conversation_Left(entity.Id));
            }
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