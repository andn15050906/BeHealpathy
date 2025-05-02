namespace Contract.Requests.Community.MeetingRequests.Dtos;

public sealed class QueryMeetingDto : PagingQueryDto
{
    public Guid? CreatorId { get; set; }
    public string? Title { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public int? MaxParticipants { get; set; }
    public List<Guid>? Participants { get; set; }
}
