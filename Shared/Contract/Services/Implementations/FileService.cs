using Contract.Domain.Shared.MultimediaBase;
using Contract.Helpers.Storage;
using Contract.Services.Contracts;

namespace Contract.Services.Implementations;

public sealed class FileService : IFileService
{
    private readonly IStorageService _storage;

    public FileService(IStorageService storageService)
    {
        _storage = storageService;
    }






    public async Task<Multimedia?> SaveImage(MediaWithStream? media)
    {
        return media is not null
            ? await _storage.SaveFile(media)
            : null;
    }

    public async Task<Multimedia?> ReplaceImage(MediaWithStream? media, string oldPath)
    {
        if (!string.IsNullOrWhiteSpace(oldPath))
            await _storage.DeleteFile(oldPath);
        return await SaveImage(media);
    }

    public async Task<bool> DeleteImage(string path)
    {
        if (!string.IsNullOrWhiteSpace(path))
            return await _storage.DeleteFile(path);
        return false;
    }






    public async Task<List<Multimedia?>> SaveMedias(List<MediaWithStream?> medias)
    {
        List<Task<Multimedia?>> tasks = new();
        for (var i = 0; i < medias.Count; i++)
        {
            var media = medias[i];

            tasks.Add(
                media is not null
                    ? _storage.SaveFile(media)
                    : Task.FromResult<Multimedia?>(null)
            );
        }
        await Task.WhenAll(tasks);

        List<Multimedia?> result = new();
        foreach (var task in tasks)
            result.Add(task.Result);
        return result;
    }

    public async Task<List<Multimedia>> SaveNotNullMedias(List<MediaWithStream?> medias)
    {
        List<Task<Multimedia?>> tasks = new();
        for (var i = 0; i < medias.Count; i++)
        {
            var media = medias[i];

            tasks.Add(
                media is not null
                    ? _storage.SaveFile(media)
                    : Task.FromResult<Multimedia?>(null)
            );
        }
        await Task.WhenAll(tasks);

        List<Multimedia> result = new();
        foreach (var task in tasks)
            if (task.Result is not null)
                result.Add(task.Result);
        return result;
    }

    public async Task<List<Multimedia?>> UpdateMedias(List<MediaWithStream?> medias, List<string>? removedIdentifiers)
    {
        Task<bool[]> deleteTask = removedIdentifiers is not null
            ? _storage.DeleteFiles(removedIdentifiers)
            : Task.FromResult(Array.Empty<bool>());

        Task<List<Multimedia?>> saveTask = SaveMedias(medias);

        await Task.WhenAll(deleteTask, saveTask);
        return saveTask.Result;
    }

    public async Task<List<Multimedia>> UpdateNotNullMedias(List<MediaWithStream?> medias, List<string>? removedIdentifiers)
    {
        Task<bool[]> deleteTask = removedIdentifiers is not null
            ? _storage.DeleteFiles(removedIdentifiers)
            : Task.FromResult(Array.Empty<bool>());

        Task<List<Multimedia>> saveTask = SaveNotNullMedias(medias);

        await Task.WhenAll(deleteTask, saveTask);
        return saveTask.Result;
    }

    public async Task<bool[]> DeleteMedias(List<string>? removedIdentifiers)
    {
        return removedIdentifiers is not null
            ? await _storage.DeleteFiles(removedIdentifiers)
            : Array.Empty<bool>();
    }
}
