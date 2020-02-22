using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using BookOrganizer.Domain;
using BookOrganizer.Domain.Services;
using BookOrganizer.UI.WPFCore.Events;
using BookOrganizer.UI.WPFCore.Wrappers;
using Prism.Commands;
using Prism.Events;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public abstract class BaseDetailViewModel<T, TBase> : ViewModelBase, IDetailViewModel
            where TBase: BaseWrapper<T>
            where T : class, IIdentifiable
    {
        protected readonly IEventAggregator eventAggregator;

        private Tuple<bool, DetailViewState, SolidColorBrush, bool> userMode;
        private bool hasChanges;
        private Guid selectedBookId;
        private string tabTitle;

        public BaseDetailViewModel(IEventAggregator eventAggregator, IRepository<T> repository)
        {
            this.eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));            
            Repository = repository;

            SwitchEditableStateCommand = new DelegateCommand(SwitchEditableStateExecute);
            SaveItemCommand = new DelegateCommand(SaveItemExecute, SaveItemCanExecute);
            DeleteItemCommand = new DelegateCommand(DeleteItemExecute);
            CloseDetailViewCommand = new DelegateCommand(CloseDetailViewExecute);
            ShowSelectedBookCommand = new DelegateCommand<Guid?>(OnShowSelectedBookExecute, OnShowSelectedBookCanExecute);

            UserMode = (true, DetailViewState.ViewMode, Brushes.LightGray, false).ToTuple();
        }

        public ICommand SwitchEditableStateCommand { get; }
        public ICommand SaveItemCommand { get; }
        public ICommand DeleteItemCommand { get; }
        public ICommand CloseDetailViewCommand { get; }
        public ICommand ShowSelectedBookCommand { get; }

        public abstract TBase SelectedItem { get; set; }
        
        public Tuple<bool, DetailViewState, SolidColorBrush, bool> UserMode
        {
            get => userMode;
            set 
            { 
                userMode = value; 
                OnPropertyChanged(); 
            }
        }

        public bool HasChanges
        {
            get => hasChanges;
            set
            {
                if (hasChanges != value)
                {
                    hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveItemCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public virtual string TabTitle
        {
            get
            {
                if (tabTitle is null) 
                    return "";
                if (tabTitle.Length <= 50)
                    return tabTitle;
                else
                    return tabTitle.Substring(0, 50) + "...";
            }
            set 
            { 
                tabTitle = value; 
                OnPropertyChanged(); 
            }
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

        private Guid id;
        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        public IRepository<T> Repository { get; set; }

        protected void SetChangeTracker()
        {
            if (!HasChanges)
            {
                HasChanges = Repository.HasChanges();
            }
        }

        public abstract Task LoadAsync(Guid id);
        public abstract TBase CreateWrapper(T entity);

        public virtual async Task OnOpenItemDetailsViewAsync(Guid id)
        {
            var entity = await Repository.GetSelectedAsync(id);
            SelectedItem = CreateWrapper(entity);
        }


        public virtual void SwitchEditableStateExecute()
        {
            if (UserMode.Item2 == DetailViewState.ViewMode)
                UserMode = (!UserMode.Item1, DetailViewState.EditMode, Brushes.LightGreen, !UserMode.Item4).ToTuple();
            else
                UserMode = (!UserMode.Item1, DetailViewState.ViewMode, Brushes.LightGray, !UserMode.Item4).ToTuple();
        }

        private async void CloseDetailViewExecute()
        {
            if (Repository.HasChanges())
            {
                //var result = await metroDialogService
                //    .ShowOkCancelDialogAsync(
                //    "You have made changes. Closing will loose all unsaved changes. Are you sure you still want to close this view?",
                //    "Close the view?");

                //if (result == MessageDialogResult.Canceled)
                //{
                //    return;
                //}
            }

            eventAggregator.GetEvent<CloseDetailsViewEvent>()
                .Publish(new CloseDetailsViewEventArgs
                {
                    Id = SelectedItem.Model.Id,
                    ViewModelName = this.GetType().Name
                });
        }

        private bool OnShowSelectedBookCanExecute(Guid? id)
            => (id is null || id == Guid.Empty) ? false : true;

        public virtual void OnShowSelectedBookExecute(Guid? id)
            => SelectedBookId = (Guid)id;

        private bool SaveItemCanExecute()
            => true; // (!HasErrors) && (HasChanges || SelectedItem.Id == default);

        private async void SaveItemExecute()
        {
            if (this.Repository.HasChanges() || SelectedItem.Model.Id == default)
            {
                var isNewItem = false;

                //var resultWhenChanges = await metroDialogService
                //    .ShowOkCancelDialogAsync(
                //    "You are about to save your changes. This will overwrite the previous version. Are you sure?",
                //    "Save changes?");

                //if (resultWhenChanges == MessageDialogResult.Canceled)
                //{
                //    return;
                //}

                if (SelectedItem.Model.Id == default)
                {
                    isNewItem = true;
                }

                Repository.Update(SelectedItem.Model);
                await SaveRepository();

                if (isNewItem)
                {
                    eventAggregator.GetEvent<SavedDetailsViewEvent>()
                        .Publish(new OpenDetailViewEventArgs
                        {
                            Id = SelectedItem.Model.Id,
                            ViewModelName = this.GetType().Name
                        });
                }

                HasChanges = false;
            }
            else
            {
                //var unmodifiedResult = metroDialogService.ShowInfoDialogAsync("You have no unsaved changes on this view.");
            }

        }

        private async void DeleteItemExecute()
        {
            //var result = await metroDialogService
            //    .ShowOkCancelDialogAsync(
            //    "You are about to delete an item. This operation cannot be undone. Are you sure?",
            //    "Delete an item?");

            //if (result == MessageDialogResult.Canceled)
            //{
            //    return;
            //}
            //else
            {
                Repository.Delete(SelectedItem.Model);
                await SaveRepository();
            }
        }

        private async Task SaveRepository()
            => await Repository.SaveAsync();
    }
}
