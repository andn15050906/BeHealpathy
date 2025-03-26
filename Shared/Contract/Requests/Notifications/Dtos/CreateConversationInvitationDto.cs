namespace Contract.Requests.Notifications.Dtos;

public sealed class CreateConversationInvitationDto
{
    public List<Guid> UserIds { get; set; }
    public Guid ConversationId { get; set; }
}