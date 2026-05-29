using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.System;
using PolarisCommander_App.Models;
using PolarisCommander_App.ViewModels;

namespace PolarisCommander_App;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        try
        {
            ViewModel = App.Services.GetRequiredService<MainViewModel>();

            InitializeComponent();
        }
        catch (Exception ex)
        {
            File.WriteAllText(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "mainpage-crash.txt"),
                ex.ToString());

            throw;
        }
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

        NavigateTo(targetPath);
    }

    private void OnDriveSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ComboBox comboBox || comboBox.SelectedItem is not string drive)
        {
            return;
        }

        if (string.Equals(drive, ViewModel.SelectedDrive, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        NavigateTo(drive);
    }

    private void OnNavigateBackClicked(object sender, RoutedEventArgs e)
    {
        ViewModel.NavigateBack();
        Bindings.Update();
    }

    private void OnNavigateForwardClicked(object sender, RoutedEventArgs e)
    {
        ViewModel.NavigateForward();
        Bindings.Update();
    }

    private void OnNavigateUpClicked(object sender, RoutedEventArgs e)
    {

        ViewModel.NavigateUp();
        Bindings.Update();
    }

    private void OnRefreshClicked(object sender, RoutedEventArgs e)
    {
        ViewModel.Refresh();
        Bindings.Update();
    }

    private void OnBreadcrumbItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
    {
        if (args.Item is BreadcrumbItem breadcrumb)
        {
            ViewModel.NavigateToBreadcrumb(breadcrumb);
            Bindings.Update();
        }
    }

    private void OnItemClicked(object sender, ItemClickEventArgs e)
    {
        NavigateIntoItem(e.ClickedItem);
    }

    private void OnFileListKeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key != VirtualKey.Enter || FileListView.SelectedItem is null)
        {
            return;
        }

        NavigateIntoItem(FileListView.SelectedItem);
        e.Handled = true;
    }

    private void NavigateIntoItem(object? item)
    {
        if (item is not FileSystemItem fileSystemItem || !fileSystemItem.IsDirectory)
        {
            return;
        }

        NavigateTo(fileSystemItem.FullPath);
    }

    private void NavigateTo(string path)
    {
        ViewModel.NavigateTo(path);
        Bindings.Update();
    }
}