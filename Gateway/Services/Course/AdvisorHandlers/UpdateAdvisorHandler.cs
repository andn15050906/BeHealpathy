using Contract.Domain.CourseAggregate;
using Contract.Helpers;
using Contract.Requests.Courses.AdvisorRequests;
using Contract.Requests.Courses.AdvisorRequests.Dtos;
using Infrastructure.DataAccess.SQLServer.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Services.Course.AdvisorHandlers;

public class UpdateAdvisorHandler : RequestHandler<UpdateAdvisorCommand, HealpathyContext>
{
    public UpdateAdvisorHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }

    public override async Task<Result> Handle(UpdateAdvisorCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _context.Advisors.Where(_ => _.CreatorId == command.UserId).FirstOrDefaultAsync(cancellationToken);
            if (entity is null)
                return NotFound(string.Empty);

            if (command.AddedMedias is not null)
                _context.Multimedia.AddRange(command.AddedMedias);
            if (command.RemovedMedias is not null)
                await _context.Multimedia.DeleteExt(command.RemovedMedias);
            ApplyChanges(entity, command.Rq, command.UserId);
            await _context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private void ApplyChanges(Advisor entity, UpdateAdvisorDto dto, Guid userId)
    {
        if (dto.Intro is not null)
            entity.Intro = dto.Intro;
        if (dto.Experience is not null)
            entity.Experience = dto.Experience;
        if (dto.Balance is not null)
            entity.Balance = (long)dto.Balance;
    }
}
