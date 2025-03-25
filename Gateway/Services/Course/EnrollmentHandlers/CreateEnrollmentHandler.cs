using Contract.Domain.CourseAggregate;
using Contract.Helpers;
using Contract.Requests.Courses.EnrollmentRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Course.EnrollmentHandlers;

public class CreateEnrollmentHandler(HealpathyContext context, IAppLogger logger, IEventCache cache)
    : RequestHandler<CreateEnrollmentCommand, HealpathyContext>(context, logger, cache)
{
    public override async Task<Result> Handle(CreateEnrollmentCommand command, CancellationToken cancellationToken)
    {
        try
        {
            if (command.Rq.BillId is null && !command.Rq.IsGranted)
                return BadRequest(BusinessMessages.Enrollment.INVALID_BILL_OR_GRANTED);

            var entity = Adapt(command);
            await _context.Enrollments.InsertExt(entity);
            await _context.SaveChangesAsync(cancellationToken);

            _cache.Add(command.UserId, new Events.Course_Enrolled(command.Id));
            return Created();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private Enrollment Adapt(CreateEnrollmentCommand command)
    {
        if (command.Rq.BillId is not null)
            return new Enrollment(command.Id, command.Rq.CourseId, command.UserId, (Guid)command.Rq.BillId);
        return new Enrollment(command.Id, command.Rq.CourseId, command.UserId);
    }
}
