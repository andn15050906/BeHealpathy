using Contract.Domain.ProgressAggregate;
using Contract.Helpers;
using Contract.Requests.Progress.SurveyRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Library.SurveyHandlers;

public sealed class CreateSurveyHandler : RequestHandler<CreateSurveyCommand, HealpathyContext>
{
    public CreateSurveyHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result> Handle(CreateSurveyCommand command, CancellationToken cancellationToken)
    {
        Survey entity = Adapt(command);

        try
        {
            await _context.Surveys.InsertExt(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Created();
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private static Survey Adapt(CreateSurveyCommand command)
    {
        List<McqQuestion> questions = [];
        foreach (var question in command.Rq.Questions)
        {
            var id = Guid.NewGuid();
            var answers = question.Answers.Select(_ => new McqAnswer(Guid.NewGuid(), _.Content, _.Score ?? 0)).ToList();
            questions.Add(new McqQuestion(id, question.Content, question.Explanation, questions.Count, command.Id, answers));
        }

        var bands = (command.Rq.Bands ?? []).Select(_ => new SurveyScoreBand(_.MinScore, _.MaxScore, _.BandName, _.BandRating)).ToList();

        return new Survey(command.Id, command.Rq.Name, command.Rq.Description, command.Rq.IsScientific, questions, bands);
    }
}