using Microsoft.UI.Xaml;

namespace PolarisCommander_App;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        try
        {
            InitializeComponent();

            ExtendsContentIntoTitleBar = true;

            SetTitleBar(AppTitleBar);

            RootFrame.Navigate(typeof(MainPage));
        }
        catch (Exception ex)
        {
            File.WriteAllText(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "mainwindow-crash.txt"),
                ex.ToString());

            throw;
        }
    }
}
