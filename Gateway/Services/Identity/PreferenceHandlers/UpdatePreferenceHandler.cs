using Contract.BusinessRules.PreferenceBiz;
using Contract.Helpers;
using Contract.Requests.Identity.PreferenceRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace Gateway.Services.Identity.PreferenceHandlers;

public sealed class UpdatePreferenceHandler : RequestHandler<UpdatePreferenceCommand, HealpathyContext>
{
    public UpdatePreferenceHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result> Handle(UpdatePreferenceCommand request, CancellationToken cancellationToken)
    {
        var surveys = PrefStore.Surveys.FirstOrDefault(_ => _.Id == request.Rq.SourceId);
        if (surveys is null)
            return NotFound(string.Empty);

        var values = surveys.Values.Where(_ => request.Rq.PreferenceValueIds.Contains(_.Key));
        var jsonValue = JsonSerializer.Serialize(values);

        var existingEntity = _context.Preferences
            .FirstOrDefault(_ => _.CreatorId == request.UserId && _.SourceId == request.Rq.SourceId);
        if (existingEntity is not null)
        {
            existingEntity.Value = jsonValue;
        }
        else
        {
            var entity = new Preference(Guid.NewGuid(), request.UserId, request.Rq.SourceId, jsonValue);
            await _context.Preferences.InsertExt(entity);
        }

        /*var surveys = PrefStore.Surveys.Where(_ => request.Rq.Any(
            dto => _.Id == dto.SourceId && _.Values.Any(value => value.Item1 == dto.PreferenceValueId))
        );
        var sourceIds = surveys.Select(_ => _.Id);

        var existingEntities = _context.Preferences
            .Where(_ => _.CreatorId == request.UserId && sourceIds.Contains(_.SourceId))
            .ToList();

        foreach (var dto in request.Rq)
        {
            var correspondingDto = request.Rq.First(_ => _.SourceId == dto.SourceId);
            var correspondingSurvey = surveys.First(_ => _.Id == dto.SourceId);
            var value = correspondingSurvey.Values.FirstOrDefault(_ => _.Item1 == correspondingDto.PreferenceValueId).Item2;

            var existingEntity = existingEntities.FirstOrDefault(_ => _.SourceId == _.SourceId);
            if (existingEntity is not null)
            {
                existingEntity.Value = value;
            }
            else
            {
                var entity = new Preference(Guid.NewGuid(), request.UserId, dto.SourceId, value);
                await _context.Preferences.InsertExt(entity);
            }
        }*/

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(string.Empty);
        }
    }
}