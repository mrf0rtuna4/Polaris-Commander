using PolarisCommander_App.Models;
using PolarisCommander_App.Services.Abstractions;

namespace PolarisCommander_App.Services;

public sealed class FileSystemService : IFileSystemService
{
    public IReadOnlyList<FileSystemItem> GetItems(string path)
    {
        if (!Directory.Exists(path))
        {
            return [];
        }

        List<FileSystemItem> items = [];
        DirectoryInfo directoryInfo = new(path);

        foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
        {
            items.Add(new FileSystemItem
            {
                Name = directory.Name,
                FullPath = directory.FullName,
                IsDirectory = true,
            });
        }

        foreach (FileInfo file in directoryInfo.GetFiles())
        {
            items.Add(new FileSystemItem
            {
                Name = file.Name,
                FullPath = file.FullName,
                IsDirectory = false,
            });
        }

        return items;
    }
}