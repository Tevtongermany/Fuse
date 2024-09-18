using Avalonia.Controls;
using Fuse.Service;
using Fuse.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuse;

public class ViewBase<T> : UserControl where T : ViewModelBase, new()
{
    protected readonly T ViewModel;

    public ViewBase(T? viewModel = null, bool initialize = true)
    {
        ViewModel = viewModel ?? ViewModelRegistry.Register<T>();
        DataContext = ViewModel;

        if (initialize)
        {
            TaskService.Run(async () => await ViewModel.Initialize());
        }
    }
}