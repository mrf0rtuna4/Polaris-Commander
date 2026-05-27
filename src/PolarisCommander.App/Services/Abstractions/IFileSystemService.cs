using PolarisCommander_App.Models;

namespace PolarisCommander_App.Services.Abstractions;

public interface IFileSystemService
{
    IReadOnlyList<FileSystemItem> GetItems(string path);
}