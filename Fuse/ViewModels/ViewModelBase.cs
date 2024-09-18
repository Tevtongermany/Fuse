using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;

namespace Fuse.ViewModels;

public class ViewModelBase : ObservableObject
{
    public virtual async Task Initialize()
    {
    }
}


