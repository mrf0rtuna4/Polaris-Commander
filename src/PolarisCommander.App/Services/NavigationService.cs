using PolarisCommander_App.Services.Abstractions;

namespace PolarisCommander_App.Services;

public sealed class NavigationService : INavigationService
{
    private readonly Stack<string> _backHistory = new();
    private readonly Stack<string> _forwardHistory = new();
    private string _currentPath;

    public NavigationService()
    {
        _currentPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    }

    public event EventHandler<string>? CurrentPathChanged;

    public string CurrentPath => _currentPath;

    public bool CanNavigateBack => _backHistory.Count > 0;

    public bool CanNavigateForward => _forwardHistory.Count > 0;

    public void NavigateTo(string path)
    {
        string? normalizedPath = NormalizeExistingPath(path);
        if (normalizedPath is null || PathsMatch(normalizedPath, _currentPath))
        {
            return;
        }

        _backHistory.Push(_currentPath);
        _forwardHistory.Clear();
        SetCurrentPath(normalizedPath);
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

    public void NavigateBack()
    {
        if (!CanNavigateBack)
        {
            return;
        }

        _forwardHistory.Push(_currentPath);
        SetCurrentPath(_backHistory.Pop());
    }

    public void NavigateForward()
    {
        if (!CanNavigateForward)
        {
            return;
        }

        _backHistory.Push(_currentPath);
        SetCurrentPath(_forwardHistory.Pop());
    }

    public void Refresh()
    {
        CurrentPathChanged?.Invoke(this, _currentPath);
    }

    private static string? NormalizeExistingPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
        {
            return null;
        }

        return Path.GetFullPath(path);
    }

    private static bool PathsMatch(string left, string right)
    {
        return string.Equals(
            Path.TrimEndingDirectorySeparator(left),
            Path.TrimEndingDirectorySeparator(right),
            StringComparison.OrdinalIgnoreCase);
    }

    private void SetCurrentPath(string path)
    {
        _currentPath = path;
        CurrentPathChanged?.Invoke(this, _currentPath);
    }
}