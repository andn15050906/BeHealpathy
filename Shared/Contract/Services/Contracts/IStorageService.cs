using Contract.Domain.Shared.MultimediaBase;
using Contract.Helpers.Storage;

namespace Contract.Services.Contracts;

public interface IStorageService
{
    Task<List<Multimedia?>> SaveFiles(List<MediaWithStream> medias);
    Task<Multimedia?> SaveFile(MediaWithStream media);
    Task<bool[]> DeleteFiles(List<string> identifiers);
    Task<bool> DeleteFile(string identifier);
}