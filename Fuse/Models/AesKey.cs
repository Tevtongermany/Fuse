using CommunityToolkit.Mvvm.ComponentModel;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Fuse.Models;
public partial class AesKey : ObservableObject
{
    public static AesKey ZERO => new(Globals.ZERO_CHAR);

    [ObservableProperty] private string hex;

    public AesKey(string hex)
    {
        Hex = hex;
    }
}

