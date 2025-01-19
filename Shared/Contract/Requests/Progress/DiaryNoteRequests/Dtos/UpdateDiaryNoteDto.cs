using Contract.Requests.Shared.BaseDtos.Media;

namespace Contract.Requests.Progress.DiaryNoteRequests.Dtos;

public sealed class UpdateDiaryNoteDto
{
    public Guid Id { get; set; }

    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? Mood { get; set; }
    public string? Theme { get; set; }

    public List<CreateMediaDto>? AddedMedias { get; set; }
    public List<Guid>? RemovedMedias { get; set; }
}
