using Contract.Domain.Shared.MultimediaBase.Enums;
using Contract.Requests.Shared.BaseDtos.Media;

namespace Contract.Helpers.Storage;

public sealed class MediaWithStream
{
    //private static readonly List<string> VideoContentTypes
    //    = new() { "video/mp4", "video/avi", "video/quicktime", "video/x-matroska", "video/x-ms-wmv", "video/x-flv", "video/webm" };
    private static readonly List<string> VideoExtensions
        = [".mp4", ".avi", ".mov", ".mkv", ".wmv", ".flv", ".webm", ".mpeg", ".ogv"];
    private static readonly List<string> DocumentContentTypes
        = [ ".apng", ".avif", ".bmp", ".gif", ".jpeg", ".jpg", ".png", ".svg", ".tiff",
            ".abw", ".csv", ".doc", ".docx", ".odp", ".ods", ".odt", ".pdf", ".ppt", ".pptx", ".rtf", ".txt", ".xls", ".xlsx" ];

    public string Identifier { get; set; }
    public Guid SourceId { get; set; }
    public MediaType Type { get; set; }
    public string? Url { get; set; }
    public string Title { get; set; }

    public Stream Stream { get; set; }
    //public MediaCategory Category { get; set; }



#pragma warning disable CS8618
    private MediaWithStream() { }
#pragma warning restore CS8618

    public static async Task<List<MediaWithStream?>> FromImageDtos(
        IEnumerable<CreateMediaDto> dtos, Guid sourceId, Func<CreateMediaDto, string> identifierFunc)
    {
        List<Task<MediaWithStream?>> tasks = [];
        foreach (var dto in dtos)
            tasks.Add(FromImageDto(dto, sourceId, identifierFunc));
        return [.. (await Task.WhenAll(tasks))];
    }

    public static List<MediaWithStream?> FromDtos(
        IEnumerable<CreateMediaDto?>? dtos, Guid sourceId, Guid id)
    {
        if (dtos is null)
            return [];
        return dtos.Select((dto, index) => FromDto(dto, sourceId, _ => sourceId + "_" + index + "_" + id)).ToList();
    }

    public static async Task<MediaWithStream?> FromImageDto(CreateMediaDto dto, Guid sourceId, Func<CreateMediaDto, string> identifierFunc)
    {
        if (dto.File is null)
            return null;

        return new MediaWithStream
        {
            Identifier = identifierFunc(dto),
            SourceId = sourceId,
            Type = MediaType.Image,
            Url = dto.Url,
            Title = dto.Title,
            Stream = await FileConverter.ToJpg(dto.File),
            //Category = category
        };
    }

    public static MediaWithStream? FromDto(CreateMediaDto? dto, Guid sourceId, Func<CreateMediaDto, string> identifierFunc)
    {
        if (dto is null || dto.File is null)
            return null;

        return new MediaWithStream
        {
            Identifier = identifierFunc(dto),
            SourceId = sourceId,
            Type = GetMediaType(dto.File.FileName),
            Url = dto.Url,
            Title = dto.Title,
            Stream = dto.File.OpenReadStream(),
            //Category = category
        };
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
