using Contract.Domain.CourseAggregate;
using Contract.Helpers;
using Contract.Requests.Courses.AdvisorRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Course.AdvisorHandlers;

public class CreateAdvisorHandler(HealpathyContext context, IAppLogger logger, IEventCache cache)
    : RequestHandler<CreateAdvisorCommand, HealpathyContext>(context, logger, cache)
{
    public override async Task<Result> Handle(CreateAdvisorCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var entity = Adapt(command);
            await _context.Advisors.InsertExt(entity);
            await _context.Multimedia.AddRangeAsync(entity.Medias);
            await _context.SaveChangesAsync(cancellationToken);

            _cache.Add(command.UserId, new Events.Advisor_Created(entity.Id));
            return Created();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }



    private Advisor Adapt(CreateAdvisorCommand command)
    {
        return new(command.Id, command.UserId, command.Rq.Intro, command.Rq.Experience, command.Medias);
    }
}