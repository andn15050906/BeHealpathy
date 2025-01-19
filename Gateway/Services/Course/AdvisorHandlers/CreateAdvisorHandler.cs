using Contract.Domain.CourseAggregate;
using Contract.Helpers;
using Contract.Requests.Courses.AdvisorRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Course.AdvisorHandlers;

public class CreateAdvisorHandler : RequestHandler<CreateAdvisorCommand, HealpathyContext>
{
    public CreateAdvisorHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }

    public override async Task<Result> Handle(CreateAdvisorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = Adapt(request);
            await _context.Advisors.InsertExt(entity);
            await _context.Multimedia.AddRangeAsync(entity.Medias);
            await _context.SaveChangesAsync(cancellationToken);
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