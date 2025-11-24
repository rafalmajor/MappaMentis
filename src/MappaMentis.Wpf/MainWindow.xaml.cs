using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MappaMentis.Wpf.Views;

namespace MappaMentis.Wpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void GetStartedButton_Click(object sender, RoutedEventArgs e)
    {
        DashboardView dashboardView = new DashboardView();
        dashboardView.Show();
        this.Close();
    }
}