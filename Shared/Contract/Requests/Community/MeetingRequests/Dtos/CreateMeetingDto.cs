namespace Contract.Requests.Community.MeetingRequests.Dtos;

public sealed class CreateMeetingDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public int MaxParticipants { get; set; }

    public List<CreateMeetingParticipantDto> Participants { get; set; }
}