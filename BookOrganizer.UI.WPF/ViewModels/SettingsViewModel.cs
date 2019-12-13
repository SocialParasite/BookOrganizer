using BookOrganizer.Data.SqlServer;
using BookOrganizer.UI.WPF.Enums;
using BookOrganizer.UI.WPF.Startup;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class SettingsViewModel : ViewModelBase, ISelectedViewModel
    {
        public SettingsViewModel()
        {
            ApplyAndSaveSettingsCommand = new DelegateCommand(OnApplyAndSaveExecute);
            RemoveConnectionStringCommand = new DelegateCommand<string>(OnRemoveConnectionStringExecute);

            if(Databases is null) 
                Databases = new ObservableCollection<ConnectionString>();

            InitializeRepositoryAsync();
        }

        private string storagePath;
        private FileAction fileActionMode;
        
        Settings settings;

        public ICommand ApplyAndSaveSettingsCommand { get; }
        public ICommand RemoveConnectionStringCommand { get; }

        public ObservableCollection<ConnectionString> Databases { get; set; }

        public string StoragePath
        {
            get { return storagePath; }
            set { storagePath = value; OnPropertyChanged(); }
        }

        public FileAction FileActionMode
        {
            get { return fileActionMode; }
            set { fileActionMode = value; OnPropertyChanged(); }
        }

        private async Task InitializeRepositoryAsync()
        {
            ReadSettings();

            ReadAndConvertConnectionStrings();
        }
        
        private void ReadSettings()
        {
            JObject jobj;

            if (File.Exists(@"Startup\settings.json"))
            {
                string settingsFile = File.ReadAllText(@"Startup\settings.json");

                jobj = (JObject)JsonConvert.DeserializeObject(settingsFile);
                settings = jobj.ToObject<Settings>();
            }
        }

        private void ReadAndConvertConnectionStrings()
        {
            IConfigurationBuilder builder2 = new ConfigurationBuilder().AddJsonFile("connectionString.json");
            IConfiguration connectionStringConfiguration = builder2.Build(); 

            Databases.Clear();
            IConfigurationSection loop = connectionStringConfiguration.GetSection("ConnectionStrings");

            foreach (var item in loop.GetChildren())
            {
                string neConnectionString = builder2.Build().GetConnectionString(item.Key);
                SqlConnectionStringBuilder decoder = new SqlConnectionStringBuilder(neConnectionString);

                ConnectionString tempConnectionString = new ConnectionString();

                if (item.Key != null && item.Key == settings.StartupDatabase)
                {
                    tempConnectionString.Default = true;
                }
                tempConnectionString.Database = decoder.InitialCatalog;
                tempConnectionString.Identifier = item.Key;
                tempConnectionString.Server = decoder.DataSource;
                tempConnectionString.Trusted_Connection = decoder.IntegratedSecurity;

                Databases.Add(tempConnectionString);
            }
        }

        private void OnApplyAndSaveExecute()
        {
            SaveSettingsJson();
            SaveConnectionStrings();
        }

        private void SaveConnectionStrings()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine("  \"ConnectionStrings\": {");

            foreach (var db in Databases)
            {
                if (db.Identifier is null || db.Server is null || db.Database is null)
                {
                    continue;
                }

                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder["Server"] = db.Server;
                builder["Trusted_Connection"] = db.Trusted_Connection;
                builder["Database"] = db.Database;

                stringBuilder.AppendLine($"    \"{db.Identifier}\": \"{builder}\",");
            }

            stringBuilder.AppendLine("  }");
            stringBuilder.AppendLine("}"); 
            stringBuilder.Replace(@"\", @"\\");

            File.WriteAllText("connectionString.json", stringBuilder.ToString());
        }

        private void SaveSettingsJson()
        {
            settings.StartupDatabase = Databases.Where(d => d.Default == true).Select(d => d.Identifier).FirstOrDefault();

            dynamic jsonSettings = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(@"Startup\settings.json", jsonSettings);
        }

        private void OnRemoveConnectionStringExecute(string id)
        {
            if (id != null || id != Databases.LastOrDefault().Identifier)
            {
                Databases.Remove(Databases.First(i => i.Identifier == id));
            }
        }

    }
}
