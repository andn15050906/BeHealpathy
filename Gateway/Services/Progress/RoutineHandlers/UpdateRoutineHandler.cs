using Contract.Domain.ProgressAggregates;
using Contract.Domain.ProgressAggregates.Enums;
using Contract.Helpers;
using Contract.Requests.Progress.RoutineRequests;
using Core.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Services.Library.RoutineLogHandlers;

public sealed class UpdateRoutineHandler : RequestHandler<UpdateRoutineCommand, HealpathyContext>
{
    public UpdateRoutineHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



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
        if (command.Rq.Frequency is not null)
            entity.Frequency = (Frequency)command.Rq.Frequency;
        entity.LastModifierId = command.UserId;
        entity.LastModificationTime = TimeHelper.Now;

        return entity;
    }
}