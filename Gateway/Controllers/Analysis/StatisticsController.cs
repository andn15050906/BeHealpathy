using Contract.BusinessRules.PreferenceBiz;
using Contract.Domain.ProgressAggregates;
using Contract.Domain.UserAggregate.Constants;
using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Statistics;
using Contract.Responses.Identity;
using Contract.Responses.Statistics;
using Core.Helpers;
using Gateway.Services.AI.ChatBot;
using Gateway.Services.AI.Translator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Gateway.Controllers.Analysis;

public sealed class StatisticsController : ContractController
{
    public StatisticsController(IMediator mediator) : base(mediator)
    {
    }

    public class QueryStatisticsDto
    {
        public Guid? UserId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }

    [HttpGet("progress")]
    [Authorize]
    public async Task<IActionResult> GetPersonalProgressStatistics([FromServices] HealpathyContext context)
    {
        var currentRoadmap = await context.Users.Where(_ => _.Id == ClientId).Select(_ => _.RoadmapId).FirstOrDefaultAsync();
        if (currentRoadmap is null)
            return NotFound();

        var progress = await context.RoadmapProgress.Include(_ => _.RoadmapPhase).Where(_ => _.CreatorId == ClientId).ToListAsync();
        return Ok(progress);
    }

    [HttpGet("mood")]
    [Authorize]
    public async Task<IActionResult> GetPersonalMoodStatistics([FromServices] HealpathyContext context)
    {
        var moods = await context.ActivityLogs.Where(_ => _.Id == ClientId && _.Content.Contains("Mood_Updated")).Select(ActivityLogModel.MapExpression).ToListAsync();
        return Ok(moods);
    }

    [HttpGet("sentiment")]
    [Authorize]
    [ResponseCache(Duration = 180)]
    public async Task<IActionResult> GetSentimentAnalysis(
        [FromQuery] QueryStatisticsDto dto, [FromServices] HealpathyContext context,
        [FromServices] IChatbotClient chatbotClient, [FromServices] ICalculationApiService calculationApiService
        )
    {
        var userId = ClientId;
        if (dto.UserId is not null && User.IsInRole(RoleConstants.ADMIN))
            userId = (Guid)dto.UserId;

        DateTime endTime = TimeHelper.Now;
        DateTime startTime = endTime.AddDays(-30);
        if (dto.StartTime is not null && dto.EndTime is not null)
        {
            startTime = (DateTime)dto.StartTime;
            endTime = (DateTime)dto.EndTime;
        }

        // MessageInputs
        var chats = await context.ChatMessages
            .Where(_ => !_.IsDeleted && _.CreatorId == userId && _.CreationTime >= startTime && _.CreationTime <= endTime)
            .Select(_ => new Input.Message(_.CreationTime, _.Content))
            .ToListAsync();
        var diaryNotes = await context.DiaryNotes
            .Where(_ => !_.IsDeleted && _.CreatorId == userId && _.CreationTime >= startTime && _.CreationTime <= endTime)
            .Select(_ => new Input.Message(_.CreationTime, _.Content))
            .ToListAsync();
        var messages = new List<Input.Message>();
        messages = [..chats, ..diaryNotes];

        // Input.Reactions
        var articleReactions = await context.ArticleReactions
            .Where(_ => !_.IsDeleted && _.CreatorId == userId && _.CreationTime >= startTime && _.CreationTime <= endTime)
            .Select(_ => new Input.Reaction(_.CreationTime, string.Empty /*"JOIN with Articles"*/, _.Content))
            .ToListAsync();
        var lectureReactions = await context.LectureReactions
            .Where(_ => !_.IsDeleted && _.CreatorId == userId && _.CreationTime >= startTime && _.CreationTime <= endTime)
            .Select(_ => new Input.Reaction(_.CreationTime, string.Empty /*"JOIN with Lectures"*/, _.Content))
            .ToListAsync();
        var messageReactions = await context.MessageReactions
            .Where(_ => !_.IsDeleted && _.CreatorId == userId && _.CreationTime >= startTime && _.CreationTime <= endTime)
            .Select(_ => new Input.Reaction(_.CreationTime, string.Empty /*"JOIN with Messages"*/, _.Content))
            .ToListAsync();

        // Input.Courses
        var courses = await context.Courses
            .Include(_ => _.Enrollments).Include(_ => _.LeafCategory)
            .AsSplitQuery()
            .Where(_ => !_.IsDeleted && _.Enrollments.Any(e => e.CreatorId == userId))
            .Select(_ => new Input.Course(_.CreationTime, _.LeafCategory.Title, _.Title, _.Description))
            .ToListAsync();

        // ArticleInputs

        // MediaInputs

        // RoutineInputs



        var groupedInput = new Input.DataCollection
        {
            UserId = userId,
            MessageInputs = messages,
            ReactionInputs = [.. articleReactions, .. lectureReactions, .. messageReactions],
            CourseInputs = courses,
            SubmissionInputs = await GetUserSubmissions(context, userId, startTime, endTime),
            PreferenceInputs = await GetUserPreferences(context, userId)
        };

        Dictionary<DateTime, Input.GroupedData> inputs = [];
        var start = startTime.Date;
        var end = endTime.Date;
        for (var i = start; i < end; i = i.AddDays(1))
            inputs.TryAdd(i, new Input.GroupedData());

        foreach (var messageInput in groupedInput.MessageInputs)
            inputs[messageInput.CreatedAt.Date].Messages.Add(messageInput.Content);
        //...
        //foreach (var reactionInput in groupedInput.ReactionInputs)
        //foreach (var courseInput in groupedInput.CourseInputs)
        foreach (var submissionInput in groupedInput.SubmissionInputs)
            foreach (var band in submissionInput.SurveyBandInputs)
                inputs[submissionInput.StartedAt.Date].Metrics_Rating.Add(band.Metrics, band.Rating);



        Dictionary<DateTime, Output.Analysis> outputs = [];
        foreach (var input in inputs)
        {
            var analysis = new Output.Analysis();
            //var message = await GeminiClient.Instance.Prompt(dto.Content);

            if (input.Value.Messages is not null && input.Value.Messages.Count > 0)
            {
                var messageInput = string.Join("\n", input.Value.Messages);
                var query = new GetSentimentPredictionQuery(messageInput);
                var prediction = await calculationApiService.PredictSentiment(query);
                if (prediction.IsSuccessful)
                    analysis = prediction.Data!;
            }
            outputs.Add(input.Key, analysis);
        }
        return Ok(outputs);
    }

    /*[HttpGet("result")]
    [Authorize]
    public async Task<IActionResult> GetStatisticsResult([FromServices] ITranslateClient translateClient)
    {
        Input.Analysis input = new()
        {
            UserId = Guid.NewGuid(),
            ChatInputs =
            [
                new(new DateTime(2025, 2, 28, 6, 0, 0), "Hôm nay mình cảm thấy hơi căng thẳng vì công việc."),
                new(new DateTime(2025, 2, 28, 12, 30, 0), "Mình đã giải quyết xong vấn đề, cảm thấy tốt hơn rồi!"),
            ]
        };

        //... one by one
        foreach (var message in input.ChatInputs)
        {
            message.Content = await translateClient.Translate(message.Content, "vi", "en");
        }

        // 28 - 02
        // UserId
        Output.Analysis output1 = new()
        {
            Sentiment = "pessimistic",
            Emotions = ["anxiety"],
            Keywords = ["căng thẳng", "công việc"],
            Topics = ["problem_solving"]
        };

        Output.Analysis output2 = new()
        {
            Sentiment = "optimistic",
            Emotions = ["relief"],
            Keywords = ["giải quyết", "tốt hơn"],
            Topics = ["problem_solving"]
        };

        return Ok(output1);
    }*/






    public static async Task<List<Input.Submission>> GetUserSubmissions(
        HealpathyContext context, Guid userId, DateTime? startTime = null, DateTime? endTime = null, List<string>? latestSurveyNames = null)
    {
        Expression<Func<Survey, bool>> surveyPredicate = latestSurveyNames is not null
            ? _ => !_.IsDeleted && latestSurveyNames.Any(s => _.Name.Contains(s))
            : _ => !_.IsDeleted;

        var surveys = await context.Surveys
            .Include(_ => _.Questions).ThenInclude(_ => _.Answers)
            .Include(_ => _.Bands)
            .AsSplitQuery()
            .Where(surveyPredicate)
            .ToListAsync();

        List<Submission>? submissions = [];
        if (latestSurveyNames is null)
        {
            if (startTime is not null && endTime is not null)
            {
                submissions = await context.Submissions
                    .Include(_ => _.Choices)
                    .Where(_ => !_.IsDeleted && _.CreatorId == userId && _.CreationTime >= startTime && _.CreationTime <= endTime)
                    .ToListAsync();
            }
            else
            {
                submissions = await context.Submissions
                    .Include(_ => _.Choices)
                    .Where(_ => !_.IsDeleted && _.CreatorId == userId)
                    .ToListAsync();
            }
        }
        else
        {
            if (startTime is not null && endTime is not null)
            {
                submissions = await context.Submissions
                    .Include(_ => _.Choices).Include(_ => _.Survey)
                    .AsSplitQuery()
                    .Where(_ => !_.IsDeleted && _.CreatorId == userId && _.CreationTime >= startTime && _.CreationTime <= endTime)
                    .ToListAsync();
            }
            else
            {
                submissions = await context.Submissions
                    .Include(_ => _.Choices).Include(_ => _.Survey)
                    .AsSplitQuery()
                    .Where(_ => !_.IsDeleted && _.CreatorId == userId)
                    .ToListAsync();
            }

            var tempList = submissions;
            submissions = [];
            foreach (var submission in tempList)
            {
                var existingSurvey = submissions.FirstOrDefault(_ => _.Survey.Name == submission.Survey.Name);
                if (existingSurvey is null)
                    submissions.Add(submission);
                else if (existingSurvey.CreationTime < submission.CreationTime)
                {
                    submissions.Remove(existingSurvey);
                    submissions.Add(submission);
                }
            }
        }

        List<Input.Submission> result = [];
        foreach (var submission in submissions)
        {
            var answerArr = new List<Input.SurveyAnswer>();
            int score = 0;

            var survey = surveys.FirstOrDefault(_ => _.Id == submission.SurveyId);
            if (survey is null)
                continue;
            var questions = survey.Questions;
            var choices = submission.Choices;

            foreach (var question in questions)
            {
                var choice = choices.FirstOrDefault(_ => _.McqQuestionId == question.Id);
                if (choice is null)
                    continue;
                var answer = question.Answers.FirstOrDefault(a => a.Id == choice.McqAnswerId);
                if (answer is not null)
                {
                    if (survey.Bands.Count > 0)
                        score += answer.Score;
                    else
                        answerArr.Add(new Input.SurveyAnswer(question.Content, answer.Content));
                }
            }

            var matchingBands = new List<Input.SurveyBand>();
            foreach (var band in survey.Bands)
                if (band.MinScore <= score && band.MaxScore >= score)
                    matchingBands.Add(new Input.SurveyBand(band.BandName, $"{score}/{band.MaxScore}", band.BandRating));

            result.Add(new Input.Submission(submission.CreationTime, survey.Name, answerArr, score, matchingBands));
        }
        return result;
    }

    public static async Task<List<Input.Preference>> GetUserPreferences(HealpathyContext context, Guid userId)
    {
        List<Input.Preference> result = [];

        var rawPreferences = await context.Preferences
            .Where(_ => !_.IsDeleted && _.CreatorId == userId)
            .Select(_ => new Input.Preference(_.SourceId.ToString(), _.Value))
            .ToListAsync();

        foreach (var rawPreference in rawPreferences)
        {
            var pref = PrefStore.Surveys.FirstOrDefault(_ => _.Id.ToString() == rawPreference.Setting);
            if (pref is null)
                continue;
            result.Add(new Input.Preference(pref.Title, rawPreference.Choice));
        }

        return result;
    }

    /*private static Input.GroupedData? GetInputOfDay(Dictionary<DateTime, Input.GroupedData> inputs, DateTime date)
    {
        if (inputs.TryGetValue(date.Date, out Input.GroupedData? value))
            return value;

        value = new Input.GroupedData();
        inputs.Add(date.Date, value);
        return null;
    }*/
}