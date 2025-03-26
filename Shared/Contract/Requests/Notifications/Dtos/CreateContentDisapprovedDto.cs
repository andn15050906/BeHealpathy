using Contract.Requests.Shared;

namespace Contract.Requests.Notifications.Dtos;

public sealed class CreateContentDisapprovedDto
{
    public TargetContentEntity TargetContentEntity { get; set; }
    public Guid EntityId { get; set; }
    public string Message { get; set; } = string.Empty;
}