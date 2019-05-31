using Autofac;
using BookOrganizer.UI.WPF.Services;
using BookOrganizer.UI.WPF.Startup;
using System.Windows;

namespace BookOrganizer.UI.WPF
{
    public partial class App : Application
    {
        internal static IContainer container { get; set; }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            var bootstrapper = new Bootstrapper();

            container = bootstrapper.Bootstrap();

            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();

            var dbConnectivity = new DbConnectivityTester(container.Resolve<IMetroDialogService>());
            await dbConnectivity.DbConnectivityCheck();
        }

    }
}
