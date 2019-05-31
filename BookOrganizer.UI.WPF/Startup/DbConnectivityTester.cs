using BookOrganizer.UI.WPF.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

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

        public async Task DbConnectivityCheck()
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
                await metroDialogService.ShowInfoDialogAsync(ex.Message);
            }

            connection.Dispose();
        }

    }
}