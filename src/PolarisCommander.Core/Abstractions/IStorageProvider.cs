using PolarisCommander.Core.Models;

namespace PolarisCommander.Core.Abstractions;

public interface IStorageProvider
{
    string Name { get; }

    bool IsRemote { get; }

    Task ConnectAsync();

    Task DisconnectAsync();

    Task<IReadOnlyList<FileItem>> ListAsync(string path);
}