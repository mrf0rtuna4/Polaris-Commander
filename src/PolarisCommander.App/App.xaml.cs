using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using PolarisCommander.Core.Abstractions;
using PolarisCommander.Infrastructure.Services;
using PolarisCommander.UI.ViewModels;

namespace PolarisCommander.App;

public partial class App : Application
{
    public static IHost HostContainer { get; private set; } = null!;

    public App()
    {
        InitializeComponent();

        HostContainer = Microsoft.Extensions.Hosting.Host
            .CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<IStorageProvider, LocalFileProvider>();

                services.AddSingleton<IFileNavigationService, FileNavigationService>();

                services.AddSingleton<MainViewModel>();

                services.AddSingleton<MainWindow>();
            })
            .Build();
    }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        await HostContainer.StartAsync();

        var window = HostContainer.Services.GetRequiredService<MainWindow>();

        window.Activate();
    }
}