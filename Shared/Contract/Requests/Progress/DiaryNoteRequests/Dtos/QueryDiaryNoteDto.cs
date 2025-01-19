namespace Contract.Requests.Progress.DiaryNoteRequests.Dtos;

public sealed class QueryDiaryNoteDto : PagingQueryDto
{
    public Guid? CreatorId { get; set; }

    public string? Title { get; set; }
    public string? Mood { get; set; }
    public DateTime? StartAfter { get; set; }
    public DateTime? StartBefore { get; set; }
    public DateTime? EndAfter { get; set; }
    public DateTime? EndBefore { get; set; }
}