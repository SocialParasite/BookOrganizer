using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Events;
using BookOrganizer.Data.Lookups;
using BookOrganizer.DA;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public abstract class BaseViewModel<T> : ViewModelBase, ISelectedViewModel
            where T : class, IIdentifiable
    {
        private List<LookupItem> entityCollection;
        public readonly IEventAggregator eventAggregator;

        public BaseViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            AddNewItemCommand = new DelegateCommand<Type>(OnAddNewItemExecute);

            ItemNameLabelMouseLeftButtonUpCommand =
                new DelegateCommand<LookupItem>(OnItemNameLabelMouseLeftButtonUpExecute,
                                           OnItemNameLabelMouseLeftButtonUpCanExecute);
        }

        public IEnumerable<LookupItem> Items;
        public IRepository<T> Repository;
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
    }
}
