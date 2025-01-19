namespace Contract.Requests.Community.MeetingRequests.Dtos;

public sealed class QueryMeetingDto : PagingQueryDto
{
    public Guid? CreatorId { get; set; }

    public string? Title { get; set; }
    public DateTime? StartAfter { get; set; }
    public DateTime? StartBefore { get; set; }
    public DateTime? EndAfter { get; set; }
    public DateTime? EndBefore { get; set; }
    public int? MaxParticipants { get; set; }
    public List<Guid>? Participants { get; set; }
}
