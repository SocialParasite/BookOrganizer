using Autofac;
using BookOrganizer.UI.WPF.Services;
using BookOrganizer.UI.WPF.Startup;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;

namespace BookOrganizer.UI.WPF
{
    public partial class App : Application
    {
        internal static IContainer container { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var bootstrapper = new Bootstrapper();

            container = bootstrapper.Bootstrap();

            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();

            var test = new DbConnectivityTester(container.Resolve<IMetroDialogService>());
            test.DbConnectivityCheck();
        }

    }
}

public class DbConnectivityTester
{
    private readonly IMetroDialogService metroDialogService;
    private string connectionString;
    public static IConfigurationRoot Configuration { get; private set; }

    public DbConnectivityTester(IMetroDialogService metroDialogService, string connString = null)
    {
        this.metroDialogService = metroDialogService;
        connectionString = connString;

        if (connectionString is null)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("connectionString.json");

            Configuration = builder.Build();

            connectionString = Configuration.GetConnectionString("BookOrganizerDbDEV");
        }
    }

    public async Task DbConnectivityCheck()
    {
        SqlConnection connection = new SqlConnection();
        connection.ConnectionString = connectionString;

        try
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
            //Everything ok -> continue normally
        }
        catch (Exception ex)
        {
            await metroDialogService.ShowInfoDialogAsync(ex.Message);
        }

        connection.Dispose();
    }

}
