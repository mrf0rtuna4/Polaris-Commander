using PolarisCommander.Core.Abstractions;
using PolarisCommander.Core.Models;

namespace PolarisCommander.Infrastructure.Services;

public class LocalFileProvider : IStorageProvider
{
    public string Name => "Local";

    public bool IsRemote => false;

    public Task ConnectAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisconnectAsync()
    {
        return Task.CompletedTask;
    }

    public async Task<IReadOnlyList<FileItem>> ListAsync(string path)
    {
        return await Task.Run(() =>
        {
            var directory = new DirectoryInfo(path);

            if (!directory.Exists)
            {
                return new List<FileItem>();
            }

            return directory
                .GetFileSystemInfos()
                .Select(x => new FileItem
                {
                    Name = x.Name,
                    FullPath = x.FullName,
                    IsDirectory = (x.Attributes & FileAttributes.Directory) != 0,
                    Modified = x.LastWriteTime,
                    Size = x is FileInfo file ? file.Length : 0
                })
                .OrderByDescending(x => x.IsDirectory)
                .ThenBy(x => x.Name)
                .ToList();
        });
    }
}