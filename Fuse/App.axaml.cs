using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Fuse.ViewModels;
using Fuse.Views;
using System.IO;
using System;
using Avalonia.Controls;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Runtime.InteropServices;

namespace Fuse
{
    public partial class App : Application
    {

        [DllImport("kernel32")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static AppViewModel AppVM = null!;
        public static MainWindowViewModel MainVM => ViewModelRegistry.Get<MainWindowViewModel>();
        public static Cue4ParseViewModel Cue4ParseVM => ViewModelRegistry.Get<Cue4ParseViewModel>();
        public static readonly DirectoryInfo DataFolder = new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".data"));
     

        public override void Initialize()
        {
            
            AppUtils.AppSettings.Load();
            ViewModelRegistry.Register<Cue4ParseViewModel>();
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(theme: AnsiConsoleTheme.Literate)
                .CreateLogger();
            AvaloniaXamlLoader.Load(this);
            AllocConsole();

        }
        
        
        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Line below is needed to remove Avalonia data validation.
                // Without this line you will get duplicate validations from both Avalonia and CT
                BindingPlugins.DataValidators.RemoveAt(0);
                AppVM = new AppViewModel();
                desktop.MainWindow = new AppWindow();

            }
            

            base.OnFrameworkInitializationCompleted();
        }
        private void OnExit(object sender, ControlledApplicationLifetimeExitEventArgs e)
        {
            AppUtils.AppSettings.Save();
        }

    }
}