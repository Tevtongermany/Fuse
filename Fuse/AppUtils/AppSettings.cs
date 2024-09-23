using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.IO;
using Fuse.ViewModels;

namespace Fuse.AppUtils;

// this is the most goated thing Half has written 🙏
public partial class AppSettings : ObservableObject
{
    public static SettingsViewModel Current = new();

    private static readonly string DirectoryPath = new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Fuse"));
    private static readonly string FilePath = new(Path.Combine(DirectoryPath, "AppSettings.json"));

    public static void Load()
    {
        if (!Directory.Exists(DirectoryPath)) Directory.CreateDirectory(DirectoryPath);
        if (File.Exists(FilePath)) Current = JsonConvert.DeserializeObject<SettingsViewModel>(File.ReadAllText(FilePath)) ?? new SettingsViewModel();
    }
    public static void Save()
    {
        File.WriteAllText(FilePath, JsonConvert.SerializeObject(Current, Formatting.Indented));
    }
}