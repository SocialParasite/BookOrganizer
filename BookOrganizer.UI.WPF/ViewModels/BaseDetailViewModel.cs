using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Events;
using BookOrganizer.UI.WPF.Repositories;
using BookOrganizer.UI.WPF.Services;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public abstract class BaseDetailViewModel<T> : ViewModelBase, IDetailViewModel
        where T : class, IIdentifiable
    {
        private readonly IMetroDialogService metroDialogService;
        protected readonly IEventAggregator eventAggregator;
        protected IRepository<T> Repository;

        private Guid guid;
        private T selectedItem;
        private Tuple<bool, int, SolidColorBrush, bool> userMode;
        private bool hasChanges;
        private Guid selectedBookId;

        public BaseDetailViewModel(IEventAggregator eventAggregator, IMetroDialogService metroDialogService)
        {
            this.eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            this.metroDialogService = metroDialogService ?? throw new ArgumentNullException(nameof(metroDialogService));

            SwitchEditableStateCommand = new DelegateCommand(SwitchEditableStateExecute);
            SaveItemCommand = new DelegateCommand(SaveItemExecute);
            DeleteItemCommand = new DelegateCommand(DeleteItemExecute);
            CloseDetailViewCommand = new DelegateCommand(CloseDetailViewExecute);
            ShowSelectedBookCommand = new DelegateCommand<Guid?>(OnShowSelectedBookExecute, OnShowSelectedBookCanExecute);

            UserMode = (true, 0, Brushes.LightGray, false).ToTuple();
        }

        public ICommand SwitchEditableStateCommand { get; set; }
        public ICommand SaveItemCommand { get; set; }
        public ICommand DeleteItemCommand { get; set; }
        public ICommand CloseDetailViewCommand { get; set; }
        public ICommand ShowSelectedBookCommand { get; set; }

        public T SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value ?? throw new ArgumentNullException(nameof(SelectedItem));
                OnPropertyChanged();
            }
        }

        public Guid Id
        {
            get { return guid; }
            set { guid = value; }
        }

        public Tuple<bool, int, SolidColorBrush, bool> UserMode
        {
            get => userMode;
            set { userMode = value; OnPropertyChanged(); }
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
                }
            }
        }

        private string tabTitle;

        public string TabTitle
        {
            get { return tabTitle; }
            set { tabTitle = value; OnPropertyChanged(); }
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

        public abstract Task LoadAsync(Guid id);

        public virtual async Task OnOpenItemDetailsViewAsync(Guid id)
            => SelectedItem = await Repository.GetSelectedAsync(id);

        public virtual void SwitchEditableStateExecute()
        {
            if (UserMode.Item2 == 0)
                UserMode = (!UserMode.Item1, 1, Brushes.LightGreen, !UserMode.Item4).ToTuple();
            else
                UserMode = (!UserMode.Item1, 0, Brushes.LightGray, !UserMode.Item4).ToTuple();
        }

        private async void CloseDetailViewExecute()
        {
            if (this.Repository.HasChanges())
            {
                var result = await metroDialogService
                    .ShowOkCancelDialogAsync(
                    "You have made changes. Closing will loose all unsaved changes. Are you sure you still want to close this view?",
                    "Close the view?");

                if (result == MessageDialogResult.Canceled)
                {
                    return;
                }
            }

            eventAggregator.GetEvent<CloseDetailsViewEvent>()
                .Publish(new CloseDetailsViewEventArgs
                {
                    Id = this.Id,
                    ViewModelName = this.GetType().Name
                });
        }

        private bool OnShowSelectedBookCanExecute(Guid? id)
            => (id is null || id == Guid.Empty) ? false : true;

        private void OnShowSelectedBookExecute(Guid? id)
            => SelectedBookId = (Guid)id;

        private async void SaveItemExecute()
        {
            if (this.Repository.HasChanges())
            {
                var resultWhenChanges = await metroDialogService
                    .ShowOkCancelDialogAsync(
                    "You are about to save your changes. This will overwrite the previous version. Are you sure?",
                    "Save changes?");

                if (resultWhenChanges == MessageDialogResult.Canceled)
                {
                    return;
                }

                Repository.Update(SelectedItem);
                await SaveRepository();
            }
            else
            {
                var unmodifiedResult = metroDialogService.ShowInfoDialogAsync("You have no unsaved changes on this view.");
            }
        }

        private async void DeleteItemExecute()
        {
            var result = await metroDialogService
                .ShowOkCancelDialogAsync(
                "You are about to delete an item. This operation cannot be undone. Are you sure?",
                "Delete an item?");

            if (result == MessageDialogResult.Canceled)
            {
                return;
            }
            else
            {
                Repository.Delete(SelectedItem);
                await SaveRepository();
            }
        }

        private async Task SaveRepository()
            => await Repository.SaveAsync();
    }
}
