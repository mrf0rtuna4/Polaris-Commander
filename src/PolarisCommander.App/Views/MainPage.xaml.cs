using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PolarisCommander_App.Models;
using PolarisCommander_App.ViewModels;

namespace PolarisCommander_App;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        ViewModel = App.Services.GetRequiredService<MainViewModel>();
        InitializeComponent();
    }

    public MainViewModel ViewModel { get; }

    private void OnNavigationSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItemContainer?.Tag is not string tag)
        {
            return;
        }

        string targetPath = tag switch
        {
            "documents" => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "downloads" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"),
            _ => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        };

        ViewModel.NavigateTo(targetPath);
        Bindings.Update();
    }

    private void OnNavigateUpClicked(object sender, RoutedEventArgs e)
    {
        if (!ViewModel.CanNavigateUp())
        {
            return;
        }

        ViewModel.NavigateUp();
        Bindings.Update();
    }

    private void OnItemClicked(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is not FileSystemItem item || !item.IsDirectory)
        {
            return;
        }

        ViewModel.NavigateTo(item.FullPath);
        Bindings.Update();
    }
}