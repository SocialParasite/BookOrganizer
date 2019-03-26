using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Repositories;
using BookOrganizer.UI.WPF.Services;
using Prism.Events;
using System;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class PublisherDetailViewModel : BaseDetailViewModel<Publisher>, IPublisherDetailViewModel
    {
        private string name;

        public PublisherDetailViewModel(IEventAggregator eventAggregator,
            IMetroDialogService metroDialogService,
            IRepository<Publisher> publisherRepo)
            : base(eventAggregator, metroDialogService)
        {
            Repository = publisherRepo ?? throw new ArgumentNullException(nameof(publisherRepo));

            SelectedItem = new Publisher();
        }

        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); TabTitle = value; SelectedItem.Name = value; }
        }

        public async override Task LoadAsync(Guid id)
        {
            SelectedItem = await Repository.GetSelectedAsync(id) ?? null;

            Id = id;

            if (Id != Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                TabTitle = SelectedItem.Name;
                Name = SelectedItem.Name;
            }
            //SetDefaultPublisherLogoIfNoneSet();

            //void SetDefaultPublisherLogoIfNoneSet()
            //{
            //    if (SelectedItem.BookCoverPicture is null)
            //        SelectedItem.BookCoverPicture =
            //            $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6)}\\placeholder.png";
            //}
        }
    }
}
