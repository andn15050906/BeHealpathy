using Contract.Domain.ToolAggregate;
using Contract.Domain.ToolAggregate.Enums;
using Contract.Helpers;
using Contract.Requests.Progress.RoutineRequests;
using Core.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Services.Progress.RoutineHandlers;

public sealed class UpdateRoutineHandler(HealpathyContext context, IAppLogger logger, IEventCache cache)
    : RequestHandler<UpdateRoutineCommand, HealpathyContext>(context, logger, cache)
{
    public override async Task<Result> Handle(UpdateRoutineCommand command, CancellationToken cancellationToken)
    {
        var entity = await _context.Routines.FirstOrDefaultAsync(_ => _.Id == command.Rq.Id);

        if (entity is null)
            return NotFound(string.Empty);
        if (entity.CreatorId != command.UserId)
            return Unauthorized(string.Empty);

        try
        {
            entity = ApplyChanges(entity, command);
            await _context.SaveChangesAsync(cancellationToken);

            _cache.Add(command.UserId, new Events.Routine_Updated(entity.Id));
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private Routine ApplyChanges(Routine entity, UpdateRoutineCommand command)
    {
        if (command.Rq.Title is not null)
            entity.Title = command.Rq.Title;
        if (command.Rq.Description is not null)
            entity.Description = command.Rq.Description;
        if (command.Rq.Objective is not null)
            entity.Objective = command.Rq.Objective;
        if (command.Rq.Repeater is not null)
            entity.Repeater = (Frequency)command.Rq.Repeater;
        if (command.Rq.RepeaterSequenceId is not null)
            entity.RepeaterSequenceId = command.Rq.RepeaterSequenceId;
        if (command.Rq.StartDate is not null)
            entity.StartDate = ((DateTime)command.Rq.StartDate).ToLocalTime();
        if (command.Rq.EndDate is not null)
            entity.EndDate = ((DateTime)command.Rq.EndDate).ToLocalTime();
        if (command.Rq.IsCompleted is not null)
            entity.IsCompleted = (bool)command.Rq.IsCompleted;
        if (command.Rq.IsClosed is not null)
            entity.IsClosed = (bool)command.Rq.IsClosed;
        if (command.Rq.Tag is not null)
            entity.Tag = (int)command.Rq.Tag;
        entity.LastModifierId = command.UserId;
        entity.LastModificationTime = TimeHelper.Now;

        return entity;
    }
}