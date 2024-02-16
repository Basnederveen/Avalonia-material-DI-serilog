using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using AvaloniaApp.ViewModels;
using AvaloniaApp.Views;

namespace AvaloniaApp
{
    public partial class App : Application
    {
        private readonly IServiceProvider serviceProvider;

        public App(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public App()
        {

        }

        public override void Initialize()
        {
            Resources[typeof(IServiceProvider)] = serviceProvider;

            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Line below is needed to remove Avalonia data validation.
                // Without this line you will get duplicate validations from both Avalonia and CT
                BindingPlugins.DataValidators.RemoveAt(0);
                desktop.MainWindow = new MainWindow
                {
                    DataContext = serviceProvider.GetService(typeof(MainViewModel))
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}