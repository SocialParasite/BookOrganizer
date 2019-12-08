using Microsoft.Extensions.Configuration;

namespace BookOrganizer.Data.SqlServer
{
    public static class ConnectivityService
    {
        public static string GetConnectionString(string database = "DEV")
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("connectionString.json");

            return builder.Build().GetConnectionString(database);
        }
    }
}
