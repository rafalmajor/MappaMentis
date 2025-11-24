using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MappaMentis.Wpf.Views;

namespace MappaMentis.Wpf.ViewModels;

/// <summary>
/// ViewModel for Welcome/MainWindow content
/// </summary>
public partial class WelcomeViewModel : ObservableObject
{
    public string WelcomeTitle => "Welcome to MappaMentis";
    public string WelcomeSubtitle => "Your Mind Mapping Application";

    [RelayCommand]
    private void GetStarted()
    {
        DashboardView dashboardView = new DashboardView();
        dashboardView.Show();

        // Close the main window
        //System.Windows.Application.Current.MainWindow?.Close();
    }
}
