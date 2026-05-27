namespace PolarisCommander_App.Services.Abstractions;

public interface INavigationService
{
    event EventHandler<string>? CurrentPathChanged;

    string CurrentPath { get; }

    void NavigateTo(string path);

    bool CanNavigateUp();

    void NavigateUp();
}