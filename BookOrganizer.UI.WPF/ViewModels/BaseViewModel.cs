using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Events;
using BookOrganizer.UI.WPF.Lookups;
using BookOrganizer.UI.WPF.Repositories;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public abstract class BaseViewModel<T> : ViewModelBase
            where T : class, IIdentifiable
    {
        public BaseViewModel()
        {
            //RefreshCommand = new DelegateCommand(OnRefreshExecute);
            //SortByCommand = new DelegateCommand<object>(OnSortByExecute);
        }

        public IEnumerable<LookupItem> Items;

        public IRepository<T> Repository;

        //public ICommand RefreshCommand { get; }
        //public ICommand SortByCommand { get; }

        private List<LookupItem> entityCollection;
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
