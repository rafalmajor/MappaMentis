using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MappaMentis.Wpf.Services;

/// <summary>
/// Navigation service for switching between views
/// </summary>
public class NavigationService : INotifyPropertyChanged
{
    private object? _currentView;
    private MainWindowViewModel? _mainWindowViewModel;

    public object? CurrentView
    {
        get => _currentView;
        set
        {
            if (_currentView != value)
            {
                _currentView = value;
                if (_mainWindowViewModel != null)
                {
                    _mainWindowViewModel.CurrentView = value;
                }
                OnPropertyChanged();
            }
        }
    }

    public void SetMainWindowViewModel(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
