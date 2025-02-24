using CloudinaryDotNet;
using Contract.Domain.Shared.MultimediaBase;
using Contract.Helpers.Storage;
using Contract.Services.Contracts;

namespace Contract.Services.Implementations;

public sealed class CloudStorageService : IStorageService
{
    private readonly Cloudinary _client;

    public CloudStorageService(CloudStorageConfig config)
    {
        _client = new Cloudinary($"cloudinary://{config.ApiKey}:{config.Secret}@{config.Provider}");
        _client.Api.Secure = true;
    }






    public async Task<List<Multimedia?>> SaveFiles(List<MediaWithStream> medias)
    {
        if (medias.Count == 0)
            return [];

        List<Task<Multimedia?>> tasks = [];
        for (var i = 0; i < medias.Count; i++)
            tasks[i] = SaveFile(medias[i]);
        return [.. (await Task.WhenAll(tasks))];
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
        try
        {
            var uploadFileResult = await _client.UploadAsync(
                new CloudinaryDotNet.Actions.RawUploadParams()
                {
                    File = new FileDescription(media.Title, media.Stream),
                    DisplayName = media.Title
                });

            if (uploadFileResult != null && uploadFileResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var identifier = uploadFileResult.FullyQualifiedPublicId;
                return new Multimedia(media.SourceId, identifier, media.Type, uploadFileResult.SecureUrl.ToString(), media.Title);
            }
            return null;
        }
        catch (Exception /*ex*/)
        {
            return null;
        }
    }

    public async Task<bool> DeleteFile(string identifier/*, IAppLogger logger*/)
    {
        if (!string.IsNullOrWhiteSpace(identifier))
        {
            var publicId = identifier.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Last(); ;
            var deleteResult = await _client.DeleteResourcesAsync(publicId);
            return deleteResult.DeletedCounts.First().Value.Original > 0;
        }
        return false;
    }
}