namespace Contract.Requests.Notifications.Dtos;

public sealed class CreateAdminMessageDto
{
    public string Message { get; init; } = string.Empty;
    public Guid ReceiverId { get; init; }
}