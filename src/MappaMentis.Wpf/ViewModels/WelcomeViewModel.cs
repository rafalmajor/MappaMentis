using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MappaMentis.Wpf.Services;

namespace MappaMentis.Wpf.ViewModels;

/// <summary>
/// ViewModel for Welcome/MainWindow content
/// </summary>
public partial class WelcomeViewModel : ObservableObject
{
    private readonly NavigationService _navigationService;

    public string WelcomeTitle => "Welcome to MappaMentis";
    public string WelcomeSubtitle => "Your Mind Mapping Application";
    
    public WelcomeViewModel(NavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    [RelayCommand]
    private void GetStarted()
    {
        var dashboardViewModel = new DashboardViewModel(_navigationService);
        _navigationService.CurrentView = dashboardViewModel;
    }
}
