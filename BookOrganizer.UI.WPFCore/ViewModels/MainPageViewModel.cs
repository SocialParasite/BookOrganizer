﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using BookOrganizer.DA;
using BookOrganizer.UI.WPFCore.Events;
using Prism.Commands;
using Prism.Events;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public class MainPageViewModel : ISelectedViewModel
    {
        private readonly IEventAggregator eventAggregator;
        private readonly ILanguageLookupDataService languageLookup;
        private readonly INationalityLookupDataService nationalityLookupDataService;
        private readonly IFormatLookupDataService formatLookupDataService;
        private readonly IGenreLookupDataService genreLookupDataService;

        public MainPageViewModel(IEventAggregator eventAggregator, 
                                 ILanguageLookupDataService languageLookup,
                                 INationalityLookupDataService nationalityLookupDataService,
                                 IFormatLookupDataService formatLookupDataService,
                                 IGenreLookupDataService genreLookupDataService)
        {
            this.eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            this.languageLookup = languageLookup ?? throw new ArgumentNullException(nameof(languageLookup));
            this.nationalityLookupDataService = nationalityLookupDataService ?? throw new ArgumentNullException(nameof(nationalityLookupDataService));
            this.formatLookupDataService = formatLookupDataService ?? throw new ArgumentNullException(nameof(formatLookupDataService));
            this.genreLookupDataService = genreLookupDataService ?? throw new ArgumentNullException(nameof(genreLookupDataService));

            AddNewItemCommand = new DelegateCommand<Type>(OnAddNewItemExecute);
            EditLanguagesCommand = new DelegateCommand(OnEditLanguagesExecute);
            EditNationalitiesCommand = new DelegateCommand(OnEditNationalitiesExecute);
            EditBookFormatsCommand = new DelegateCommand(OnEditBookFormatsExecute);
            EditBookGenresCommand = new DelegateCommand(OnEditBookGenresExecute);
        }

        public ICommand AddNewItemCommand { get; }
        public ICommand EditLanguagesCommand { get; }
        public ICommand EditNationalitiesCommand { get; }
        public ICommand EditBookFormatsCommand { get; }
        public ICommand EditBookGenresCommand { get; }

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
                               ViewModelName = null //nameof(LanguageDetailViewModel)
                           });
        }
        private async void OnEditNationalitiesExecute()
        {
            eventAggregator.GetEvent<OpenDetailViewEvent>()
                        .Publish(new OpenDetailViewEventArgs
                        {
                            Id = await nationalityLookupDataService.GetNationalityId(),
                            ViewModelName = null //nameof(NationalityDetailViewModel)
                        });
        }

        private async void OnEditBookFormatsExecute()
        {
            eventAggregator.GetEvent<OpenDetailViewEvent>()
                           .Publish(new OpenDetailViewEventArgs
                           {
                               Id = await formatLookupDataService.GetFormatId(),
                               ViewModelName = null //nameof(FormatDetailViewModel)
                           });
        }

        private async void OnEditBookGenresExecute()
        {
            eventAggregator.GetEvent<OpenDetailViewEvent>()
                           .Publish(new OpenDetailViewEventArgs
                           {
                               Id = await genreLookupDataService.GetGenreId(),
                               ViewModelName = null //nameof(GenreDetailViewModel)
                           });

        }
    }
}
