using Autofac;
using BookOrganizer.Data.SqlServer;
using BookOrganizer.UI.WPF.ViewModels;

namespace BookOrganizer.UI.WPF.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();

            builder.RegisterType<BookOrganizerDbContext>().AsSelf();

            return builder.Build();
        }
    }
}
