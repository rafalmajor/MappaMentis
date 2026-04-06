using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MappaMentis.Domain.Entities;
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
    
    [ObservableProperty]
    private MindMap mindMap;

    public MindMapViewModel(NavigationService navigationService)
    {
        _navigationService = navigationService;
        mindMap = new MindMap(Guid.NewGuid(), "First Map", "Description");
        mindMap.AddNode(new MindNode(Guid.NewGuid(), MindMap.Id, "Note 1"));
        mindMap.AddNode(new MindNode(Guid.NewGuid(), MindMap.Id, "Note 2"));
        mindMap.AddNode(new MindNode(Guid.NewGuid(), MindMap.Id, "Note 3"));
        mindMap.AddNode(new MindNode(Guid.NewGuid(), MindMap.Id, "Note 4"));
    }

    [RelayCommand]
    private void BackToDashboard()
    {
        var dashboardViewModel = new DashboardViewModel(_navigationService);
        _navigationService.CurrentView = dashboardViewModel;
    }
}
