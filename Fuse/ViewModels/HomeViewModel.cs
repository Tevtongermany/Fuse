using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuse.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    [RelayCommand]
    public void OnButton() {
        App.MainVM.Greeting = "kys";
        AppUtils.AppSettings.Save();

    }
}
