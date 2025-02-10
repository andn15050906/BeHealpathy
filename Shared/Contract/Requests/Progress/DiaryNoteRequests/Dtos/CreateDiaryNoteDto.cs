using Contract.Requests.Shared.BaseDtos.Media;

namespace Contract.Requests.Progress.DiaryNoteRequests.Dtos;

public sealed class CreateDiaryNoteDto
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? Mood { get; set; }
    public string? Theme { get; set; }

    public List<CreateMediaDto>? Medias { get; set; }
}