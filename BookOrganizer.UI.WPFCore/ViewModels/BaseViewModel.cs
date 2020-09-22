using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BookOrganizer.Domain;
using BookOrganizer.UI.WPFCore.DialogServiceManager;
using BookOrganizer.UI.WPFCore.Events;
using BookOrganizer.UI.WPFCore.Extensions;
using Prism.Commands;
using Prism.Events;
using Serilog;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public abstract class BaseViewModel<T> : ViewModelBase, IItemLists
            where T : class, IIdentifiable
    {
        private List<LookupItem> entityCollection;
        public readonly IEventAggregator eventAggregator;
        protected readonly ILogger logger;
        protected readonly IDialogService dialogService;

        public BaseViewModel(IEventAggregator eventAggregator,
                             ILogger logger,
                             IDialogService dialogService)
        {
            this.eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));

            AddNewItemCommand = new DelegateCommand<string>(OnAddNewItemExecute);

            ItemNameLabelMouseLeftButtonUpCommand =
                new DelegateCommand<LookupItem>(OnItemNameLabelMouseLeftButtonUpExecute,
                                           OnItemNameLabelMouseLeftButtonUpCanExecute);
        }

        public IEnumerable<LookupItem> Items;
        public IRepository<T> Repository;
        public ICommand AddNewItemCommand { get; }
        public ICommand ItemNameLabelMouseLeftButtonUpCommand { get; }

        public string ViewModelType { get; set; }

        public List<LookupItem> EntityCollection
        {
            get => entityCollection;
            set
            {
                entityCollection = value;
                FilteredEntityCollection = entityCollection.FromListToList();
                OnPropertyChanged();
            }
        }

        private List<LookupItem> filteredEntityCollection;

        public List<LookupItem> FilteredEntityCollection
        {
            get => filteredEntityCollection;
            set
            {
                filteredEntityCollection = value;
                OnPropertyChanged();
            }
        }

        private string searchString;

        public string SearchString
        {
            get => searchString;
            set
            {
                searchString = value;
                OnPropertyChanged();
                UpdateFilteredEntityCollection();
            }
        }

        private void UpdateFilteredEntityCollection()
        {
            FilteredEntityCollection?.Clear();
            FilteredEntityCollection = EntityCollection?.Where(w => w.DisplayMember
                                                       .IndexOf(SearchString, StringComparison.OrdinalIgnoreCase) != -1)
                                                       .ToList();
        }

        public abstract Task InitializeRepositoryAsync();

        private void OnAddNewItemExecute(string itemType)
        {
            eventAggregator.GetEvent<OpenDetailViewEvent>()
                       .Publish(new OpenDetailViewEventArgs
                       {
                           Id = new Guid(),
                           ViewModelName = itemType
                       });
        }

        private bool OnItemNameLabelMouseLeftButtonUpCanExecute(LookupItem item)
            => (item.Id != Guid.Empty);

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
