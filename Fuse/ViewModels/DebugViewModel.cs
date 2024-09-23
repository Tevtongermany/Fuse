using CommunityToolkit.Mvvm.Input;
using Fuse.AppUtils;
using Fuse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CUE4Parse.UE4.Versions;
using Serilog;
using CUE4Parse.UE4.Objects.Engine;
using Fuse.Export;
using System.IO;
using Avalonia.Controls;

namespace Fuse.ViewModels;

public partial class DebugViewModel : ViewModelBase
{

    private string pathtoinstall;
    public string Pathtoinstall
    {  get { return pathtoinstall; } set { pathtoinstall = value; } }

    [RelayCommand]
    private void SaveAppSettings()
    {
        AppSettings.Save();
    }

    [RelayCommand]
    private void LoadAppSettings()
    {
        AppSettings.Load();
    }
    [RelayCommand]
    private void LoadCue()
    {
        
        App.Cue4ParseVM.Initialize();
    }

    [RelayCommand]
    private void Text()
    {
        Log.Information("{0}", "Hello");
    }

    [RelayCommand]
    private async Task GimmeAthenaTerrain()
    {
        var AthenaTerrain = await App.Cue4ParseVM.FuseFileProvider.TryLoadObjectAsync("FortniteGame/Content/Athena/Maps/Athena_Terrain.Athena_Terrain");
        
        Log.Information("{0}", "AthenaTerrain my beloved");
    }

    [RelayCommand]
    private async Task ExportPOI()
    {
        UWorld bridge = (UWorld)await App.Cue4ParseVM.FuseFileProvider.TryLoadObjectAsync("FortniteGame/Content/Athena/Maps/POI/Athena_POI_LakeIsland_001.Athena_POI_LakeIsland_001");
        string expstring = HMapExporter.Export(bridge);
        await App.Clipboard.SetTextAsync(expstring);
        File.WriteAllText($"C:\\Users\\tevtongermany\\Downloads\\{bridge.Name.ToString()}.txt",expstring);
    }

    [RelayCommand]
    private void InsertGameInstallIntoAppSettings()
    {
        FortniteFileInstallation predefInstall = new FortniteFileInstallation();
        List<AesKey> keys = new List<AesKey>();
        keys.Add(new AesKey("0x2CCDFD22AD74FBFEE693A81AC11ACE57E6D10D0B8AC5FA90E793A130BC540ED4"));
        predefInstall.AesKeys = keys;
        predefInstall.VersionName = "3.00";
        predefInstall.InstallPath = "G:\\3.0\\3.0\\FortniteGame\\Content\\Paks";
        predefInstall.GameVersion = EGame.GAME_UE4_20;
        AppSettings.Current.Installs.Add(predefInstall);
        Log.Information("guh");
    }
}
