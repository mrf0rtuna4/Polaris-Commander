using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PolarisCommander_App.Models;
using PolarisCommander_App.Services.Abstractions;

namespace PolarisCommander_App.ViewModels;

public sealed class MainViewModel : INotifyPropertyChanged
{
    private readonly IFileSystemService _fileSystemService;
    private readonly INavigationService _navigationService;

    public MainViewModel(IFileSystemService fileSystemService, INavigationService navigationService)
    {
        _fileSystemService = fileSystemService;
        _navigationService = navigationService;
        _navigationService.CurrentPathChanged += OnCurrentPathChanged;

        LoadDrives();
        LoadItems();
        UpdateNavigationState();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<FileSystemItem> Items { get; } = [];

    public ObservableCollection<BreadcrumbItem> Breadcrumbs { get; } = [];

    public ObservableCollection<string> Drives { get; } = [];

    public string CurrentPath => _navigationService.CurrentPath;

    public string? SelectedDrive => Path.GetPathRoot(CurrentPath);

    public bool CanNavigateBack => _navigationService.CanNavigateBack;

    public bool CanNavigateForward => _navigationService.CanNavigateForward;

    public bool CanNavigateUp => _navigationService.CanNavigateUp();

    public void NavigateTo(string path)
    {
        _navigationService.NavigateTo(path);
    }

    public void NavigateToBreadcrumb(BreadcrumbItem breadcrumb)
    {
        _navigationService.NavigateTo(breadcrumb.Path);
    }

    public void NavigateUp()
    {
        _navigationService.NavigateUp();
    }

    public void NavigateBack()
    {
        _navigationService.NavigateBack();
    }

    public void NavigateForward()
    {
        _navigationService.NavigateForward();
    }

    public void Refresh()
    {
        _navigationService.Refresh();
    }

    private void OnCurrentPathChanged(object? sender, string path)
    {
        LoadItems();
        UpdateNavigationState();
    }

    private void LoadDrives()
    {
        Drives.Clear();

        foreach (DriveInfo drive in DriveInfo.GetDrives().Where(static drive => drive.IsReady))
        {
            Drives.Add(drive.Name);
        }
    }

    private void LoadItems()
    {
        Items.Clear();
        foreach (FileSystemItem item in _fileSystemService.GetItems(_navigationService.CurrentPath))
        {
            Items.Add(item);
        }
        LoadBreadcrumbs();
    }

    private void LoadBreadcrumbs()
    {
        Breadcrumbs.Clear();

        DirectoryInfo? directory = new(_navigationService.CurrentPath);
        List<BreadcrumbItem> breadcrumbs = [];
        while (directory is not null)
        {
            breadcrumbs.Add(new BreadcrumbItem
            {
                DisplayName = string.IsNullOrEmpty(directory.Name) ? directory.FullName : directory.Name,
                Path = directory.FullName,
            });

            directory = directory.Parent;
        }

        breadcrumbs.Reverse();
        foreach (BreadcrumbItem breadcrumb in breadcrumbs)
        {
            Breadcrumbs.Add(breadcrumb);
        }
    }

    private void UpdateNavigationState()
    {
        OnPropertyChanged(nameof(CurrentPath));
        OnPropertyChanged(nameof(SelectedDrive));
        OnPropertyChanged(nameof(CanNavigateBack));
        OnPropertyChanged(nameof(CanNavigateForward));
        OnPropertyChanged(nameof(CanNavigateUp));
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}