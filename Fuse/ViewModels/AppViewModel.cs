using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fuse.Views;
using Serilog;

namespace Fuse.ViewModels;

public partial class AppViewModel : ViewModelBase
{
    [ObservableProperty] private UserControl? currentView;
    public AppViewModel() 
    {
        SetView<MainWindow>();
        Console.WriteLine("SetView Main Window");
    }

    public void SetView<T>() where T : UserControl, new()
    {
        CurrentView = new T();
    }
}
