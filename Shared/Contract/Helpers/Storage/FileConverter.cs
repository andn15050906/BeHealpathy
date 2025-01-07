using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;

namespace Contract.Helpers.Storage;

/// <summary>
/// Using SixLabors.ImageSharp.
/// System.Drawing.Common is only supported on Windows
/// </summary>
public static class FileConverter
{
    public const string EXTENSION_JPG = ".jpg";



    /// <summary>
    /// Does not close the returned stream
    /// </summary>
    public static async Task<Stream> ToJpg(IFormFile file)
    {
        if (file.ContentType == "image/jpeg")
            return file.OpenReadStream();

        using MemoryStream imgStream = new();
        await file.CopyToAsync(imgStream);
        imgStream.Seek(0, SeekOrigin.Begin);
        using var image = await Image.LoadAsync(imgStream);

        MemoryStream jpgStream = new();
        await image.SaveAsJpegAsync(jpgStream);
        jpgStream.Position = 0;
        return jpgStream;
    }
}