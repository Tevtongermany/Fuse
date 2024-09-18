using Avalonia.Threading;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace Fuse.Service;

public static class TaskService
{
    private static List<int> RunningHashes = new();

    public static void Run(Func<Task> function, bool oneInstance = false)
    {
        Task.Run(async () =>
        {
            try
            {
                await function().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Log.Error("{0}", e);
            }
        });
    }

    public static async Task RunAsync(Func<Task> function)
    {
        await Task.Run(async () =>
        {
            try
            {
                await function().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Log.Error("{0}", e);
            }
        });
    }

    public static void Run(Action function)
    {
        Task.Run(() =>
        {
            try
            {
                function();
            }
            catch (Exception e)
            {
                Log.Error("{0}", e);
            }
        });
    }

    public static async Task RunAsync(Action function)
    {
        await Task.Run(() =>
        {
            try
            {
                function();
            }
            catch (Exception e)
            {
                Log.Error("{0}", e);
            }
        });
    }

    public static void RunDispatcher(Action function, DispatcherPriority priority = default)
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            try
            {
                function();
            }
            catch (Exception e)
            {
                Log.Error("{0}", e);
            }
        }, priority);
    }

    public static async Task RunDispatcherAsync(Action function, DispatcherPriority priority = default)
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            try
            {
                function();
            }
            catch (Exception e)
            {
                Log.Error("{0}", e);
            }
        }, priority);
    }
}