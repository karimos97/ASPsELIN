using System.Threading;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GroupPoster.ApplicationLayer.Common.Interfaces;
using GroupPoster.Infrastructure.DataAccess;
using GroupPoster.Infrastructure.DataAccess.DataInteractor;
using UniversalPoster.ViewModels;
using UniversalPoster.Views;

namespace UniversalPoster
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                ISystemPersistence persistence = new SystemPersistence(new FileInteractor(new SemaphoreSlim(1, 1)));
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(persistence),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
