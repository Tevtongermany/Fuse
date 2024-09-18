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
        public static readonly DirectoryInfo DataFolder = new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".data"));
     
        public App()
        {
            
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            
        }
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
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

    }
}