using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Fuse.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty] private UserControl activeTab;
        [ObservableProperty] private string greeting;
        public override async Task Initialize()
        {
            Console.WriteLine("MainVM Init");
            greeting = "Hunde";
        }
        

    }
}
