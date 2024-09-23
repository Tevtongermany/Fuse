using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using Fuse.Models;

namespace Fuse.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty] private int selectedInstallation = 0;
    [ObservableProperty] private List<FortniteFileInstallation> installs = new();
}