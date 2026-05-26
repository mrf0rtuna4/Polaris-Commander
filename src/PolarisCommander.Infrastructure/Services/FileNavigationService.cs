using PolarisCommander.Core.Abstractions;
using PolarisCommander.Core.Models;

namespace PolarisCommander.Infrastructure.Services;

public class FileNavigationService : IFileNavigationService
{
    private readonly IStorageProvider _provider;

    public FileNavigationService(IStorageProvider provider)
    {
        _provider = provider;
    }

    public async Task<IReadOnlyList<FileItem>> NavigateAsync(string path)
    {
        return await _provider.ListAsync(path);
    }
}