using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Events;
using BookOrganizer.UI.WPF.Repositories;
using Prism.Commands;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public abstract class BaseDetailViewModel<T> : ViewModelBase, IDetailViewModel
        where T : class, IIdentifiable
    {
        private readonly IEventAggregator eventAggregator;
        protected IRepository<T> Repository;

        private Guid guid;
        private T selectedItem;
        private Tuple<bool, int, SolidColorBrush, bool> userMode;
        private bool hasChanges;

        public BaseDetailViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));

            SwitchEditableStateCommand = new DelegateCommand(SwitchEditableStateExecute);
            SaveItemCommand = new DelegateCommand(SaveItemExecute);
            DeleteItemCommand = new DelegateCommand(DeleteItemExecute);
            CloseDetailViewCommand = new DelegateCommand(CloseDetailViewExecute);

            UserMode = (true, 0, Brushes.LightGray, false).ToTuple();
        }

        private void CloseDetailViewExecute()
        {
            if (this.Repository.HasChanges())
            {
                // TODO
            }

            eventAggregator.GetEvent<CloseDetailsViewEvent>()
                .Publish(new CloseDetailsViewEventArgs
                {
                    Id = this.Id,
                    ViewModelName = this.GetType().Name
                });
        }

        public ICommand SwitchEditableStateCommand { get; set; }
        public ICommand SaveItemCommand { get; set; }
        public ICommand DeleteItemCommand { get; set; }
        public ICommand CloseDetailViewCommand { get; set; }

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
            get { return hasChanges; }
            set
            {
                if (hasChanges != value)
                {
                    hasChanges = value;
                    OnPropertyChanged();
                }
            }
        }
        public abstract Task LoadAsync(Guid id);

        public virtual async void OnOpenItemDetailsViewAsync(Guid id)
            => SelectedItem = await Repository.GetSelectedAsync(id);

        public virtual void SwitchEditableStateExecute()
        {
            if (UserMode.Item2 == 0)
                UserMode = (!UserMode.Item1, 1, Brushes.LightGreen, !UserMode.Item4).ToTuple();
            else
                UserMode = (!UserMode.Item1, 0, Brushes.LightGray, !UserMode.Item4).ToTuple();
        }

        private async void SaveItemExecute()
        {
            Repository.Update(SelectedItem);
            await SaveRepository();
        }

        private async void DeleteItemExecute()
        {
            Repository.Delete(SelectedItem);
            await SaveRepository();
        }

        private async Task SaveRepository()
            => await Repository.SaveAsync();
    }
}
