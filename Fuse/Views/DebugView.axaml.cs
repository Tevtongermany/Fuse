using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Fuse.ViewModels;

namespace Fuse.Views;

public partial class DebugView : ViewBase<DebugViewModel>
{
    public DebugView()
    {
        InitializeComponent();
    }
}