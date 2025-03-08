namespace Contract.IntegrationEvents;

public sealed record Events
{
    /*
    QuestionOfTheDay_Answered,
    Mood_Updated,
    Yoga_Practiced,
    Course_Completed
    Media_Viewed
    */
    public record General_Activity_Created(string Content) : BaseIntegrationEvent;



    ////User_Created,
    //User_Signed_In,
    ////User_Requested_Password_Reset,
    ////User_Reseted_Password,
    //User_Updated,
    ////User_Verified_Email,
    //Setting_Updated,
    //Preference_Updated,



    // Notification_Created
    // Notification_Updated



    public record BillCreated(Guid Id) : BaseIntegrationEvent;



    public record Submission_Created(Guid Id) : BaseIntegrationEvent;
    public record Routine_Created(Guid Id) : BaseIntegrationEvent;
    public record Routine_Updated(Guid Id) : BaseIntegrationEvent;
    //Routine_Deleted,
    public record RoutineLog_Created(Guid Id) : BaseIntegrationEvent;
    public record RoutineLog_Updated(Guid Id) : BaseIntegrationEvent;
    //RoutineLog_Deleted,
    public record DiaryNote_Created(Guid Id) : BaseIntegrationEvent;
    public record DiaryNote_Updated(Guid Id) : BaseIntegrationEvent;
    //DiaryNote_Deleted,



    public record Article_Created(Guid Id) : BaseIntegrationEvent;
    //Article_Updated,
    //Article_Deleted,
    public record ArticleComment_Created(Guid Id) : BaseIntegrationEvent;
    //ArticleComment_Updated,
    //ArticleComment_Deleted,
    public record ArticleReaction_Created(Guid Id) : BaseIntegrationEvent;
    //ArticleReaction_Deleted,
    public record MediaResource_Created(Guid Id) : BaseIntegrationEvent;
    //MediaResource_Updated,
    //MediaResource_Deleted,



    public record Conversation_Created(Guid Id) : BaseIntegrationEvent;
    //Conversation_Updated,
    public record Conversation_Joined(Guid Id) : BaseIntegrationEvent;
    public record Conversation_Left(Guid Id) : BaseIntegrationEvent;
    //Conversation_Deleted,
    public record ChatMessage_Created(Guid Id) : BaseIntegrationEvent;
    //ChatMessage_Updated,
    //ChatMessage_Deleted,
    public record MessageReaction_Created(Guid Id) : BaseIntegrationEvent;
    //MessageReaction_Deleted,
    public record Meeting_Created(Guid Id) : BaseIntegrationEvent;
    public record Meeting_Joined(Guid Id) : BaseIntegrationEvent;
    //Meeting_Left,



    public record Advisor_Created(Guid Id) : BaseIntegrationEvent;
    //Advisor_Updated,
    public record Course_Created(Guid Id) : BaseIntegrationEvent;
    //Course_Updated,
    public record Course_Enrolled(Guid Id) : BaseIntegrationEvent;
    public record Course_Unenrolled(Guid Id) : BaseIntegrationEvent;
    public record CourseReview_Created(Guid Id) : BaseIntegrationEvent;
    //CourseReview_Updated,
    //CourseReview_Deleted,
    public record LectureComment_Created(Guid Id) : BaseIntegrationEvent;
    //LectureComment_Updated,
    //LectureReaction_Deleted,
}