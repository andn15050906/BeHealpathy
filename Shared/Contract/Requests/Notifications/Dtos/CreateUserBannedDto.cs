namespace Contract.Requests.Notifications.Dtos;

public sealed class CreateUserBannedDto
{
    public Guid UserId { get; set; }
    public string Message { get; set; } = string.Empty;
}