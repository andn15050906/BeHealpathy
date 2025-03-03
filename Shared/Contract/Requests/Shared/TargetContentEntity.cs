namespace Contract.Requests.Shared;

public enum TargetContentEntity : byte
{
    Message,
    Reaction,

    ChatGroup,
    Meeting,

    Course,
    Lecture,

    Article,
    Media,

    Routine,
    Survey,

    Preference
}