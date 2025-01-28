using Contract.Domain.Shared.MultimediaBase;
using Contract.Domain.Shared.MultimediaBase.Enums;
using Contract.Helpers.Storage;
using Contract.Requests.Shared.BaseDtos.Media;
using Contract.Services.Contracts;

namespace Contract.Services.Implementations;

public sealed class FileService : IFileService
{
    private readonly IStorageService _storage;

    public FileService(IStorageService storageService)
    {
        _storage = storageService;
    }






    public async Task<List<Multimedia>> SaveMediasAndUpdateDtos(List<(CreateMediaDto, Guid)> dto_sourceIds)
    {
        List<(Multimedia?, MediaWithStream?)> list = [];

        for (var i = 0; i < dto_sourceIds.Count; i++)
        {
            var dto = dto_sourceIds[i].Item1;
            var sourceId = dto_sourceIds[i].Item2;

            if (dto is null)
                continue;

            list.Add(
                !string.IsNullOrWhiteSpace(dto.Url)
                    ? (new Multimedia(sourceId, dto.Url!, MediaType.Other, dto.Url!, dto.Title), null)
                    : (null, MediaWithStream.FromDto(dto, sourceId, _ => $"{sourceId}_{Guid.NewGuid()}"))
            );
        }

        var dtosWithFile = list.Where(_ => _.Item2 is not null).Select(_ => _.Item2).ToList();
        var medias = await SaveNotNullMedias(dtosWithFile);
        var j = 0;
        for (var i = 0; i < list.Count; i++)
            if (list[i].Item1 is null)
            {
                list[i] = (medias[j], null);
                j++;
                dto_sourceIds[i].Item1.UpdateAfterSave(list[i].Item1);
            }

        return list.Select(_ => _.Item1!).ToList();
    }

    public async Task<Multimedia?> SaveImageAndUpdateDto(CreateMediaDto dto, Guid sourceId)
    {
        dto.Url ??= string.Empty;
        if (!string.IsNullOrWhiteSpace(dto.Url))
            return new Multimedia(sourceId, dto.Url!, MediaType.Other, dto.Url!, dto.Title);
        if (dto.File is null)
            return null;

        var mediaWithFile = MediaWithStream.FromDto(dto, sourceId, _ => $"{sourceId}_{Guid.NewGuid()}");
        var media = await SaveImage(mediaWithFile)!;
        dto.UpdateAfterSave(media);
        return media!;
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
        List<Task<Multimedia?>> tasks = [];
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

        List<Multimedia?> result = [];
        foreach (var task in tasks)
            result.Add(task.Result);
        return result;
    }

    public async Task<List<Multimedia>> SaveNotNullMedias(List<MediaWithStream?> medias)
    {
        List<Task<Multimedia?>> tasks = [];
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

        List<Multimedia> result = [];
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
            : [];
    }
}
