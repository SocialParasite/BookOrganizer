using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Repositories;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private List<T> itemCollection;
        private T selectedItem;
        private Tuple<bool, int, SolidColorBrush, bool> userMode;

        public BaseDetailViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));

            SwitchEditableStateCommand = new DelegateCommand(SwitchEditableStateExecute);
            UpdateItemCommand = new DelegateCommand(UpdateItemExecute);

            UserMode = (true, 0, Brushes.LightGray, false).ToTuple();
        }

        public ICommand SwitchEditableStateCommand { get; set; }
        public ICommand UpdateItemCommand { get; set; }

        public List<T> ItemCollection
        {
            get { return itemCollection; }
            set { itemCollection = value; OnPropertyChanged(); }
        }

        public T SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; OnPropertyChanged(); }
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

        public abstract Task LoadAsync(Guid id);

        public virtual async void OnOpenItemDetailsViewAsync(Guid id)
            => SelectedItem = await Repository.GetSelectedAsync(id);

        private void SwitchEditableStateExecute()
        {
            if (UserMode.Item2 == 0)
                UserMode = (!UserMode.Item1, 1, Brushes.LightGreen, !UserMode.Item4).ToTuple();
            else
                UserMode = (!UserMode.Item1, 0, Brushes.LightGray, !UserMode.Item4).ToTuple();

        }

        private async void UpdateItemExecute()
        {
            Repository.Update(SelectedItem);
            await Repository.SaveAsync();
        }

    }
}
