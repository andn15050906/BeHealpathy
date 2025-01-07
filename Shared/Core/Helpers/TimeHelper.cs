namespace Core.Helpers;

public class TimeHelper
{
    public static DateTime Now { get => DateTime.UtcNow; }
    public static DateTime DefaultDateOfBirth { get => new(2000, 1, 1); }
}