using System.Text;

namespace Gateway.Services.AI.Recommendation;

public class Input
{
    // Message, Diary, Review
    public class Message(DateTime createdAt, string content)
    {
        public DateTime CreatedAt { get; set; } = createdAt;
        public string Content { get; set; } = content;
    }

    public class Reaction(DateTime createdAt, string sourceContent, string content)
    {
        public DateTime CreatedAt { get; set; } = createdAt;
        public string SourceContent { get; set; } = sourceContent;
        public string Content { get; set; } = content;
    }

    public class Course(DateTime startedAt, string category, string title, string description)
    {
        public DateTime StartedAt { get; set; } = startedAt;
        public string Category { get; set; } = category;
        public string Title { get; set; } = title;
        public string Description { get; set; } = description;
    }

    public class Article(DateTime startedAt, string category, string title, string description)
    {
        public DateTime StartedAt { get; set; } = startedAt;
        public string Category { get; set; } = category;
        public string Title { get; set; } = title;
        public string Description { get; set; } = description;
    }

    public class Media(DateTime startedAt, string description, string title, string type)
    {
        public DateTime StartedAt { get; set; } = startedAt;
        public string Description { get; set; } = description;
        public string Title { get; set; } = title;
        //MediaResourceType
        public string Type { get; set; } = type;
    }

    public class RoutineLog(DateTime startedAt, string content)
    {
        public DateTime StartedAt { get; set; } = startedAt;
        public string Content { get; set; } = content;
    }

    public class Routine(string title, string description, string objective, string frequency, List<RoutineLog> logs)
    {
        public string Title { get; set; } = title;
        public string Description { get; set; } = description;
        public string Objective { get; set; } = objective;
        //Frequency
        public string Frequency { get; set; } = frequency;
        public List<RoutineLog> Logs { get; set; } = logs;
    }

    public class SurveyAnswer(string question, string answer)
    {
        public string Question { get; set; } = question;
        public string Answer { get; set; } = answer;
    }

    public class SurveyBand(string metrics, string score, string rating)
    {
        public string Metrics { get; set; } = metrics;
        public string Score { get; set; } = score;
        public string Rating { get; set; } = rating;
    }

    public class Submission(DateTime startedAt, string surveyName, List<SurveyAnswer> answers, int score, List<SurveyBand> metrics)
    {
        public DateTime StartedAt { get; set; } = startedAt;
        public string SurveyName { get; set; } = surveyName;
        public List<SurveyAnswer> SurveyAnswerInputs { get; set; } = answers;
        public int Score { get; set; } = score;
        public List<SurveyBand> SurveyBandInputs { get; set; } = metrics;
    }

    public class Preference(string setting, string choice)
    {
        public string Setting { get; set; } = setting;
        public string Choice { get; set; } = choice;
    }






    public class DataCollection
    {
        public Guid UserId { get; set; }
        public List<Message> MessageInputs { get; set; } = [];
        public List<Reaction> ReactionInputs { get; set; } = [];
        public List<Course> CourseInputs { get; set; } = [];
        public List<Article> ArticleInputs { get; set; } = [];
        public List<Media> MediaInputs { get; set; } = [];
        public List<Routine> RoutineInputs { get; set; } = [];
        public List<Submission> SubmissionInputs { get; set; } = [];
        public List<Preference> PreferenceInputs { get; set; } = [];
    }

    public class GroupedData
    {
        public List<string> Messages { get; set; } = [];
        public Dictionary<string, string> Content_Reactions { get; set; } = [];
        public List<string> Interests_Category = [];
        public List<string> Interests_Title = [];
        public List<string> Interests_Description = [];
        public Dictionary<string, string> Metrics_Rating = [];
    }
}
