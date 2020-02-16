﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BookOrganizer.DA;
using BookOrganizer.Domain;
using BookOrganizer.UI.WPFCore.Events;
using BookOrganizer.UI.WPFCore.Extensions;
using Prism.Commands;
using Prism.Events;
using Serilog;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public abstract class BaseViewModel<T> : ViewModelBase, ISelectedViewModel
            where T : class, IIdentifiable
    {
        private List<LookupItem> entityCollection;
        public readonly IEventAggregator eventAggregator;
        protected readonly ILogger logger;
        
        public BaseViewModel(IEventAggregator eventAggregator,
                             ILogger logger)
        {
            this.eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

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
            get { return searchString; }
            set 
            { 
                searchString = value; 
                OnPropertyChanged();
                UpdateFilteredEntityCollection();
            }
        }

        private void UpdateFilteredEntityCollection()
        {
            FilteredEntityCollection.Clear();
            FilteredEntityCollection = EntityCollection.Where(w => w.DisplayMember
                                                       .IndexOf(SearchString, StringComparison.OrdinalIgnoreCase) != -1)
                                                       .ToList();
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