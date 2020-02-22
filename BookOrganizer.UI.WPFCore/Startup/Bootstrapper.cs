using Autofac;
using BookOrganizer.DA;
using BookOrganizer.Data.DA;
using BookOrganizer.Data.SqlServer;
using BookOrganizer.Domain;
using BookOrganizer.Domain.Services;
using BookOrganizer.UI.WPFCore.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Events;
using Serilog;
using System.IO;

namespace BookOrganizer.UI.WPFCore.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();

            builder.RegisterType<AuthorService>().AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(AuthorsViewModel).Assembly)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .Keyed<ISelectedViewModel>(c => c.Name);

            builder.RegisterAssemblyTypes(typeof(AuthorDetailViewModel).Assembly)
                .Where(type => type.Name.EndsWith("DetailViewModel"))
                .Keyed<IDetailViewModel>(c => c.Name);

            builder.RegisterType<LookupDataService>().AsImplementedInterfaces()
                .WithParameter("imagePath", ""); // FileExplorerService.GetImagePath());
            
            builder.RegisterType<BookStatisticsLookupDataService>().AsSelf();

            builder.RegisterAssemblyTypes(typeof(BooksRepository).Assembly)
                .Where(type => type.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();

            builder.Register<ILogger>((_)
                => new LoggerConfiguration()
                    .WriteTo.File("Log-{Date}.txt", rollingInterval: RollingInterval.Day)
                    .WriteTo.Seq("http://localhost:5341/")
                    .CreateLogger())
                .SingleInstance();

            //var startupDb = GetStartupDatabase();
            //var connectionString = ConnectivityService.GetConnectionString(startupDb);

            //builder.RegisterType<BookOrganizerDbContext>().AsSelf().WithParameter("connectionString", connectionString);
            builder.RegisterType<BookOrganizerDbContext>().AsSelf();

            return builder.Build();
        }

        //string GetStartupDatabase()
        //{
        //    string connectionString = null;

        //    if (File.Exists(@"Startup\settings.json"))
        //    {
        //        var settingsFile = File.ReadAllText(@"Startup\settings.json");

        //        JObject jobj = (JObject)JsonConvert.DeserializeObject(settingsFile);
        //        Settings settings = jobj.ToObject<Settings>();
        //        connectionString = settings.StartupDatabase;
        //    }

        //    return connectionString;
        //}
    }
}
