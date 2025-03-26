using Contract.Requests.Shared;

namespace Contract.Requests.Notifications.Dtos;

public sealed class CreateUserReportDto
{
    public Guid UserId { get; set; }
    public string Message { get; set; } = string.Empty;
    public TargetContentEntity TargetContentEntity { get; set; }
    public Guid ProofEntityId { get; set; }
}