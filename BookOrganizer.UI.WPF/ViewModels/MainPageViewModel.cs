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
        private readonly INationalityLookupDataService nationalityLookupDataService;
        private readonly IFormatLookupDataService formatLookupDataService;

        public MainPageViewModel(IEventAggregator eventAggregator, ILanguageLookupDataService languageLookup,
            INationalityLookupDataService nationalityLookupDataService,
            IFormatLookupDataService formatLookupDataService)
        {
            this.eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            this.languageLookup = languageLookup ?? throw new ArgumentNullException(nameof(languageLookup));
            this.nationalityLookupDataService = nationalityLookupDataService ?? throw new ArgumentNullException(nameof(nationalityLookupDataService));
            this.formatLookupDataService = formatLookupDataService ?? throw new ArgumentNullException(nameof(formatLookupDataService));

            AddNewItemCommand = new DelegateCommand<Type>(OnAddNewItemExecute);
            EditLanguagesCommand = new DelegateCommand(OnEditLanguagesExecute);
            EditNationalitiesCommand = new DelegateCommand(OnEditNationalitiesExecute);
            EditBookFormatsCommand = new DelegateCommand(OnEditBookFormatsExecute);
        }

        // Taken from BaseViewModel. Repeat or create a new base for just this?
        public ICommand AddNewItemCommand { get; }
        public ICommand EditLanguagesCommand { get; }
        public ICommand EditNationalitiesCommand { get; }
        public ICommand EditBookFormatsCommand { get; }

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
        private async void OnEditNationalitiesExecute()
        {
            eventAggregator.GetEvent<OpenDetailViewEvent>()
                        .Publish(new OpenDetailViewEventArgs
                        {
                            Id = await nationalityLookupDataService.GetNationalityId(),
                            ViewModelName = nameof(NationalityDetailViewModel)
                        });
        }

        private async void OnEditBookFormatsExecute()
        {
            eventAggregator.GetEvent<OpenDetailViewEvent>()
                           .Publish(new OpenDetailViewEventArgs
                           {
                               Id = await formatLookupDataService.GetFormatId(),
                               ViewModelName = nameof(FormatDetailViewModel)
                           });
        }
    }
}
