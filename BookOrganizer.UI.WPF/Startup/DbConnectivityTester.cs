using BookOrganizer.Data.SqlServer;
using BookOrganizer.UI.WPF.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;

namespace BookOrganizer.UI.WPF.Startup
{
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

        public async Task DbConnectivityCheckAsync()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;

            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();

                connection.Open();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cannot open database"))
                {
                    await metroDialogService
                        .ShowInfoDialogAsync($"Book Organizer database couldn't be opened. " +
                        $"This might be because the database is down or it doesn't exist. \r\rError message(s):\r{ex.Message}");

                    if(await metroDialogService.ShowOkCancelDialogAsync("Would you like to create a database now?", "Create database?")
                        == MahApps.Metro.Controls.Dialogs.MessageDialogResult.Affirmative)
                    {
                        await CreateDatabase();
                    }
                }
                else
                    await metroDialogService.ShowInfoDialogAsync(ex.Message);
            }

            connection.Dispose();
        }

        private async Task CreateDatabase()
        {
            var context = new BookOrganizerDbContext();
            await context.Database.EnsureCreatedAsync();
            Application.Current.MainWindow.Close();
        }

    }
}