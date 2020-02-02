using System.Windows;
using Autofac;
using BookOrganizer.UI.WPFCore.Startup;

namespace BookOrganizer.UI.WPFCore
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static IContainer Container { get; set; }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            var bootstrapper = new Bootstrapper();

            Container = bootstrapper.Bootstrap();

            var mainWindow = Container.Resolve<MainWindow>();
            mainWindow.Show();

            //var dbConnectivity = new DbConnectivityTester(container.Resolve<IMetroDialogService>(),
            //                                              container.Resolve<BookOrganizerDbContext>().connectionString);
            //await dbConnectivity.DbConnectivityCheckAsync();
        }
    }
}
