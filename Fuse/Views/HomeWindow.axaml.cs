using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Fuse.ViewModels;

namespace Fuse.Views;

public partial class HomeWindow : ViewBase<HomeViewModel>
{
    public HomeWindow()
    {
        InitializeComponent();
    }
}