using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MappaMentis.Wpf.Services;

namespace MappaMentis.Wpf.ViewModels;

/// <summary>
/// ViewModel for MindMap view
/// </summary>
public partial class MindMapViewModel : ObservableObject
{
    private readonly NavigationService _navigationService;

    [ObservableProperty]
    private string mindMapTitle = "My Mind Map";

    [ObservableProperty]
    private string statusMessage = "Create and manage your mind maps here";

    public MindMapViewModel(NavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    [RelayCommand]
    private void BackToDashboard()
    {
        var dashboardViewModel = new DashboardViewModel(_navigationService);
        _navigationService.CurrentView = dashboardViewModel;
    }
}
