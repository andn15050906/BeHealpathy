using Contract.Domain.ProgressAggregates;
using Contract.Helpers;
using Contract.Requests.Progress.SubmissionRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Library.SubmissionHandlers;

public sealed class CreateSubmissionHandler(HealpathyContext context, IAppLogger logger, IEventCache cache)
    : RequestHandler<CreateSubmissionCommand, Guid, HealpathyContext>(context, logger, cache)
{
    public override async Task<Result<Guid>> Handle(CreateSubmissionCommand command, CancellationToken cancellationToken)
    {
        Submission entity = Adapt(command);

        try
        {
            await _context.Submissions.InsertExt(entity);
            await _context.SaveChangesAsync(cancellationToken);

            _cache.Add(command.UserId, new Events.Submission_Created(entity.Id));
            return Created(entity.Id);
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private Submission Adapt(CreateSubmissionCommand command)
    {
        var choices = command.Rq.Choices.Select(_ => new McqChoice(command.Id, _.McqQuestionId, _.McqAnswerId)).ToList();

        return new Submission(command.Id, command.UserId, command.Rq.SurveyId, choices);
    }
}