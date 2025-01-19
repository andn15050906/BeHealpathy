namespace Contract.Requests.Community.MeetingRequests.Dtos;

public sealed class CreateMeetingParticipantDto
{
    public Guid UserId { get; set; }
    public bool IsHost { get; set; }
}
