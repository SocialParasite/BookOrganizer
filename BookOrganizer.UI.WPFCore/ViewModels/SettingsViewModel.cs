using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using BookOrganizer.Data.SqlServer;
using BookOrganizer.UI.WPFCore.Events;
using BookOrganizer.UI.WPFCore.Startup;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Events;
using System;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public class SettingsViewModel : ViewModelBase, ISelectedViewModel
    {
        private readonly IEventAggregator eventAggregator;

        private FileAction fileActionMode;

        private Settings settings;
        private Settings unmodifiedSettings;

        public SettingsViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));

            ApplyAndSaveSettingsCommand = new DelegateCommand(OnApplyAndSaveExecute, OnApplyAndSaveCanExecute)
                .ObservesProperty(() => HasChanges);
            RemoveConnectionStringCommand = new DelegateCommand<string>(OnRemoveConnectionStringExecute);

            if (Databases is null)
                Databases = new ObservableCollection<ConnectionString>();

            InitializeRepository();

            HasChanges = true;
        }

        public ICommand ApplyAndSaveSettingsCommand { get; }
        public ICommand RemoveConnectionStringCommand { get; }

        public ObservableCollection<ConnectionString> Databases { get; set; }

        private bool hasChanges;

        public bool HasChanges
        {
            get { return hasChanges; }
            set { hasChanges = value; OnPropertyChanged(); }
        }

        public string StoragePath
        {
            get { return settings.StoragePath; }
            set { settings.StoragePath = value; OnPropertyChanged(); }
        }

        public FileAction FileActionMode
        {
            get { return fileActionMode; }
            set { fileActionMode = value; OnPropertyChanged(); }
        }

        public string LogFilePath
        {
            get { return settings.LogFilePath; }
            set { settings.LogFilePath = value; OnPropertyChanged(); }
        }

        public string LogServerUrl
        {
            get { return settings.LogServerUrl; }
            set { settings.LogServerUrl = value; OnPropertyChanged(); }
        }

        private void InitializeRepository()
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
                unmodifiedSettings = jobj.ToObject<Settings>();
            }
        }

        private void ReadAndConvertConnectionStrings()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("connectionString.json");
            IConfiguration connectionStringConfiguration = builder.Build();

            Databases.Clear();
            IConfigurationSection loop = connectionStringConfiguration.GetSection("ConnectionStrings");

            foreach (var item in loop.GetChildren())
            {
                string newConnectionString = builder.Build().GetConnectionString(item.Key);
                SqlConnectionStringBuilder decoder = new SqlConnectionStringBuilder(newConnectionString);

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

        private bool OnApplyAndSaveCanExecute()
        {
            return HasChanges;
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

            eventAggregator.GetEvent<ChangeDetailsViewEvent>()
                    .Publish(new ChangeDetailsViewEventArgs
                    {
                        Message = CreateChangeMessage(DatabaseOperation.SETTINGS),
                        MessageBackgroundColor = Brushes.Green
                    });
        }

        private void OnRemoveConnectionStringExecute(string id)
        {
            if (id != null || id != Databases.LastOrDefault().Identifier)
            {
                Databases.Remove(Databases.First(i => i.Identifier == id));

                eventAggregator.GetEvent<ChangeDetailsViewEvent>()
                    .Publish(new ChangeDetailsViewEventArgs
                    {
                        Message = CreateChangeMessage(DatabaseOperation.DATABASE_CONNECTIONS),
                        MessageBackgroundColor = Brushes.Red
                    });
            }
        }

        private string CreateChangeMessage(DatabaseOperation operation)
            => $"{operation} modified.";
    }
}
