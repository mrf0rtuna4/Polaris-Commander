using System.Collections.ObjectModel;
using PolarisCommander_App.Models;
using PolarisCommander_App.Services.Abstractions;

namespace PolarisCommander_App.ViewModels;

public sealed class MainViewModel
{
    private readonly IFileSystemService _fileSystemService;
    private readonly INavigationService _navigationService;

    public MainViewModel(IFileSystemService fileSystemService, INavigationService navigationService)
    {
        _fileSystemService = fileSystemService;
        _navigationService = navigationService;
        _navigationService.CurrentPathChanged += OnCurrentPathChanged;
        LoadItems();
    }

    public ObservableCollection<FileSystemItem> Items { get; } = [];

    public string CurrentPath => _navigationService.CurrentPath;

    public void NavigateTo(string path)
    {
        _navigationService.NavigateTo(path);
    }

    public void NavigateUp()
    {
        _navigationService.NavigateUp();
    }

    public bool CanNavigateUp()
    {
        return _navigationService.CanNavigateUp();
    }

    private void OnCurrentPathChanged(object? sender, string path)
    {
        LoadItems();
    }

    private void LoadItems()
    {
        Items.Clear();
        foreach (FileSystemItem item in _fileSystemService.GetItems(_navigationService.CurrentPath))
        {
            Items.Add(item);
        }
    }
}