using Contract.Domain.ProgressAggregates;
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

        if (command.Rq.AddedQuestions is not null)
        {
            List<McqQuestion> questions = [];
            foreach (var question in command.Rq.AddedQuestions)
            {
                var id = Guid.NewGuid();
                var answers = question.Answers.Select(_ => new McqAnswer(Guid.NewGuid(), _.Content)).ToList();
                questions.Add(new McqQuestion(id, question.Content, question.Explanation, command.Rq.Id, answers));
            }
            entity.Questions.AddRange(questions);
        }
        if (command.Rq.RemovedQuestions is not null)
        {
            var questions = await _context.McqQuestions
                .Where(_ => command.Rq.RemovedQuestions.Contains(_.Id) && _.SurveyId == command.Rq.Id)
                .ToListAsync();
            foreach (var question in questions)
                entity.Questions.Remove(question);
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