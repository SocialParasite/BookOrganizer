using Autofac;
using BookOrganizer.DA;
using BookOrganizer.Data.SqlServer;
using BookOrganizer.UI.WPF.Services;
using BookOrganizer.UI.WPF.ViewModels;
using Prism.Events;

namespace BookOrganizer.UI.WPF.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterType<MetroDialogService>().As<IMetroDialogService>();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();

            builder.RegisterAssemblyTypes(typeof(MainPageViewModel).Assembly)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .Keyed<ISelectedViewModel>(c => c.Name);

            builder.RegisterAssemblyTypes(typeof(BookDetailViewModel).Assembly)
                .Where(type => type.Name.EndsWith("DetailViewModel"))
                .Keyed<IDetailViewModel>(c => c.Name);

            builder.RegisterType<LookupDataService>().AsImplementedInterfaces()
                .WithParameter("imagePath", FileExplorerService.GetImagePath());

            builder.RegisterAssemblyTypes(typeof(BooksRepository).Assembly)
                .Where(type => type.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();

            builder.RegisterType<BookOrganizerDbContext>().AsSelf();

            return builder.Build();
        }
    }
}
