using BookOrganizer.DA;
using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Enums;
using BookOrganizer.UI.WPF.Services;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class LanguageDetailViewModel : BaseDetailViewModel<Language>
    {
        private readonly ILanguageLookupDataService languageLookupService;
        private string languageName;

        public LanguageDetailViewModel(IEventAggregator eventAggregator,
            IMetroDialogService metroDialogService,
            IRepository<Language> languageRepository,
            ILanguageLookupDataService languageLookupService)
            : base(eventAggregator, metroDialogService)
        {
            Repository = languageRepository;
            this.languageLookupService = languageLookupService ?? throw new ArgumentNullException(nameof(languageLookupService));

            ChangeEditedLanguageCommand = new DelegateCommand<Guid?>(OnChangeEditedLanguageExecute);

            SelectedItem = new Language();

            UserMode = (!UserMode.Item1, DetailViewState.EditMode, Brushes.LightGray, !UserMode.Item4).ToTuple();

            Languages = new ObservableCollection<LookupItem>();
        }

        public ICommand ChangeEditedLanguageCommand { get; }

        [Required]
        [MinLength(1, ErrorMessage = "Language name should be at minimum 1 character long.")]
        [MaxLength(32, ErrorMessage = "Language name should be maximum of 32 characters long.")]
        public string LanguageName
        {
            get => languageName;
            set
            {
                ValidatePropertyInternal(nameof(LanguageName), value);
                languageName = value;
                OnPropertyChanged();
                TabTitle = value;
                ((DelegateCommand)SaveItemCommand).RaiseCanExecuteChanged();
                SelectedItem.LanguageName = value;
            }
        }

        public ObservableCollection<LookupItem> Languages { get; set; }

        public async override Task LoadAsync(Guid id)
        {
            SelectedItem = await Repository.GetSelectedAsync(id) ?? new Language();

            Id = id;

            if (Id != default)
            {
                LanguageName = SelectedItem.LanguageName;

                TabTitle = LanguageName;
            }

            await InitializeLanguageCollection();

            SelectedItem.PropertyChanged += (s, e) =>
            {
                SetChangeTracker();
            };

            async Task InitializeLanguageCollection()
            {
                if (!Languages.Any())
                {
                    Languages.Clear();

                    foreach (var item in await GetLanguageList())
                    {
                        Languages.Add(item);
                    }
                }
            }
        }

        private async Task<IEnumerable<LookupItem>> GetLanguageList()
            => await languageLookupService.GetLanguageLookupAsync(nameof(LanguageDetailViewModel));

        private async void OnChangeEditedLanguageExecute(Guid? languageId)
        {
            if (this.Repository.HasChanges())
            {
                var result = await metroDialogService
                   .ShowOkCancelDialogAsync(
                   "You have made changes. Changing editable language will loose all unsaved changes. Are you sure you still want to switch?",
                   "Close the view?");

                if (result == MessageDialogResult.Canceled)
                {
                    return;
                }
            }

            Repository.ResetTracking(SelectedItem);
            HasChanges = Repository.HasChanges();

            await LoadAsync((Guid)languageId);
        }
    }

}
