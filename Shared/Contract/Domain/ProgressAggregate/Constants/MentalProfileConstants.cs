namespace Contract.Domain.ProgressAggregate.Constants;

public sealed class MentalProfileConstants
{
    public const string ISSUE = "Issue";
    public const string GOAL = "Goal";
    public const string SUGGESTION = "Suggestion";

    // Related to Issue
    public const string SYMPTOM = "Symptom";
    public const string IMPACT = "Impact";
    public const string RECENT = "Recent";
    public const string FREQUENCY = "Frequency";

    // Related to suggestion
    public const string JOB = "Job";
    public const string TEMP_MOOD = "TempMood";
    public const string RECENT_MOOD = "RecentMood";
}