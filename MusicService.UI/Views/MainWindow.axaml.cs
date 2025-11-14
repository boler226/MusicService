using Avalonia.Controls;
using MusicService.UI.ViewModels;

namespace MusicService.UI.Views;
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}