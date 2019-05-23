using BookOrganizer.UI.WPF.Events;
using Prism.Commands;
using Prism.Events;
using System;
using System.Windows.Input;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class MainPageViewModel : IMainPageViewModel
    {
        private readonly IEventAggregator eventAggregator;

        public MainPageViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));

            AddNewItemCommand = new DelegateCommand<Type>(OnAddNewItemExecute);
            EditLanguagesCommand = new DelegateCommand(OnEditLanguagesExecute);
        }


        // Taken from BaseViewModel. Repeat or create a new base for just this?
        public ICommand AddNewItemCommand { get; }
        public ICommand EditLanguagesCommand { get; }

        private void OnAddNewItemExecute(Type itemType)
        {
            eventAggregator.GetEvent<OpenDetailViewEvent>()
                       .Publish(new OpenDetailViewEventArgs
                       {
                           Id = new Guid(),
                           ViewModelName = Type.GetType(itemType.FullName).Name
                       });
        }

        private void OnEditLanguagesExecute()
        {
            throw new NotImplementedException();
        }

    }
}
