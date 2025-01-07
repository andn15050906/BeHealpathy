using Contract.Domain.Shared.MultimediaBase;
using Microsoft.AspNetCore.Http;

namespace Contract.Requests.Shared.Media;

public class CreateMediaDto
{
    public string Identifier { get; set; }          // Set in handler, updated in storageService
    public string? Url { get; set; }
    public IFormFile? File { get; set; }
    public string Title { get; set; }



    public void ReplaceFileWithUrl(Multimedia? media)
    {
        File = null;
        Url = media?.Url ?? string.Empty;
    }
}