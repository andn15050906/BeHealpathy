using Contract.BusinessRules.PreferenceBiz;
using Contract.Domain.ProgressAggregates;
using Contract.Domain.UserAggregate.Constants;
using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Statistics;
using Contract.Responses.Identity;
using Contract.Responses.Statistics;
using Core.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text.Json;

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
    //[ResponseCache(Duration = 180)]
    public async Task<IActionResult> GetSentimentAnalysis(
        [FromQuery] QueryStatisticsDto dto, [FromServices] HealpathyContext context, [FromServices] ICalculationApiService calculationApiService
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
        var chatInputs = await context.ChatMessages
            .Where(_ => !_.IsDeleted && _.CreatorId == userId && _.CreationTime >= startTime && _.CreationTime <= endTime)
            .Select(_ => new Input.Message(_.CreationTime, _.Content))
            .ToListAsync();
        var diaryNoteInputs = await context.DiaryNotes
            .Where(_ => !_.IsDeleted && _.CreatorId == userId && _.CreationTime >= startTime && _.CreationTime <= endTime)
            .Select(_ => new Input.Message(_.CreationTime, _.Content))
            .ToListAsync();
        var messageInputs = new List<Input.Message>();
        messageInputs = [..chatInputs, ..diaryNoteInputs];

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
        List<Guid> mediaIds = [];
        List<Input.Media> mediaInputs = [];
        var mediaViewedLogs = await context.ActivityLogs
            .Where(_ => !_.IsDeleted && _.CreatorId == userId && _.Content.Contains("Media_Viewed") && _.CreationTime >= startTime && _.CreationTime <= endTime)
            .ToListAsync();
        foreach (var mediaViewLog in mediaViewedLogs)
        {
            try
            {
                var log = JsonSerializer.Deserialize<MediaViewedLog>(mediaViewLog.Content);
                var logContent = JsonSerializer.Deserialize<MediaViewedLog.MediaViewedLogContent>(log?.Content ?? string.Empty);
                var logInnerContent = JsonSerializer.Deserialize<MediaViewedLog.MediaViewedLogContentContent>(logContent?.Content ?? string.Empty, JsonSerializerOptions.Web);
                mediaIds.Add(logInnerContent?.Id ?? Guid.Empty);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                mediaIds.Add(Guid.Empty);
            }
        }
        var medias = await context.MediaResources
            .Where(_ => mediaIds.Contains(_.Id))
            .ToListAsync();
        for (int i = 0; i < mediaViewedLogs.Count; i++)
        {
            var media = medias.FirstOrDefault(_ => _.Id == mediaViewedLogs[i].Id);
            if (media is null || string.IsNullOrWhiteSpace(media.Description))
                continue;
            mediaInputs.Add(new Input.Media(
                mediaViewedLogs[i].CreationTime,
                media.Description,
                media.Title,
                media.Type.ToString()
            ));
        }

        // RoutineInputs
        var routineInputs = await context.Routines
            .Where(_ => _.CreatorId == userId && _.CreationTime >= startTime && _.CreationTime <= endTime)
            .Select(_ => new Input.Routine(_.CreationTime, _.Title, _.Description, _.Objective, string.Empty, new List<Input.RoutineLog>()))
            .ToListAsync();



        var groupedInput = new Input.DataCollection
        {
            UserId = userId,
            MessageInputs = messageInputs,
            ReactionInputs = [.. articleReactions, .. lectureReactions, .. messageReactions],
            CourseInputs = courses,
            SubmissionInputs = await GetUserSubmissions(context, userId, startTime, endTime),
            MediaInputs = mediaInputs,
            RoutineInputs = routineInputs,
            PreferenceInputs = await GetUserPreferences(context, userId)
        };

        Dictionary<DateTime, Input.GroupedData> inputs = [];
        var start = startTime.Date;
        var end = endTime.Date;
        for (var i = start; i <= end; i = i.AddDays(1))
            inputs.TryAdd(i, new Input.GroupedData());

        foreach (var messageInput in groupedInput.MessageInputs)
            inputs[messageInput.CreatedAt.Date].Messages.Add(messageInput.Content);
        //...
        //foreach (var reactionInput in groupedInput.ReactionInputs)
        //foreach (var courseInput in groupedInput.CourseInputs)
        foreach (var submissionInput in groupedInput.SubmissionInputs)
            foreach (var band in submissionInput.SurveyBandInputs)
                inputs[submissionInput.StartedAt.Date].Metrics_Rating.Add(band.Metrics, band.Rating);
        //...
        foreach (var mediaInput in groupedInput.MediaInputs)
            inputs[mediaInput.StartedAt.Date].Messages.Add(mediaInput.Title);
        foreach (var routineInput in groupedInput.RoutineInputs)
            inputs[routineInput.StartedAt.Date].Messages.Add(routineInput.Title);



        Dictionary<DateTime, Output.Analysis> outputs = [];
        var tasks = new List<Task>();
        foreach (var input in inputs)
        {
            tasks.Add(Task.Run(async () =>
            {
                var analysis = new Output.Analysis();
                //var message = await GeminiClient.Instance.Prompt(dto.Content);

                if (input.Value.Messages is not null && input.Value.Messages.Count > 0)
                {
                    var messageInput = string.Join(" ", input.Value.Messages);
                    var prediction = await calculationApiService.PredictSentiment(new GetSentimentPredictionQuery(messageInput));
                    if (prediction.IsSuccessful)
                        analysis = prediction.Data!;
                }
                else if (outputs.Count > 0)
                {
                    lock (outputs)
                    {
                        analysis = outputs.ToList().Last().Value;
                    }
                }

                lock (outputs)
                {
                    outputs.Add(input.Key, analysis);
                }
            }));
        }
        await Task.WhenAll(tasks);
        return Ok(outputs);
    }

    [HttpGet("report/general")]
    [Authorize(Roles = RoleConstants.ADMIN)]
    public async Task<IActionResult> GetGeneralReport([FromServices] HealpathyContext context)
    {
        var totalUserCount = await context.Users
            .CountAsync();
        var totalUserCountOverMonths = await context.Users
            .GroupBy(_ => new { _.CreationTime.Year, _.CreationTime.Month })
            .Select(_ => new
            {
                Year = _.Key.Year,
                Month = _.Key.Month,
                Count = _.Count()
            })
            .OrderBy(_ => _.Year).ThenBy(_ => _.Month)
            .ToListAsync();
        var recentlyActiveUserCount = await context.ActivityLogs
            .Where(_ => _.CreationTime > TimeHelper.Now.AddDays(-7))
            .Select(_ => _.CreatorId)
            .Distinct()
            .CountAsync();



        var totalPremiumUserCount = await context.Users
            .CountAsync(_ => _.IsPremium);
        var totalPremiumUserCountOverMonths = await context.Users
            .Where(_ => _.IsPremium)
            .GroupBy(_ => new { _.CreationTime.Year, _.CreationTime.Month })
            .Select(_ => new
            {
                Year = _.Key.Year,
                Month = _.Key.Month,
                Count = _.Count()
            })
            .OrderBy(_ => _.Year).ThenBy(_ => _.Month)
            .ToListAsync();



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



        /*var eventList = new string[]
        {
            //"QuestionOfTheDay_Answered",
            //"Mood_Updated",
            "Yoga_Practiced",
            //"Course_Completed",
            "Media_Viewed",
            //General_Activity_Created

            //"BillCreated",
            "Submission_Created",
            "Routine_Created",
            //"Routine_Updated",
            //"RoutineLog_Created",
            //"RoutineLog_Updated",
            "DiaryNote_Created",
            //"DiaryNote_Updated",

            "Article_Created",
            "ArticleComment_Created",
            //"ArticleReaction_Created",
            //"MediaResource_Created",

            "Conversation_Created",
            "Conversation_Joined",
            //"Conversation_Left",
            "ChatMessage_Created",
            //"MessageReaction_Created",
            "Meeting_Created",
            "Meeting_Joined",

            //"Advisor_Created",
            //"Course_Created",
            "Course_Enrolled",
            //"Course_Unenrolled",
            "CourseReview_Created",
            "LectureComment_Created",

            "Article_Read",
        };
        Dictionary<string, int> activityLogCounts = [];
        foreach (var eventName in eventList)
        {
            activityLogCounts.Add(
                eventName,
                await context.ActivityLogs.CountAsync(_ => _.CreationTime > TimeHelper.Now.AddDays(-7) && _.Content.Contains(eventName))
            );
        }*/



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