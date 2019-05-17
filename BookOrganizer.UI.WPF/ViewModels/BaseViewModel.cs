using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Events;
using BookOrganizer.UI.WPF.Lookups;
using BookOrganizer.UI.WPF.Repositories;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public abstract class BaseViewModel<T> : ViewModelBase
            where T : class, IIdentifiable
    {
        private List<LookupItem> entityCollection;

        public BaseViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            //RefreshCommand = new DelegateCommand(OnRefreshExecute);
            //SortByCommand = new DelegateCommand<object>(OnSortByExecute);

            AddNewItemCommand = new DelegateCommand<Type>(OnAddNewItemExecute);

            ItemNameLabelMouseLeftButtonUpCommand =
                new DelegateCommand<LookupItem>(OnItemNameLabelMouseLeftButtonUpExecute,
                                           OnItemNameLabelMouseLeftButtonUpCanExecute);
        }

        public readonly IEventAggregator eventAggregator;

        public IEnumerable<LookupItem> Items;

        public IRepository<T> Repository;

        //public ICommand RefreshCommand { get; }
        //public ICommand SortByCommand { get; }
        public ICommand AddNewItemCommand { get; }
        public ICommand ItemNameLabelMouseLeftButtonUpCommand { get; }

        public List<LookupItem> EntityCollection
        {
            get => entityCollection;
            set
            {
                entityCollection = value;
                OnPropertyChanged();
            }
        }

        public abstract Task InitializeRepositoryAsync();

        private void OnAddNewItemExecute(Type itemType)
        {
            eventAggregator.GetEvent<OpenDetailViewEvent>()
                       .Publish(new OpenDetailViewEventArgs
                       {
                           Id = new Guid(),
                           ViewModelName = Type.GetType(itemType.FullName).Name
                       });
        }

        private bool OnItemNameLabelMouseLeftButtonUpCanExecute(LookupItem item)
            => (item.Id == Guid.Empty) ? false : true;

        private void OnItemNameLabelMouseLeftButtonUpExecute(LookupItem item)
        {
            eventAggregator.GetEvent<OpenDetailViewEvent>()
                                   .Publish(new OpenDetailViewEventArgs
                                   {
                                       Id = item.Id,
                                       ViewModelName = item.ViewModelName
                                   });
        }


        //public abstract void SortEntityCollection(IEnumerable<T> itemList, string orderBy);

        //public virtual void OnSortByExecute(object selectedValue)
        //{
        //    if (selectedValue.ToString() == "A-Z")
        //        SortEntityCollection(items, "asc");
        //    else
        //        SortEntityCollection(items, "desc");
        //}

        //private async void OnRefreshExecute()
        //    => await InitializeRepositoryAsync();
    }
}
