using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Objects.UObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuse.AppUtils;

public static class ProviderUtils
{
    public static bool TryGetPackageIndexExport<T>(FPackageIndex? packageIndex, out T export) where T : UObject
    {
        return packageIndex!.TryLoad(out export);
    }
}
