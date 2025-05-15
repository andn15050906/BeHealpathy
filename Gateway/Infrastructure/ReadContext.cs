using Contract.Responses.Identity;
using Contract.Responses.Notifications;
using Contract.Responses.Community;
using Microsoft.EntityFrameworkCore;
using Contract.Responses.Progress;

namespace Gateway.Infrastructure;

public class ReadContext
{
    private static HealpathyContext _db;

    public static void Init(HealpathyContext db)
    {
        _db = db;
    }

    public static async Task RefreshAllCaches()
    {
        await RefreshUsers();
        await RefreshNotifications();
        await RefreshActivityLogs();
        await RefreshUserStatistics();
        await RefreshChatMessages();
        await RefreshSurveys();
        await RefreshSubmissions();
        await RefreshRoadmaps();
    }

    #region Refresh methods
    public static async Task RefreshUsers()
    {
        Users = await _db.Users
            /*
            .Include(_ => _.Settings)
            .Include(_ => _.Preferences)
            */
            .AsSplitQuery()
            .Where(_ => !_.IsDeleted)
            .Select(UserFullModel.MapExpression)
            .ToListAsync();
    }

    public static async Task RefreshNotifications()
    {
        Notifications = await _db.Notifications
            .Where(_ => !_.IsDeleted)
            .Select(NotificationModel.MapExpression)
            .ToListAsync();
    }

    public static async Task RefreshActivityLogs()
    {
        ActivityLogs = await _db.ActivityLogs
            .Where(_ => !_.IsDeleted)
            .Select(ActivityLogModel.MapExpression)
            .ToListAsync();
    }

    public static async Task RefreshUserStatistics()
    {
        UserStatistics = await _db.UserStatistics
            .Where(_ => !_.IsDeleted)
            .ToListAsync();
    }

    public static async Task RefreshChatMessages()
    {
        ChatMessages = await _db.ChatMessages
            .Include(_ => _.Reactions)
            .Where(_ => !_.IsDeleted)
            .Select(ChatMessageModel.MapExpression)
            .ToListAsync();
    }

    public static async Task RefreshSurveys()
    {
        Surveys = await _db.Surveys
            .Include(_ => _.Questions).ThenInclude(_ => _.Answers)
            .Include(_ => _.Bands)
            .AsSplitQuery()
            .Where(_ => !_.IsDeleted)
            .Select(SurveyModel.MapExpression)
            .ToListAsync();
    }

    public static async Task RefreshSubmissions()
    {
        Submissions = await _db.Submissions
            .Include(_ => _.Choices)
            .Include(_ => _.Survey)
            .AsSplitQuery()
            .Where(_ => !_.IsDeleted)
            .Select(SubmissionModel.MapExpression)
            .ToListAsync();
    }

    public static async Task RefreshRoadmaps()
    {
        Roadmaps = await _db.Roadmaps
            .Include(_ => _.Phases).ThenInclude(_ => _.Milestones)//.ThenInclude(_ => _.Recommendations)
            .Where(_ => !_.IsDeleted)
            .Select(RoadmapModel.MapExpression)
            .ToListAsync();
    }
    #endregion Refresh methods











    public static List<UserFullModel> Users { get; set; }
    //public static List<SettingModel> Settings { get; set; }
    //public static List<PreferenceModel> Preferences { get; set; }
    public static List<NotificationModel> Notifications { get; set; }
    //public static List<BillModel> Bills { get; set; }
    public static List<ActivityLogModel> ActivityLogs { get; set; }
    public static List<UserStatistics> UserStatistics { get; set; }                 // Not model

    //public static List<ConversationModel> Conversations { get; set; }
    //public static List<ConversationMemberModel> ConversationMembers { get; set; }
    public static List<ChatMessageModel> ChatMessages { get; set; }
    //public static List<MessageReaction> MessageReactions { get; set; }
    //public static List<Meeting> Meetings { get; set; }
    //public static List<MeetingParticipant> MeetingParticipants { get; set; }

    public static List<SurveyModel> Surveys { get; set; }
    //public static List<SurveyScoreBand> SurveyScoreBands { get; set; }
    //public static List<McqQuestion> McqQuestions { get; set; }
    //public static List<McqAnswer> McqAnswers { get; set; }
    //public static List<McqChoice> McqChoices { get; set; }
    public static List<SubmissionModel> Submissions { get; set; }
    //public static List<Routine> Routines { get; set; }
    //public static List<RoutineLog> RoutineLogs { get; set; }
    //public static List<DiaryNote> DiaryNotes { get; set; }
    public static List<RoadmapModel> Roadmaps { get; set; }
    //public static List<RoadmapPhase> RoadmapPhases { get; set; }
    //public static List<RoadmapMilestone> RoadmapMilestones { get; set; }
    //public static List<RoadmapRecommendation> RoadmapRecommendations { get; set; }
    //public static List<RoadmapProgress> RoadmapProgress { get; set; }

    //public static List<Article> Articles { get; set; }
    //public static List<ArticleSection> ArticleSections { get; set; }
    //public static List<ArticleComment> ArticleComments { get; set; }
    //public static List<ArticleReaction> ArticleReactions { get; set; }
    //public static List<Tag> Tags { get; set; }
    //public static List<ArticleTag> ArticleTags { get; set; }
    //public static List<MediaResource> MediaResources { get; set; }

    //public static List<Advisor> Advisors { get; set; }
    //public static List<Category> Categories { get; set; }
    //public static List<Course> Courses { get; set; }
    //public static List<Enrollment> Enrollments { get; set; }
    //public static List<CourseReview> CourseReviews { get; set; }
    //public static List<Lecture> Lectures { get; set; }
    //public static List<LectureComment> LectureComments { get; set; }
    //public static List<LectureReaction> LectureReactions { get; set; }
    //public static List<YogaPose> YogaPoses { get; set; }

    //public static List<Multimedia> Multimedia { get; set; }
}