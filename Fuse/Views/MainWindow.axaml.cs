using Avalonia.Controls;
using Fuse.ViewModels;
using System;

namespace Fuse.Views;

public partial class MainWindow : ViewBase<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
    }

}