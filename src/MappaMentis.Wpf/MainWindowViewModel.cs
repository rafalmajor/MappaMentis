using CommunityToolkit.Mvvm.ComponentModel;
using MappaMentis.Wpf.Services;
using MappaMentis.Wpf.ViewModels;

namespace MappaMentis.Wpf;

/// <summary>
/// ViewModel for MainWindow to manage view navigation
/// </summary>
public partial class MainWindowViewModel : ObservableObject
{
    private readonly NavigationService _navigationService;

    [ObservableProperty]
    private object? currentView;

    public MainWindowViewModel()
    {
        _navigationService = new NavigationService();
        _navigationService.SetMainWindowViewModel(this);
        
        // Initialize with WelcomeViewModel
        CurrentView = new WelcomeViewModel(_navigationService);
    }
}
