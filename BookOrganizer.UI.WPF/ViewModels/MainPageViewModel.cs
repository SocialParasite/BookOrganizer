using BookOrganizer.UI.WPF.Events;
using BookOrganizer.UI.WPF.Lookups;
using Prism.Commands;
using Prism.Events;
using System;
using System.Windows.Input;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class MainPageViewModel: ISelectedViewModel
    {
        private readonly IEventAggregator eventAggregator;
        private readonly ILanguageLookupDataService languageLookup;

        public MainPageViewModel(IEventAggregator eventAggregator, ILanguageLookupDataService languageLookup)
        {
            this.eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            this.languageLookup = languageLookup ?? throw new ArgumentNullException(nameof(languageLookup));

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

        private async void OnEditLanguagesExecute()
        {
            eventAggregator.GetEvent<OpenDetailViewEvent>()
                           .Publish(new OpenDetailViewEventArgs
                           {
                               Id = await languageLookup.GetLanguageId(),
                               ViewModelName = nameof(LanguageDetailViewModel)
                           });
        }

    }
}
