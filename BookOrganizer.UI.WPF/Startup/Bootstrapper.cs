using Autofac;
using BookOrganizer.DA;
using BookOrganizer.Data.SqlServer;
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
            builder.RegisterType<MetroDialogService>().As<IMetroDialogService>();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();

            builder.RegisterType<MainPageViewModel>().Keyed<ISelectedViewModel>(nameof(MainPageViewModel));

            builder.RegisterType<BookDetailViewModel>()
                .Keyed<IDetailViewModel>(nameof(BookDetailViewModel));

            builder.RegisterType<BooksViewModel>().Keyed<ISelectedViewModel>(nameof(BooksViewModel));

            builder.RegisterType<SeriesViewModel>().Keyed<ISelectedViewModel>(nameof(SeriesViewModel));

            builder.RegisterType<SeriesDetailViewModel>()
                .Keyed<IDetailViewModel>(nameof(SeriesDetailViewModel));

            builder.RegisterType<PublishersViewModel>().Keyed<ISelectedViewModel>(nameof(PublishersViewModel));

            builder.RegisterType<PublisherDetailViewModel>()
                .Keyed<IDetailViewModel>(nameof(PublisherDetailViewModel));

            builder.RegisterType<AuthorsViewModel>().Keyed<ISelectedViewModel>(nameof(AuthorsViewModel));

            builder.RegisterType<AuthorDetailViewModel>()
                .Keyed<IDetailViewModel>(nameof(AuthorDetailViewModel));

            builder.RegisterType<LanguageDetailViewModel>().Keyed<IDetailViewModel>(nameof(LanguageDetailViewModel));

            builder.RegisterType<NationalityDetailViewModel>().Keyed<IDetailViewModel>(nameof(NationalityDetailViewModel));

            builder.RegisterType<FormatDetailViewModel>().Keyed<IDetailViewModel>(nameof(FormatDetailViewModel));
            builder.RegisterType<GenreDetailViewModel>().Keyed<IDetailViewModel>(nameof(GenreDetailViewModel));

            builder.RegisterType<BooksView>().AsSelf();

            builder.RegisterType<LookupDataService>().AsImplementedInterfaces()
                .WithParameter("imagePath", FileExplorerService.GetImagePath());

            builder.RegisterType<BooksRepository>().AsImplementedInterfaces();
            builder.RegisterType<PublishersRepository>().AsImplementedInterfaces();
            builder.RegisterType<AuthorsRepository>().AsImplementedInterfaces();
            builder.RegisterType<SeriesRepository>().AsImplementedInterfaces();
            builder.RegisterType<LanguageRepository>().AsImplementedInterfaces();
            builder.RegisterType<NationalityRepository>().AsImplementedInterfaces();
            builder.RegisterType<FormatRepository>().AsImplementedInterfaces();
            builder.RegisterType<GenreRepository>().AsImplementedInterfaces();

            builder.RegisterType<SettingsViewModel>().Keyed<ISelectedViewModel>(nameof(SettingsViewModel));

            builder.RegisterType<BookOrganizerDbContext>().AsSelf();

            return builder.Build();
        }
    }
}
