using Contract.Requests.Shared.BaseDtos.Media;

namespace Contract.Requests.Community.ConversationRequests.Dtos;

public sealed class UpdateConversationDto
{
    public Guid Id { get; set; }

    public string? Title { get; set; }
    public bool? IsPrivate { get; set; }
    public CreateMediaDto? Thumb { get; set; }

    public List<CreateConversationMemberDto>? AddedMembers { get; set; }
    public List<Guid>? RemovedMembers { get; set; }
}
