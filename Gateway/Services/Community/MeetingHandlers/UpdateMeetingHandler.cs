using Contract.Domain.CommunityAggregate;
using Contract.Helpers;
using Contract.Requests.Community.MeetingRequests;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Services.Community.MeetingHandlers;

public sealed class UpdateMeetingHandler : RequestHandler<UpdateMeetingCommand, HealpathyContext>
{
    public UpdateMeetingHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result> Handle(UpdateMeetingCommand command, CancellationToken cancellationToken)
    {
        var entity = await _context.Meetings.FirstOrDefaultAsync(_ => _.Id == command.Rq.Id);

        if (entity is null)
            return NotFound(string.Empty);
        if (entity.CreatorId != command.UserId)
            return Unauthorized(string.Empty);

        var addedParticipants = command.Rq.AddedParticipants is not null
            ? _context.MeetingParticipants
                .Where(_ => _.MeetingId == command.Rq.Id && command.Rq.AddedParticipants.Any(item => item.UserId == _.CreatorId))
                .ToList()
            : null;
        var removedParticipants = command.Rq.RemovedParticipants is not null
            ? _context.MeetingParticipants
                .Where(_ => _.MeetingId == command.Rq.Id && command.Rq.RemovedParticipants.Any(item => item == _.CreatorId))
                .ToList()
            : null;

        try
        {
            entity = ApplyChanges(entity, command, addedParticipants, removedParticipants);
            await _context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private Meeting ApplyChanges(
        Meeting entity, UpdateMeetingCommand command,
        List<MeetingParticipant>? addedParticipants, List<MeetingParticipant>? removedParticipants)
    {
        if (command.Rq.Title is not null)
            entity.Title = command.Rq.Title;
        if (command.Rq.Description is not null)
            entity.Description = command.Rq.Description;
        if (command.Rq.StartAt is not null)
            entity.StartAt = (DateTime)command.Rq.StartAt;
        if (command.Rq.EndAt is not null)
            entity.EndAt = (DateTime)command.Rq.EndAt;
        if (command.Rq.MaxParticipants is not null)
            entity.MaxParticipants = (int)command.Rq.MaxParticipants;

        if (addedParticipants is not null)
            entity.Participants.AddRange(addedParticipants);
        if (removedParticipants is not null)
            foreach (var member in removedParticipants)
                entity.Participants.Remove(member);

        return entity;
    }
}