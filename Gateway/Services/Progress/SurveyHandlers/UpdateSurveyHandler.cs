using Contract.Domain.ProgressAggregate;
using Contract.Helpers;
using Contract.Requests.Progress.SurveyRequests;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Services.Library.SurveyHandlers;

public sealed class UpdateSurveyHandler : RequestHandler<UpdateSurveyCommand, HealpathyContext>
{
    public UpdateSurveyHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result> Handle(UpdateSurveyCommand command, CancellationToken cancellationToken)
    {
        var entity = await _context.Surveys.FirstOrDefaultAsync(_ => _.Id == command.Rq.Id);

        if (entity is null)
            return NotFound(string.Empty);

        if ((command.Rq.AddedQuestions ??= []).Count > 0)
        {
            List<McqQuestion> questions = [];
            foreach (var question in command.Rq.AddedQuestions)
            {
                var id = Guid.NewGuid();
                var answers = question.Answers.Select(_ => new McqAnswer(Guid.NewGuid(), _.Content, _.OptionValue, _.Score ?? 0)).ToList();
                // questions.Count => Add to last
                questions.Add(new McqQuestion(id, question.Content, question.Precondition, questions.Count, command.Rq.Id, answers));
            }
            entity.Questions.AddRange(questions);
        }
        if ((command.Rq.RemovedQuestions ??= []).Count > 0)
        {
            var questions = await _context.McqQuestions
                .Where(_ => command.Rq.RemovedQuestions.Contains(_.Id) && _.SurveyId == command.Rq.Id)
                .ToListAsync();
            foreach (var question in questions)
                entity.Questions.Remove(question);
        }
        if ((command.Rq.AddedScoreBands ??= []).Count > 0)
        {
            var bands = command.Rq.AddedScoreBands.Select(_ => new SurveyScoreBand(_.MinScore, _.MaxScore, _.BandName, _.BandRating)).ToList();
            entity.Bands.AddRange(bands);
        }
        if ((command.Rq.RemovedScoreBands ??= []).Count > 0)
        {
            var bands = await _context.SurveyScoreBands
                .Where(_ => command.Rq.RemovedScoreBands.Contains(_.Id))
                .ToListAsync();
            foreach (var band in bands)
                entity.Bands.Remove(band);
        }

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

    private Survey ApplyChanges(Survey entity, UpdateSurveyCommand command)
    {
        if (command.Rq.Name is not null)
            entity.Name = command.Rq.Name;
        if (command.Rq.Description is not null)
            entity.Description = command.Rq.Description;
        return entity;
    }
}