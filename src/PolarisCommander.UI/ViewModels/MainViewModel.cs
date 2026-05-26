using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PolarisCommander.Core.Abstractions;
using PolarisCommander.Core.Models;
using System.Collections.ObjectModel;

namespace PolarisCommander.UI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IFileNavigationService _navigationService;

    [ObservableProperty]
    private string currentPath = "C:\\";

    [ObservableProperty]
    private bool isLoading;

    public ObservableCollection<FileItem> Items { get; } = new();

    public MainViewModel(IFileNavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        try
        {
            IsLoading = true;

            Items.Clear();

            var items = await _navigationService.NavigateAsync(CurrentPath);

            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }
}