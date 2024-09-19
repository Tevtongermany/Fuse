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

namespace Fuse
{
    public partial class App : Application
    {

        public static AppViewModel AppVM = null!;
        public static MainWindowViewModel MainVM => ViewModelRegistry.Get<MainWindowViewModel>();
        public static Cue4ParseViewModel Cue4ParseVM => ViewModelRegistry.Get<Cue4ParseViewModel>();
        public static readonly DirectoryInfo DataFolder = new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".data"));
     

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            AppUtils.AppSettings.Load();
            ViewModelRegistry.Register<Cue4ParseViewModel>();

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