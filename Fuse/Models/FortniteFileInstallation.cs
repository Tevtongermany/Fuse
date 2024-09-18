using CommunityToolkit.Mvvm.ComponentModel;
using CUE4Parse.UE4.Versions;

using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuse.Models;

public partial class FortniteFileInstallation : ObservableObject
{
    [ObservableProperty] string versionName = string.Empty;
    [ObservableProperty] EGame gameVersion = EGame.GAME_UE4_27;
    [ObservableProperty] List<AesKey> aesKeys = new();
    [ObservableProperty] string installPath = string.Empty;
}

