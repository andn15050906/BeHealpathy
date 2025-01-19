using Contract.Domain.Shared.MultimediaBase;
using Contract.Helpers.Storage;
using Contract.Services.Contracts;

namespace Contract.Services.Implementations;

public sealed class LocalStorageService : IStorageService
{
    private const string BASE_PATH = "wwwroot/";






    public async Task<List<Multimedia?>> SaveFiles(List<MediaWithStream> medias)
    {
        if (medias.Count == 0)
            return new List<Multimedia?>();

        List<Task<Multimedia?>> tasks = new();
        for (var i = 0; i < medias.Count; i++)
            tasks[i] = SaveFile(medias[i]);
        return (await Task.WhenAll(tasks)).ToList();
    }

    public async Task<bool[]> DeleteFiles(List<string> identifiers)
    {
        List<Task<bool>> tasks = new();
        for (var i = 0; i < identifiers.Count; i++)
            tasks[i] = DeleteFile(identifiers[i]);
        return await Task.WhenAll(tasks);
    }

    public async Task<Multimedia?> SaveFile(MediaWithStream media)
    {
        var identifier = GetPhysicalPath(media.Identifier);

        // clarify DirPath and FilePath
        //string fileName = Path.GetFileName(displayName);
        string dirPath = Path.GetDirectoryName(identifier)!;

        try
        {
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            using var createdStream = File.Create(identifier);
            await media.Stream.CopyToAsync(createdStream);
            media.Stream.Dispose();
            return new Multimedia(media.SourceId, identifier, media.Type, string.Empty, media.Title);
        }
        catch (Exception ex)
        {
            //logger.Warn(e.Message);
            return null;
        }
    }

    public async Task<bool> DeleteFile(string identifier/*, IAppLogger logger*/)
    {
        identifier = GetPhysicalPath(identifier);

        try
        {
            await Task.Run(() => File.Delete(Path.GetFullPath(identifier)!));
            return true;
        }
        catch (Exception e)
        {
            //logger.Warn(e.Message);
            return false;
        }
    }






    private static string GetPhysicalPath(string filePath)
        => BASE_PATH + filePath;
}
