using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using FGame.WindowsApp.Views;

namespace FGame.WindowsApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public sealed partial class App : PrismApplication
    {
        // TODO: try Elmish.WPF library to simplify WPF logic.
        public App()
        {
            // Set current culture for app globally.
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(CultureInfo.InvariantCulture.Name)
                )
            );

            // TODO: replace next line with logger call.
            Debug.WriteLine("Desktop client application started.");
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // TODO: register common domain and models classes here.
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            // TODO: split desktop project on separate modules.
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            // TODO: replace next line with logger call.
            Debug.WriteLine("Desktop client application stopped.");
        }
    }
}
