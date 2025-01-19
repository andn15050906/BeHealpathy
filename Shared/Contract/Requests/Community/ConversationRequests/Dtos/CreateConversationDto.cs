using Contract.Requests.Shared.BaseDtos.Media;

namespace Contract.Requests.Community.ConversationRequests.Dtos;

public sealed class CreateConversationDto
{
    public string Title { get; set; }
    public bool IsPrivate { get; set; }
    public CreateMediaDto Thumb { get; set; }

    public List<CreateConversationMemberDto> Members { get; set; }
}
