using CommunityToolkit.Mvvm.ComponentModel;
using MappaMentis.Wpf.Services;

namespace MappaMentis.Wpf.ViewModels;

/// <summary>
/// ViewModel for Dashboard
/// </summary>
public partial class DashboardViewModel : ObservableObject
{
    private readonly NavigationService _navigationService;

    public string DashboardTitle => "Dashboard";
    public string DashboardSubtitle => "Welcome to your mind mapping workspace!";

    public DashboardViewModel(NavigationService navigationService)
    {
        _navigationService = navigationService;
    }
}
