namespace Contract.Requests.Community.MeetingRequests.Dtos;

public sealed class UpdateMeetingDto
{
    public Guid Id { get; set; }

    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public int? MaxParticipants { get; set; }

    public List<CreateMeetingParticipantDto>? AddedParticipants { get; set; }
    public List<Guid>? RemovedParticipants { get; set; }
}