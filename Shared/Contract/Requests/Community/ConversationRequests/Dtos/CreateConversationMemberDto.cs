namespace Contract.Requests.Community.ConversationRequests.Dtos;

public sealed class CreateConversationMemberDto
{
    public Guid UserId { get; set; }
    public bool IsAdmin { get; set; }
}
