using System.Windows;

namespace MappaMentis.Wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Show splash screen
        SplashScreen splashScreen = new SplashScreen();
//        splashScreen.Show();

        // Create and show main window
        MainWindow mainWindow = new MainWindow();
        
        // Close splash screen and show main window after a delay
        System.Threading.Tasks.Task.Delay(2000).ContinueWith(_ =>
        {
            Dispatcher.Invoke(() =>
            {
                mainWindow.Show();
                splashScreen.Close();
            });
        });
    }
}
