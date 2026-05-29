using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;

using PolarisCommander_App.Services;
using PolarisCommander_App.Services.Abstractions;
using PolarisCommander_App.ViewModels;

namespace PolarisCommander_App;

public partial class App : Application
{
    private Window? _window;

    public App()
    {
        UnhandledException += OnUnhandledException;
        InitializeComponent();
        Services = ConfigureServices();
    }

    public static IServiceProvider Services { get; private set; } = null!;
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        try
        {
            _window = new MainWindow();
            _window.Activate();
        }
        catch (Exception ex)
        {
            string path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "polaris-startup-crash.txt");

            File.WriteAllText(path, ex.ToString());

            throw;
        }
    }

    private static void OnUnhandledException(
        object sender,
        Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        try
        {
            string logDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "PolarisCommander");

            Directory.CreateDirectory(logDirectory);

            string logPath = Path.Combine(logDirectory, "startup-errors.log");

            File.AppendAllText(
                logPath,
                $"[{DateTimeOffset.Now:u}] {e.Exception}\n");
        }
        catch
        {
        }

        e.Handled = true;
    }


    private static IServiceProvider ConfigureServices()
    {
        ServiceCollection services = new();

        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IFileSystemService, FileSystemService>();

        services.AddSingleton<IFtpClientService, StubFtpClientService>();
        services.AddSingleton<ISftpClientService, StubSftpClientService>();

        services.AddTransient<MainViewModel>();

        return services.BuildServiceProvider();
    }
}

file sealed class StubFtpClientService : IFtpClientService
{
    public Task ConnectAsync(string host, int port, string userName, string password, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}

file sealed class StubSftpClientService : ISftpClientService
{
    public Task ConnectAsync(string host, int port, string userName, string password, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
