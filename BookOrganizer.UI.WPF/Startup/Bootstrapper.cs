using Autofac;
using BookOrganizer.Data.SqlServer;
using BookOrganizer.UI.WPF.Lookups;
using BookOrganizer.UI.WPF.Repositories;
using BookOrganizer.UI.WPF.Services;
using BookOrganizer.UI.WPF.ViewModels;
using BookOrganizer.UI.WPF.Views;
using Prism.Events;

namespace BookOrganizer.UI.WPF.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();

            builder.RegisterType<BookDetailViewModel>().As<IBookDetailViewModel>();
            builder.RegisterType<BookDetailViewModel>()
                .Keyed<IDetailViewModel>(nameof(BookDetailViewModel));

            builder.RegisterType<BooksViewModel>().As<IBooksViewModel>();
            builder.RegisterType<BooksViewModel>().Keyed<ISelectedViewModel>(nameof(BooksViewModel));

            builder.RegisterType<BooksView>().AsSelf();

            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();

            builder.RegisterType<BooksRepository>().AsImplementedInterfaces();

            builder.RegisterType<BookOrganizerDbContext>().AsSelf();

            builder.RegisterType<MetroDialogService>().As<IMetroDialogService>();

            return builder.Build();
        }
    }
}
