namespace PolarisCommander_App.Services.Abstractions;

public interface INavigationService
{
    event EventHandler<string>? CurrentPathChanged;

    string CurrentPath { get; }

    bool CanNavigateBack { get; }

    bool CanNavigateForward { get; }

    void NavigateTo(string path);

    bool CanNavigateUp();

    void NavigateUp();

    void NavigateBack();

    void NavigateForward();

    void Refresh();
}