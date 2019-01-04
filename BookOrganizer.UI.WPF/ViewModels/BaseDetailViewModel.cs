using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Events;
using BookOrganizer.UI.WPF.Repositories;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public abstract class BaseDetailViewModel<T> : ViewModelBase, IDetailViewModel
        where T : class, IIdentifiable
    {
        public BaseDetailViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            //this.eventAggregator.GetEvent<OpenDetailsViewEvent<T>>()
            //        .Subscribe(OnOpenItemDetailsViewAsync);
        }

        private readonly IEventAggregator eventAggregator;

        public IRepository<T> Repository;

        private List<T> itemCollection;
        public List<T> ItemCollection
        {
            get { return itemCollection; }
            set { itemCollection = value; OnPropertyChanged(); }
        }

        private T selectedItem;
        public T SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; OnPropertyChanged(); }
        }

        public virtual async void OnOpenItemDetailsViewAsync(Guid id)
            => SelectedItem = await Repository.GetSelectedAsync(id);

        public abstract Task LoadAsync(Guid id);

        private Guid guid;

        public Guid Id
        {
            get { return guid; }
            set { guid = value; }
        }

    }
}
