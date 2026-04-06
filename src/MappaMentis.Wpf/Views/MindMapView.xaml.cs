using System.Windows.Controls;
using MappaMentis.Wpf.ViewModels;

namespace MappaMentis.Wpf.Views;

/// <summary>
/// Interaction logic for MindMapView.xaml
/// </summary>
public partial class MindMapView : UserControl
{
    public MindMapView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
    {
        if (DataContext is MindMapViewModel viewModel)
        {
            viewModel.SetRadialView(RadialView);
        }
    }
}
