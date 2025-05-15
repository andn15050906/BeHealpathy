namespace Contract.Domain.CourseAggregate.Enums;

public enum CourseStatus : byte
{
    Draft,
    Published,

    Ongoing,
    Postponed,
    Completed
}