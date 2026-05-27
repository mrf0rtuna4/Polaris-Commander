using PolarisCommander_App.Services.Abstractions;

namespace PolarisCommander_App.Services;

public sealed class NavigationService : INavigationService
{
    private string _currentPath;

    public NavigationService()
    {
        _currentPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    }

    public event EventHandler<string>? CurrentPathChanged;

    public string CurrentPath => _currentPath;

    public void NavigateTo(string path)
    {
        if (!Directory.Exists(path))
        {
            return;
        }

        _currentPath = path;
        CurrentPathChanged?.Invoke(this, _currentPath);
    }

    public bool CanNavigateUp() => Directory.GetParent(_currentPath) is not null;

    public void NavigateUp()
    {
        DirectoryInfo? parent = Directory.GetParent(_currentPath);
        if (parent is null)
        {
            return;
        }

        NavigateTo(parent.FullName);
    }
}