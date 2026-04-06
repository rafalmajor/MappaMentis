using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MappaMentis.Domain.Entities;
using MappaMentis.Wpf.Services;
using MappaMentis.Wpf.Views;

namespace MappaMentis.Wpf.ViewModels;

/// <summary>
/// ViewModel for MindMap view
/// </summary>
public partial class MindMapViewModel : ObservableObject
{
    private readonly NavigationService _navigationService;
    private MindMapRadialView? _radialView;

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
    }

    /// <summary>
    /// Sets the radial view reference and renders the mind map.
    /// </summary>
    public void SetRadialView(MindMapRadialView radialView)
    {
        _radialView = radialView;
        if (_radialView != null)
        {
            _radialView.RenderMindMap(MindMap);
        }
    }

    [RelayCommand]
    private void BackToDashboard()
    {
        var dashboardViewModel = new DashboardViewModel(_navigationService);
        _navigationService.CurrentView = dashboardViewModel;
    }
}
