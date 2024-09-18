using Avalonia;
using Avalonia.Controls;
using Avalonia.Logging;
using Avalonia.Markup.Xaml;
using Fuse;

namespace Fuse.Views;

public partial class AppWindow : Window
{
    public AppWindow()
    {
        InitializeComponent();
        DataContext = App.AppVM;
    }
}