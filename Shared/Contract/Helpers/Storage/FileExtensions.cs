using Contract.Services.Contracts;
using Contract.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Contract.Helpers.Storage;

public static class FileExtensions
{
    public static IServiceCollection AddCloudStorage<TStorage>(this IServiceCollection services, CloudStorageConfig config)
        where TStorage : class, IStorageService
    {
        return services
            .AddSingleton(config)
            .AddScoped<IStorageService, CloudStorageService>()
            .AddScoped<IFileService, FileService>();
    }

    public static IServiceCollection AddLocalStorage<TStorage>(this IServiceCollection services)
        where TStorage : class, IStorageService
    {
        return services
            .AddScoped<IStorageService, LocalStorageService>()
            .AddScoped<IFileService, FileService>();
    }
}