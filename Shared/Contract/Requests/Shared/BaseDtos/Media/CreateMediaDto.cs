using Contract.Domain.Shared.MultimediaBase;
using Microsoft.AspNetCore.Http;

namespace Contract.Requests.Shared.BaseDtos.Media;

public class CreateMediaDto
{
    public string? Url { get; set; }
    public IFormFile? File { get; set; }
    public string Title { get; set; }



    public void UpdateAfterSave(Multimedia? media)
    {
        Url = media?.Url ?? string.Empty;
        File = null;
    }
}