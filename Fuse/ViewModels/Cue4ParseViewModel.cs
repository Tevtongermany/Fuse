
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CUE4Parse.Compression;
using CUE4Parse.FileProvider;
using CUE4Parse.UE4.Versions;
using Fuse.AppUtils;
using Fuse.Models;
using Fuse.Models.Endpoint;

namespace Fuse.ViewModels;

public class Cue4ParseViewModel : ViewModelBase
{
    public FuseFileProvider FuseFileProvider = new FuseFileProvider(AppSettings.Current.Installs[0].InstallPath, new VersionContainer(AppSettings.Current.Installs[0].GameVersion));


    public async Task Initialize()
    {
        var oodlePath = Path.Combine(App.DataFolder.FullName, OodleHelper.OODLE_DLL_NAME);
        if (!File.Exists(oodlePath)) await OodleHelper.DownloadOodleDllAsync(oodlePath);
        OodleHelper.Initialize(oodlePath);

        FuseFileProvider.Initialize();
    }

}
