using Contract.Domain.ProgressAggregates;
using Contract.Helpers;
using Contract.Requests.Progress.McqRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Library.McqQuestionHandlers;

public sealed class CreateMcqQuestionHandler : RequestHandler<CreateMcqQuestionCommand, HealpathyContext>
{
    public CreateMcqQuestionHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result> Handle(CreateMcqQuestionCommand command, CancellationToken cancellationToken)
    {
        McqQuestion entity = Adapt(command);

        try
        {
            await _context.McqQuestions.InsertExt(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Created();
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private static McqQuestion Adapt(CreateMcqQuestionCommand command)
    {
        List<McqAnswer> answers = command.Rq.Answers.Select(_ => new McqAnswer(Guid.NewGuid(), _.Content, _.Score ?? 0)).ToList();

        // 0 as index
        return new McqQuestion(command.Id, command.Rq.Content, command.Rq.Explanation, 0, command.Rq.SurveyId, answers);
    }
}