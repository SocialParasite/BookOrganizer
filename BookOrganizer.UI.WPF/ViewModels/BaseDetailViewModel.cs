using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Enums;
using BookOrganizer.UI.WPF.Events;
using BookOrganizer.UI.WPF.Services;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public abstract class BaseDetailViewModel<T> : ViewModelBase, IDetailViewModel, INotifyDataErrorInfo
        where T : class, IIdentifiable
    {
        protected readonly IMetroDialogService metroDialogService;
        protected readonly IEventAggregator eventAggregator;
        protected IRepository<T> Repository;

        private Guid id;
        private T selectedItem;
        private Tuple<bool, DetailViewState, SolidColorBrush, bool> userMode;
        private bool hasChanges;
        private Guid selectedBookId;
        private string tabTitle;

        public BaseDetailViewModel(IEventAggregator eventAggregator, IMetroDialogService metroDialogService)
        {
            this.eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            this.metroDialogService = metroDialogService ?? throw new ArgumentNullException(nameof(metroDialogService));

            SwitchEditableStateCommand = new DelegateCommand(SwitchEditableStateExecute);
            SaveItemCommand = new DelegateCommand(SaveItemExecute, SaveItemCanExecute)
                .ObservesProperty(() => HasChanges)
                .ObservesProperty(() => HasErrors);
            DeleteItemCommand = new DelegateCommand(DeleteItemExecute);
            CloseDetailViewCommand = new DelegateCommand(CloseDetailViewExecute);
            ShowSelectedBookCommand = new DelegateCommand<Guid?>(OnShowSelectedBookExecute, OnShowSelectedBookCanExecute);

            UserMode = (true, DetailViewState.ViewMode, Brushes.LightGray, false).ToTuple();

            Errors = new Dictionary<string, List<string>>();
        }

        public ICommand SwitchEditableStateCommand { get; }
        public ICommand SaveItemCommand { get; }
        public ICommand DeleteItemCommand { get; }
        public ICommand CloseDetailViewCommand { get; }
        public ICommand ShowSelectedBookCommand { get; }

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
            get { return id; }
            set { id = value; }
        }

        public Tuple<bool, DetailViewState, SolidColorBrush, bool> UserMode
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

        public virtual string TabTitle
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

        protected void SetChangeTracker()
        {
            if (!HasChanges)
            {
                HasChanges = Repository.HasChanges();
            }
        }

        public abstract Task LoadAsync(Guid id);

        public virtual async Task OnOpenItemDetailsViewAsync(Guid id)
            => SelectedItem = await Repository.GetSelectedAsync(id);

        public virtual void SwitchEditableStateExecute()
        {
            if (UserMode.Item2 == DetailViewState.ViewMode)
                UserMode = (!UserMode.Item1, DetailViewState.EditMode, Brushes.LightGreen, !UserMode.Item4).ToTuple();
            else
                UserMode = (!UserMode.Item1, DetailViewState.ViewMode, Brushes.LightGray, !UserMode.Item4).ToTuple();
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

        public virtual void OnShowSelectedBookExecute(Guid? id)
            => SelectedBookId = (Guid)id;

        private bool SaveItemCanExecute()
            => (!HasErrors) && (HasChanges || SelectedItem.Id == default);

        private async void SaveItemExecute()
        {
            if (this.Repository.HasChanges() || SelectedItem.Id == default)
            {
                var isNewItem = false;

                var resultWhenChanges = await metroDialogService
                    .ShowOkCancelDialogAsync(
                    "You are about to save your changes. This will overwrite the previous version. Are you sure?",
                    "Save changes?");

                if (resultWhenChanges == MessageDialogResult.Canceled)
                {
                    return;
                }

                if (SelectedItem.Id == default)
                {
                    isNewItem = true;
                }

                Repository.Update(SelectedItem);
                await SaveRepository();

                if (isNewItem)
                {
                    eventAggregator.GetEvent<SavedDetailsViewEvent>()
                        .Publish(new OpenDetailViewEventArgs
                        {
                            Id = SelectedItem.Id,
                            ViewModelName = this.GetType().Name
                        });
                }

                HasChanges = false;
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

        public bool HasErrors => Errors.Any();


        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public Dictionary<string, List<string>> Errors { get; set; }
        public IEnumerable GetErrors(string propertyName)
        {
            return Errors.ContainsKey(propertyName) ? Errors[propertyName] : null;
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void AddError(string propertyName, string error)
        {
            if (!Errors.ContainsKey(propertyName))
            {
                Errors[propertyName] = new List<string>();
            }
            if (!Errors[propertyName].Contains(error))
            {
                Errors[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        private void ClearErrors(string propertyName)
        {
            if (Errors.ContainsKey(propertyName))
            {
                Errors.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }

        public dynamic ValidateDataAnnotations(string propertyName, object currentValue)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(this) { MemberName = propertyName };
            Validator.TryValidateProperty(currentValue, context, results);

            foreach (var result in results)
            {
                AddError(propertyName, result.ErrorMessage);
            }

            switch (Type.GetTypeCode(GetType().GetProperty(propertyName).PropertyType))
            {
                case TypeCode.Int32:
                    return (int)currentValue;
                case TypeCode.String:
                    return (string)currentValue;
                default:
                    return currentValue;
            }
        }

        public void ValidatePropertyInternal(string propertyName, object currentValue)
        {
            ClearErrors(propertyName);

            ValidateDataAnnotations(propertyName, currentValue);

            ValidateCustomErrors(propertyName);
        }

        public void ValidateCustomErrors(string propertyName)
        {
            var errors = ValidateProperty(propertyName);
            if (errors != null)
            {
                foreach (var error in errors)
                {
                    AddError(propertyName, error);
                }
            }
        }

        protected virtual IEnumerable<string> ValidateProperty(string propertyName)
        {
            return null;
        }

    }
}
