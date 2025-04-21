using Contract.Domain.ProgressAggregates;
using Contract.Helpers;
using Contract.Requests.Statistics;
using Contract.Responses.Progress;
using Contract.Responses.Statistics;
using Core.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using static Contract.Responses.Statistics.Output;
using static Gateway.Controllers.Analysis.StatisticsController;

namespace Gateway.Services.Background;

public sealed class JobRunner
{
    public class CalculateRoadmapProgress
    {
        private readonly HealpathyContext _context;
        private readonly IAppLogger _appLogger;

        public CalculateRoadmapProgress(HealpathyContext context, IAppLogger logger)
        {
            _context = context;
            _appLogger = logger;
        }

        public async Task Execute()
        {
            try
            {
                var allUserIds = await _context.Users.Where(_ => !_.IsDeleted).Select(_ => _.Id).ToListAsync();

                foreach (var userId in allUserIds)
                {
                    var currentRoadmapId = await _context.Users.Where(_ => _.Id == userId).Select(_ => _.RoadmapId).FirstOrDefaultAsync();
                    if (currentRoadmapId == null)
                        continue;
                    var currentRoadmap = await _context.Roadmaps.Include(_ => _.Phases).ThenInclude(_ => _.Milestones).Where(_ => _.Id == currentRoadmapId).Select(RoadmapModel.MapExpression).FirstOrDefaultAsync();
                    if (currentRoadmap == null)
                        continue;

                    foreach (var phase in currentRoadmap.Phases.OrderBy(_ => _.Index))
                    {
                        var currentProgress = await _context.RoadmapProgress.Where(_ => _.CreatorId == userId && _.RoadmapPhaseId == phase.Id).OrderByDescending(_ => _.CreationTime).FirstOrDefaultAsync();

                        var currentMilestoneIdList = JsonSerializer.Deserialize<List<Guid>>(currentProgress?.MilestonesCompleted ?? "[]") ?? [];
                        var pendingMilestoneIdList = phase.Milestones.Select(_ => _.Id).Except(currentMilestoneIdList);
                        if (pendingMilestoneIdList.Any())
                        {
                            var addedMilestones = new List<Guid>();
                            var logs = await _context.ActivityLogs.Where(_ => _.CreatorId == userId && _.CreationTime.AddDays(phase.TimeSpan) > TimeHelper.Now).ToListAsync();
                            foreach (var pendingMilestoneId in pendingMilestoneIdList)
                            {
                                var pendingMilestone = phase.Milestones.First(_ => _.Id == pendingMilestoneId);
                                var eventCount = logs.Count(_ =>
                                    _.Content.Contains(pendingMilestone.EventName) ||
                                    (_.Content.Contains(nameof(Events.DiaryNote_Updated)) && pendingMilestone.EventName == nameof(Events.DiaryNote_Created)) ||
                                    (_.Content.Contains("question") && pendingMilestone.EventName == "QuestionOfTheDay_Answered") ||
                                    //..
                                    (_.Content.Contains("yoga") && pendingMilestone.EventName == "Yoga_Practiced") ||
                                    (_.Content.Contains("media") && pendingMilestone.EventName == "Media_Viewed")
                                );
                                if (eventCount >= pendingMilestone.RepeatTimesRequired)
                                    addedMilestones.Add(pendingMilestoneId);
                            }
                            if (addedMilestones.Count > 0)
                            {
                                currentMilestoneIdList.AddRange(addedMilestones);
                                var completedMilestones = JsonSerializer.Serialize(currentMilestoneIdList);

                                if (currentProgress is null)
                                {
                                    _context.RoadmapProgress.Add(new RoadmapProgress(userId, phase.Id, completedMilestones));
                                }
                                else
                                {
                                    currentProgress.MilestonesCompleted = completedMilestones;
                                }
                            }
                            break;
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _appLogger.Warn(ex.Message);
            }
        }
    }

    public class CalculateSentiment
    {
        private readonly HealpathyContext _context;
        private readonly ICalculationApiService _calculationApiService;

        public CalculateSentiment(HealpathyContext context, ICalculationApiService calculationApiService)
        {
            _context = context;
            _calculationApiService = calculationApiService;
        }

        public async Task Execute()
        {
            var recentlyActiveUsers = await _context.ActivityLogs
                .Where(_ => _.CreationTime > TimeHelper.Now.AddDays(-7))
                .Take(6)
                .Select(_ => _.CreatorId)
                .Distinct()
                .ToListAsync();
            DateTime endTime = TimeHelper.Now;
            DateTime startTime = endTime.AddDays(-21);

            foreach (var user in recentlyActiveUsers)
            {
                var analysis = await AnalyzeSentiment(user, startTime, endTime, _context, _calculationApiService);
                if (analysis != null)
                {
                    _context.UserStatistics.Add(
                        new UserStatistics(Guid.NewGuid(), user, TimeHelper.Now, JsonSerializer.Serialize(analysis))
                    );
                    await _context.SaveChangesAsync();
                }
            }
        }
    }

    public static async Task<Dictionary<DateTime, Analysis>> AnalyzeSentiment(
        Guid userId, DateTime startTime, DateTime endTime,
        HealpathyContext context, ICalculationApiService calculationApiService
    )
    {
        try
        {
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
            messageInputs = [.. chatInputs, .. diaryNoteInputs];

            // Input.Reactions //"JOIN with Articles"
            /*var articleReactions = await context.ArticleReactions
                .Where(_ => !_.IsDeleted && _.CreatorId == userId && _.CreationTime >= startTime && _.CreationTime <= endTime)
                .Select(_ => new Input.Reaction(_.CreationTime, string.Empty, _.Content))
                .ToListAsync();
            var lectureReactions = await context.LectureReactions //"JOIN with Lectures"
                .Where(_ => !_.IsDeleted && _.CreatorId == userId && _.CreationTime >= startTime && _.CreationTime <= endTime)
                .Select(_ => new Input.Reaction(_.CreationTime, string.Empty, _.Content))
                .ToListAsync();
            var messageReactions = await context.MessageReactions  //"JOIN with Messages"
                .Where(_ => !_.IsDeleted && _.CreatorId == userId && _.CreationTime >= startTime && _.CreationTime <= endTime)
                .Select(_ => new Input.Reaction(_.CreationTime, string.Empty, _.Content))
                .ToListAsync();*/

            // Input.Courses
            /*var courses = await context.Courses
                .Include(_ => _.Enrollments).Include(_ => _.LeafCategory)
                .AsSplitQuery()
                .Where(_ => !_.IsDeleted && _.Enrollments.Any(e => e.CreatorId == userId))
                .Select(_ => new Input.Course(_.CreationTime, _.LeafCategory.Title, _.Title, _.Description))
                .ToListAsync();*/

            // ArticleInputs

            // MediaInputs
            /*List<Guid> mediaIds = [];
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
            }*/

            // RoutineInputs
            /*var routineInputs = await context.Routines
                .Where(_ => _.CreatorId == userId && _.CreationTime >= startTime && _.CreationTime <= endTime)
                .Select(_ => new Input.Routine(_.CreationTime, _.Title, _.Description, _.Objective, string.Empty, new List<Input.RoutineLog>()))
                .ToListAsync();*/



            var groupedInput = new Input.DataCollection
            {
                UserId = userId,
                MessageInputs = messageInputs,
                //ReactionInputs = [.. articleReactions, .. lectureReactions, .. messageReactions],
                //CourseInputs = courses,
                //SubmissionInputs = await GetUserSubmissions(context, userId, startTime, endTime),
                //MediaInputs = mediaInputs,
                //RoutineInputs = routineInputs,
                //PreferenceInputs = await GetUserPreferences(context, userId)
            };

            Dictionary<DateTime, Input.GroupedData> inputs = [];
            var start = startTime.Date;
            var end = endTime.Date;
            for (var i = start; i <= end; i = i.AddDays(1))
                inputs.TryAdd(i, new Input.GroupedData());

            foreach (var messageInput in groupedInput.MessageInputs)
                inputs[messageInput.CreatedAt.Date].Messages.Add(messageInput.Content);
            /*foreach (var submissionInput in groupedInput.SubmissionInputs)
                foreach (var band in submissionInput.SurveyBandInputs)
                    if (!inputs[submissionInput.StartedAt.Date].Metrics_Rating.TryGetValue(band.Metrics, out _))
                        inputs[submissionInput.StartedAt.Date].Metrics_Rating.Add(band.Metrics, band.Rating);
            foreach (var mediaInput in groupedInput.MediaInputs)
                inputs[mediaInput.StartedAt.Date].Messages.Add(mediaInput.Title);
            foreach (var routineInput in groupedInput.RoutineInputs)
                inputs[routineInput.StartedAt.Date].Messages.Add(routineInput.Title);*/



            Dictionary<DateTime, Analysis> outputs = [];
            var analysis = new Analysis();
            List<InputByDay> dayInputs = [];
            foreach (var input in inputs)
            {
                if (input.Value.Messages is not null && input.Value.Messages.Count > 0)
                    dayInputs.Add(new InputByDay(input.Key, input.Value.Messages));
            }

            if (dayInputs.Count > 0)
            {
                try
                {
                    var predictions = await calculationApiService.PredictSentiment(new GetSentimentPredictionQuery(dayInputs));
                    if (predictions.IsSuccessful)
                    {
                        foreach (var date in inputs.Keys)
                        {
                            var inDateData = predictions.Data!.Where(_ => _.date == date).ToList() ?? [];
                            var inDateAnalysis = new Analysis();
                            if (inDateData.Count > 0)
                            {
                                var prediction =
                                    (inDateData.Where(_ => _.analysis.Prediction) ?? []).Sum(_ => _.analysis.Probability) >
                                    (inDateData.Where(_ => !_.analysis.Prediction) ?? []).Sum(_ => _.analysis.Probability);
                                var prob = inDateData.Average(_ => _.analysis.Probability);
                                var score = inDateData.Average(_ => _.analysis.Score);

                                var emotions = inDateData.SelectMany(_ => _.analysis.Emotions).ToList();
                                var keywords = inDateData.SelectMany(_ => _.analysis.Keywords).ToList();
                                var topics = inDateData.SelectMany(_ => _.analysis.Topics).ToList();

                                inDateAnalysis = new Analysis
                                {
                                    Prediction = prediction,
                                    Probability = prob,
                                    Score = score,
                                    Emotions = emotions,
                                    Keywords = keywords,
                                    Topics = topics
                                };
                            }
                            else
                            {
                                if (outputs.Count > 0)
                                    inDateAnalysis = outputs.ToList().Last().Value;
                            }

                            if (!outputs.TryGetValue(date, out _))
                            {
                                outputs.Add(date, inDateAnalysis);
                            }
                        }
                    }
                }
                catch (Exception ex) { }
            }
            return outputs;
        }
        catch (Exception)
        {
            return [];
        }
    }
}