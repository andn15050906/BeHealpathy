using Contract.BusinessRules.PreferenceBiz;
using Contract.Domain.CourseAggregate;
using Contract.Domain.LibraryAggregate;
using Contract.Domain.ProgressAggregates;
using Contract.Messaging.ApiClients.Http;
using Core.Helpers;
using Gateway.Services.AI.Translator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Controllers.Progress;

public class StatisticsController : ContractController
{
    public StatisticsController(IMediator mediator) : base(mediator)
    {
    }






    // Message, Diary, Review
    public class MessageInput(DateTime createdAt, string content)
    {
        public DateTime CreatedAt { get; set; } = createdAt;
        public string Content { get; set; } = content;
    }

    public class ReactionInput(DateTime createdAt, string sourceContent, string reaction)
    {
        public DateTime CreatedAt { get; set; } = createdAt;
        public string SourceContent { get; set; } = sourceContent;
        public string Reaction { get; set; } = reaction;
    }

    public class CourseInput(DateTime startedAt, string category, string title, string description)
    {
        public DateTime StartedAt { get; set; } = startedAt;
        public string Category { get; set; } = category;
        public string Title { get; set; } = title;
        public string Description { get; set; } = description;
    }

    public class ArticleInput(DateTime startedAt, string category, string title, string description)
    {
        public DateTime StartedAt { get; set; } = startedAt;
        public string Category { get; set; } = category;
        public string Title { get; set; } = title;
        public string Description { get; set; } = description;
    }

    public class MediaInput(DateTime startedAt, string description, string title, string type)
    {
        public DateTime StartedAt { get; set; } = startedAt;
        public string Description { get; set; } = description;
        public string Title { get; set; } = title;
        //MediaResourceType
        public string Type { get; set; } = type;
    }

    public class RoutineLogInput(DateTime startedAt, string content)
    {
        public DateTime StartedAt { get; set; } = startedAt;
        public string Content { get; set; } = content;
    }

    public class RoutineInput(string title, string description, string objective, string frequency, List<StatisticsController.RoutineLogInput> logs)
    {
        public string Title { get; set; } = title;
        public string Description { get; set; } = description;
        public string Objective { get; set; } = objective;
        //Frequency
        public string Frequency { get; set; } = frequency;
        public List<RoutineLogInput> Logs { get; set; } = logs;
    }

    public class SurveyAnswerInput(string question, string answer)
    {
        public string Question { get; set; } = question;
        public string Answer { get; set; } = answer;
    }

    public class SurveyBandInput(string metrics, string score, string rating)
    {
        public string Metrics { get; set; } = metrics;
        public string Score { get; set; } = score;
        public string Rating { get; set; } = rating;
    }

    public class SubmissionInput(DateTime startedAt, string surveyName, List<SurveyAnswerInput> answers, List<SurveyBandInput> metrics)
    {
        public DateTime StartedAt { get; set; } = startedAt;
        public string SurveyName { get; set; } = surveyName;
        public List<SurveyAnswerInput> SurveyAnswerInputs { get; set; } = answers;
        public List<SurveyBandInput> SurveyBandInputs { get; set; } = metrics;
    }

    public class PreferenceInput(string setting, string choice)
    {
        public string Setting { get; set; } = setting;
        public string Choice { get; set; } = choice;
    }






    public class AnalysisInput
    {
        public Guid UserId { get; set; }
        public List<MessageInput> ChatInputs { get; set; } = [];
        public List<ReactionInput> ReactionInputs { get; set; } = [];
        public List<CourseInput> CourseInputs { get; set; } = [];
        public List<ArticleInput> ArticleInputs { get; set; } = [];
        public List<MediaInput> MediaInputs { get; set; } = [];
        public List<RoutineInput> RoutineInputs { get; set; } = [];
        public List<SubmissionInput> SubmissionInputs { get; set; } = [];
        public List<PreferenceInput> PreferenceInputs { get; set; } = [];
    }

    public class AnalysisOutput
    {
        public string Sentiment { get; set; } = string.Empty;
        public List<string> Emotions { get; set; } = [];
        public List<string> Keywords { get; set; } = [];
        public List<string> Topics { get; set; } = [];
    }






    public class QueryStatisticsDto
    {
        public Guid? UserId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }

    [HttpGet("data")]
    [Authorize]
    public async Task<IActionResult> GetStatisticsData([FromQuery] QueryStatisticsDto dto, [FromServices] HealpathyContext context)
    {
        var userId = dto.UserId ?? ClientId;
        DateTime startTime = TimeHelper.Now;
        DateTime endTime = startTime.AddDays(1);
        if (dto.StartTime is not null && dto.EndTime is not null)
        {
            startTime = (DateTime)dto.StartTime;
            endTime = (DateTime)dto.EndTime;
        }

        // ChatInputs
        var messages = await context.ChatMessages
            .Where(_ => !_.IsDeleted && _.CreatorId == userId && _.CreationTime >= startTime && _.CreationTime <= endTime)
            .Select(_ => new MessageInput(_.CreationTime, _.Content))
            .ToListAsync();

        // ReactionInputs
        var articleReactions = await context.ArticleReactions
            .Where(_ => !_.IsDeleted && _.CreatorId == userId && _.CreationTime >= startTime && _.CreationTime <= endTime)
            .Select(_ => new ReactionInput(_.CreationTime, string.Empty /*"JOIN with Articles"*/, _.Content))
            .ToListAsync();
        var lectureReactions = await context.LectureReactions
            .Where(_ => !_.IsDeleted && _.CreatorId == userId && _.CreationTime >= startTime && _.CreationTime <= endTime)
            .Select(_ => new ReactionInput(_.CreationTime, string.Empty /*"JOIN with Lectures"*/, _.Content))
            .ToListAsync();
        var messageReactions = await context.MessageReactions
            .Where(_ => !_.IsDeleted && _.CreatorId == userId && _.CreationTime >= startTime && _.CreationTime <= endTime)
            .Select(_ => new ReactionInput(_.CreationTime, string.Empty /*"JOIN with Messages"*/, _.Content))
            .ToListAsync();

        // CourseInputs
        var courses = await context.Courses
            .Include(_ => _.Enrollments).Include(_ => _.LeafCategory)
            .AsSplitQuery()
            .Where(_ => !_.IsDeleted && _.Enrollments.Any(e => e.CreatorId == userId))
            .Select(_ => new CourseInput(_.CreationTime, _.LeafCategory.Title, _.Title, _.Description))
            .ToListAsync();

        // ArticleInputs

        // MediaInputs

        // RoutineInputs

        // SubmissionInputs
        var surveys = await context.Surveys
            .Include(_ => _.Questions).ThenInclude(_ => _.Answers)
            .Include(_ => _.Bands)
            .AsSplitQuery()
            .Where(_ => !_.IsDeleted)
            .ToListAsync();
        var submissions = await context.Submissions
            .Include(_ => _.Choices)
            .Where(_ => !_.IsDeleted && _.CreatorId == userId)
            .ToListAsync();
        List<SubmissionInput> submissionResults = [];
        foreach (var submission in submissions)
        {
            var answerArr = new List<Guid>();
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
                answerArr.Add(choice.McqAnswerId);
                var answer = question.Answers.FirstOrDefault(a => a.Id == choice.McqAnswerId);
                if (answer is not null)
                    score += answer.Score;
            }

            var matchingBands = new List<SurveyBandInput>();
            foreach (var band in survey.Bands)
                if (band.MinScore <= score && band.MaxScore >= score)
                    matchingBands.Add(new SurveyBandInput(band.BandName, $"{score}/{band.MaxScore}", band.BandRating));

            submissionResults.Add(new SubmissionInput(submission.CreationTime, survey.Name, [], matchingBands));
        }

        // PreferenceInputs
        var rawPreferences = context.Preferences
            .Where(_ => !_.IsDeleted && _.CreatorId == userId)
            .Select(_ => new PreferenceInput(_.SourceId.ToString(), _.Value));
        List<PreferenceInput> preferences = [];
        foreach (var rawPreference in rawPreferences)
        {
            var pref = PrefStore.Surveys.FirstOrDefault(_ => _.Id.ToString() == rawPreference.Setting);
            if (pref is null)
                continue;
            preferences.Add(new PreferenceInput(pref.Title, rawPreference.Choice));
        }

        var analysisInput = new AnalysisInput
        {
            UserId = userId,
            ChatInputs = messages,
            ReactionInputs = [.. articleReactions, .. lectureReactions, .. messageReactions],
            CourseInputs = courses,
            //ArticleInputs = 
            //MediaInputs =
            //RoutineInputs =
            SubmissionInputs = submissionResults,
            PreferenceInputs = preferences
        };

        return Ok(analysisInput);
    }


    [HttpGet("result")]
    [Authorize]
    public async Task<IActionResult> GetStatisticsResult([FromServices] ITranslateClient translateClient)
    {
        AnalysisInput input = new()
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
        AnalysisOutput output1 = new()
        {
            Sentiment = "pessimistic",
            Emotions = ["anxiety"],
            Keywords = ["căng thẳng", "công việc"], 
            Topics = ["problem_solving"]
        };

        AnalysisOutput output2 = new()
        {
            Sentiment = "optimistic",
            Emotions = ["relief"],
            Keywords = ["giải quyết", "tốt hơn"],
            Topics = ["problem_solving"]
        };

        return Ok(output1);
    }
}