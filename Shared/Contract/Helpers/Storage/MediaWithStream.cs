using Contract.Domain.Shared.MultimediaBase.Enums;
using Contract.Requests.Shared.Media;

namespace Contract.Helpers.Storage;

public sealed class MediaWithStream
{
    //private static readonly List<string> VideoContentTypes
    //    = new() { "video/mp4", "video/avi", "video/quicktime", "video/x-matroska", "video/x-ms-wmv", "video/x-flv", "video/webm" };
    private static readonly List<string> VideoExtensions
        = new() { ".mp4", ".avi", ".mov", ".mkv", ".wmv", ".flv", ".webm", ".mpeg", ".ogv" };
    private static readonly List<string> DocumentContentTypes
        = new() { ".apng", ".avif", ".bmp", ".gif", ".jpeg", ".jpg", ".png", ".svg", ".tiff",
                  ".abw", ".csv", ".doc", ".docx", ".odp", ".ods", ".odt", ".pdf", ".ppt", ".pptx", ".rtf", ".txt", ".xls", ".xlsx" };

    public string Identifier { get; set; }
    public MediaType Type { get; set; }
    public string? Url { get; set; }
    public string Title { get; set; }

    public Stream Stream { get; set; }
    //public MediaCategory Category { get; set; }



#pragma warning disable CS8618
    private MediaWithStream() { }
#pragma warning restore CS8618

    public static async Task<List<MediaWithStream?>> FromImageDtos(
        IEnumerable<CreateMediaDto> dtos, Func<CreateMediaDto, string> identifierFunc)
    {
        List<Task<MediaWithStream?>> tasks = new();
        foreach (var dto in dtos)
            tasks.Add(FromImageDto(dto, identifierFunc));
        return (await Task.WhenAll(tasks)).ToList();
    }

    public static List<MediaWithStream?> FromDtos(
        IEnumerable<CreateMediaDto> dtos, Func<CreateMediaDto, string> identifierFunc)
    {
        return dtos.Select(_ => FromDto(_, identifierFunc)).ToList();
    }

    public static async Task<MediaWithStream?> FromImageDto(CreateMediaDto dto, Func<CreateMediaDto, string> identifierFunc)
    {
        if (dto.File is not null)
        {
            return new MediaWithStream
            {
                Identifier = identifierFunc(dto),
                Type = MediaType.Image,
                Url = dto.Url,
                Title = dto.Title,
                Stream = await FileConverter.ToJpg(dto.File),
                //Category = category
            };
        }

        return null;
    }

    public static MediaWithStream? FromDto(CreateMediaDto dto, Func<CreateMediaDto, string> identifierFunc)
    {
        if (dto.File is not null)
        {
            return new MediaWithStream
            {
                Identifier = identifierFunc(dto),
                Type = GetMediaType(dto.File.FileName),
                Url = dto.Url,
                Title = dto.Title,
                Stream = dto.File.OpenReadStream(),
                //Category = category
            };
        }

        return null;
    }



    private static MediaType GetMediaType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        if (VideoExtensions.Contains(extension))
            return MediaType.Video;
        if (DocumentContentTypes.Contains(extension))
            return MediaType.Document;
        return MediaType.Other;
    }
}
