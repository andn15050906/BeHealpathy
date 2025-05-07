using Contract.BusinessRules.PreferenceBiz;
using Contract.Domain.ProgressAggregates;
using Contract.Domain.UserAggregate.Constants;
using Contract.Helpers;
using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Statistics;
using Contract.Responses.Identity;
using Contract.Responses.Progress;
using Contract.Responses.Statistics;
using Core.Helpers;
using Gateway.Services.Background;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Gateway.Controllers.Analysis;

public sealed class StatisticsController : ContractController
{
    private readonly IAppLogger _logger;

    public StatisticsController(IMediator mediator, IAppLogger logger) : base(mediator)
    {
        _logger = logger;
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
        var currentRoadmap =
            ReadContext.Users.Where(_ => _.Id == ClientId).Select(_ => _.RoadmapId).FirstOrDefault()
            ?? await context.Users.Where(_ => _.Id == ClientId).Select(_ => _.RoadmapId).FirstOrDefaultAsync();
        if (currentRoadmap is null)
            return NotFound();

        var progress = await context.RoadmapProgress
            .Include(_ => _.RoadmapPhase)
            .Where(_ => _.CreatorId == ClientId)
            .ToListAsync();

        return Ok(new
        {
            progress = progress
        });
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
    public async Task<IActionResult> GetSentimentAnalysis(
        [FromQuery] QueryStatisticsDto dto, [FromServices] HealpathyContext context, [FromServices] ICalculationApiService calculationApiService
        )
    {
        var userId = ClientId;
        if (dto.UserId is not null && User.IsInRole(RoleConstants.ADMIN))
            userId = (Guid)dto.UserId;

        DateTime endTime = TimeHelper.Now;
        DateTime startTime = endTime.AddDays(-21);
        if (dto.StartTime is not null && dto.EndTime is not null)
        {
            startTime = (DateTime)dto.StartTime;
            endTime = (DateTime)dto.EndTime;
        }

        try
        {
            var resultInDb = await context.UserStatistics
                .OrderByDescending(_ => _.CreationTime)
                .Where(_ => _.CreationTime > TimeHelper.Now.AddDays(-14) && _.Content != "{}")
                .FirstOrDefaultAsync(_ => _.CreatorId == userId);
            if (resultInDb is not null)
                return Ok(JsonSerializer.Deserialize<Dictionary<DateTime, Output.Analysis>>(resultInDb.Content));
        }
        catch (Exception) { }

        var result = await JobRunner.AnalyzeSentiment(
            userId, startTime, endTime,
            context, calculationApiService
        );
        return Ok(result);
    }

    [HttpGet("report/general")]
    [Authorize(Roles = RoleConstants.ADMIN)]
    public async Task<IActionResult> GetGeneralReport([FromServices] HealpathyContext context)
    {
        var totalUserCount = ReadContext.Users.Count;

        var totalUserCountOverMonths = ReadContext.Users
            .GroupBy(_ => new { _.CreationTime.Year, _.CreationTime.Month })
            .Select(_ => new
            {
                Year = _.Key.Year,
                Month = _.Key.Month,
                Count = _.Count()
            })
            .OrderBy(_ => _.Year).ThenBy(_ => _.Month)
            .ToList();

        var recentlyActiveUserCount = ReadContext.ActivityLogs
            .Where(_ => _.CreationTime > TimeHelper.Now.AddDays(-7))
            .Select(_ => _.CreatorId)
            .Distinct()
            .Count();



        var totalPremiumUserCount = ReadContext.Users
            .Count(_ => _.IsPremium);
        var totalPremiumUserCountOverMonths = ReadContext.Users
            .Where(_ => _.IsPremium)
            .GroupBy(_ => new { _.CreationTime.Year, _.CreationTime.Month })
            .Select(_ => new
            {
                Year = _.Key.Year,
                Month = _.Key.Month,
                Count = _.Count()
            })
            .OrderBy(_ => _.Year).ThenBy(_ => _.Month)
            .ToList();



        var revenue = await context.Bills
            .SumAsync(_ => _.Amount);
        var revenueOverWeeks = await context.Bills
            .GroupBy(_ => new { _.CreationTime.Year, _.CreationTime.Month })
            .Select(_ => new
            {
                Year = _.Key.Year,
                Month = _.Key.Month,
                Sum = _.Sum(b => b.Amount)
            })
            .OrderBy(r => r.Year).ThenBy(r => r.Month)
            .ToListAsync();

        return Ok(new
        {
            totalUserCount,
            totalUserCountOverMonths,
            recentlyActiveUserCount,

            totalPremiumUserCount,
            totalPremiumUserCountOverMonths,

            revenue,
            revenueOverWeeks,

            //activityLogCounts
        });
    }

    [HttpGet("force-progress")]
    public async Task ForceCalculateProgress([FromServices] HealpathyContext context, [FromServices] IAppLogger logger)
    {
        await JobRunner.AnalyzeRoadmapProgress(context, ClientId);
    }





    public static async Task<List<Input.Submission>> GetUserSubmissions(
        HealpathyContext context, Guid userId, DateTime? startTime = null, DateTime? endTime = null, List<string>? latestSurveyNames = null)
    {
        List<SurveyModel> surveys = [];
        if (latestSurveyNames is not null)
        {
            surveys = ReadContext.Surveys.Where(_ => latestSurveyNames.Any(s => _.Name.Contains(s))).ToList();
        }
        else
        {
            surveys = ReadContext.Surveys;
        }

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

    // Content
    //   Content
    //     Event
    //     Id
    // GenericType

    public class MediaViewedLog
    {
        public class MediaViewedLogContent
        {
            public string Content { get; set; }
        }

        public class MediaViewedLogContentContent
        {
            public string Event { get; set; }
            public Guid Id { get; set; }
        }

        public string Content { get; set; }
        public string GenericType { get; set; }
    }
}