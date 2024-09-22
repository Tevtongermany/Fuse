
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CUE4Parse.Compression;
using CUE4Parse.Encryption.Aes;
using CUE4Parse.FileProvider;
using CUE4Parse.UE4.AssetRegistry;
using CUE4Parse.UE4.AssetRegistry.Objects;
using CUE4Parse.UE4.Versions;
using Fuse.AppUtils;
using Fuse.Models;
using Fuse.Models.Endpoint;
using Serilog;

namespace Fuse.ViewModels;

public class Cue4ParseViewModel : ViewModelBase
{
    public FuseFileProvider FuseFileProvider = new FuseFileProvider(AppSettings.Current.Installs[0].InstallPath, new VersionContainer(AppSettings.Current.Installs[0].GameVersion));
    public readonly List<FAssetData> AssetDataBuffers = new(); // 😘
    public FAssetRegistryState? AssetRegistry; // My beloved AssetRegistry


    public async Task Initialize()
    {
        var oodlePath = Path.Combine(App.DataFolder.FullName, OodleHelper.OODLE_DLL_NAME);
        Log.Information("{0}", oodlePath);
        if (!File.Exists(oodlePath)) await OodleHelper.DownloadOodleDllAsync(oodlePath);
        OodleHelper.Initialize(oodlePath);

        Log.Information("{0}", "Init Fuse File Provider");
        FuseFileProvider.Initialize();
        Log.Information("{0}", "Loading Aes Keys");
        await LoadKeys();
        Log.Information("{0}", "Loading Asset Registry");
        await LoadAssetReg();
    }

    public async Task LoadAssetReg()
    {
        var assetArchive = await FuseFileProvider.TryCreateReaderAsync("FortniteGame/AssetRegistry.bin");
        if (assetArchive is not null)
        {
            Log.Information("{0}", "Asset Reg is not null, we chillin");
            AssetRegistry = new FAssetRegistryState(assetArchive);
            AssetDataBuffers.AddRange(AssetRegistry.PreallocatedAssetDataBuffers);
            return;
        }
        Log.Error("{0}", "Asset Reg is null, its over");
        
    }
    public async Task LoadKeys()
    {
        var mounted = 0;
        foreach (var vfs in FuseFileProvider.UnloadedVfs.ToArray())
        {
            foreach (var key in AppSettings.Current.Installs[0].AesKeys)
            {
                mounted += await FuseFileProvider.SubmitKeyAsync(vfs.EncryptionKeyGuid, new FAesKey(key.Hex));
            }
        }
    }

}
