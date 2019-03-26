using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Events;
using BookOrganizer.UI.WPF.Repositories;
using BookOrganizer.UI.WPF.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class PublisherDetailViewModel : BaseDetailViewModel<Publisher>, IPublisherDetailViewModel
    {
        private string name;
        private Guid selectedBookId;

        public PublisherDetailViewModel(IEventAggregator eventAggregator,
            IMetroDialogService metroDialogService,
            IRepository<Publisher> publisherRepo)
            : base(eventAggregator, metroDialogService)
        {
            Repository = publisherRepo ?? throw new ArgumentNullException(nameof(publisherRepo));

            ShowSelectedBookCommand = new DelegateCommand<Guid?>(OnShowSelectedBookExecute, OnShowSelectedBookCanExecute);
            SelectedItem = new Publisher();

        }

        private bool OnShowSelectedBookCanExecute(Guid? id)
            => (id is null || id == Guid.Empty) ? false : true;

        private void OnShowSelectedBookExecute(Guid? id)
            => SelectedBookId = (Guid)id;

        public ICommand ShowSelectedBookCommand { get; set; }

        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); TabTitle = value; SelectedItem.Name = value; }
        }

        public Guid SelectedBookId
        {
            get => selectedBookId;
            set
            {
                selectedBookId = value;
                OnPropertyChanged();
                if (selectedBookId != Guid.Empty)
                {
                    eventAggregator.GetEvent<OpenItemMatchingSelectedBookIdEvent<Guid>>()
                                   .Publish(selectedBookId);
                }
            }
        }

        public async override Task LoadAsync(Guid id)
        {
            SelectedItem = await Repository.GetSelectedAsync(id) ?? null;

            Id = id;

            TabTitle = SelectedItem.Name;
            Name = SelectedItem.Name;
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
