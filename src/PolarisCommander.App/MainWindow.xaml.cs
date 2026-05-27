using Microsoft.UI.Xaml;
using PolarisCommander.UI.ViewModels;

namespace PolarisCommander.App;

public sealed partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();

        Content.XamlRoot.Changed += async (_, _) =>
        {
            await viewModel.LoadAsync();
        };

        DataContext = viewModel;
    }
}