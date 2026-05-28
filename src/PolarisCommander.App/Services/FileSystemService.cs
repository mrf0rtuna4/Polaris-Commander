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

        try
        {
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
        catch (Exception exception) when (IsFileSystemAccessException(exception))
        {
            return [];
        }
    }

    private static bool IsFileSystemAccessException(Exception exception)
    {
        return exception is IOException or UnauthorizedAccessException or DirectoryNotFoundException or PathTooLongException;
    }
}