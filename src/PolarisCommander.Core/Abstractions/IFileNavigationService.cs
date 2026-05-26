using PolarisCommander.Core.Models;

namespace PolarisCommander.Core.Abstractions;

public interface IFileNavigationService
{
    Task<IReadOnlyList<FileItem>> NavigateAsync(string path);
}