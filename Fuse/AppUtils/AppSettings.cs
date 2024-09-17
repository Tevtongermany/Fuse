using CommunityToolkit.Mvvm.ComponentModel;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuse.App;

// this is the most goated thing Half has written 🙏

public partial class AppSettings : ObservableObject
{
    public static AppSettings Current;

    public static readonly DirectoryInfo DirectoryPath = new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Fuse"));
    public static readonly DirectoryInfo FilePath = new(Path.Combine(DirectoryPath.FullName, "AppSettings.json"));

    public static void Load()
    {
        if (File.Exists(FilePath.FullName))
        {
            Current = JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(FilePath.FullName));
        }

        Current ??= new AppSettings();
    }

    public static void Save()
    {
        File.WriteAllText(FilePath.FullName, JsonConvert.SerializeObject(Current, Formatting.Indented));
    }

    [ObservableProperty] private EGame fnVersion = EGame.GAME_UE5_4;
    [ObservableProperty] private List<CustomAESKey> customAESKeys = new List<CustomAESKey>();

}

public partial class CustomAESKey : ObservableObject
{
    public static CustomAESKey ZERO => new(Globals.ZERO_CHAR);

    [ObservableProperty] private string hex;

    public CustomAESKey(string hex)
    {
        Hex = hex;
    }
}