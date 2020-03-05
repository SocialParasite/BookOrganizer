using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using BookOrganizer.Domain;
using BookOrganizer.Domain.Services;
using BookOrganizer.UI.WPFCore.Wrappers;
using Prism.Commands;
using Prism.Events;
using Serilog;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public class LanguageDetailViewModel : BaseDetailViewModel<Language, LanguageWrapper>
    {
        private LanguageWrapper selectedItem;
        private readonly ILanguageLookupDataService languageLookupDataService;

        public LanguageDetailViewModel(IEventAggregator eventAggregator,
            ILogger logger,
            IDomainService<Language> domainService,
            ILanguageLookupDataService languageLookupDataService)
            : base(eventAggregator, logger, domainService)
        {
            this.languageLookupDataService = languageLookupDataService ?? throw new ArgumentNullException(nameof(languageLookupDataService));

            ChangeEditedLanguageCommand = new DelegateCommand<Guid?>(OnChangeEditedLanguageExecute);
            SaveItemCommand = new DelegateCommand(base.SaveItemExecute, base.SaveItemCanExecute)
                .ObservesProperty(() => SelectedItem.LanguageName);

            SelectedItem = CreateWrapper(domainService.CreateItem());

            Languages = new ObservableCollection<LookupItem>();

            UserMode = (!UserMode.Item1, DetailViewState.EditMode, Brushes.LightGray, !UserMode.Item4).ToTuple();
        }

        public ICommand ChangeEditedLanguageCommand { get; }

        public override LanguageWrapper SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value ?? throw new ArgumentNullException(nameof(SelectedItem));
                OnPropertyChanged();
            }
        }

        public ObservableCollection<LookupItem> Languages { get; set; }

        public async override Task LoadAsync(Guid id)
        {
            var format = await domainService.Repository.GetSelectedAsync(id) ?? new Language();

            SelectedItem = CreateWrapper(format);

            SelectedItem.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = domainService.Repository.HasChanges();
                }
                if (e.PropertyName == nameof(SelectedItem.HasErrors))
                {
                    ((DelegateCommand)SaveItemCommand).RaiseCanExecuteChanged();
                }
                if (e.PropertyName == nameof(SelectedItem.LanguageName)
                    || e.PropertyName == nameof(SelectedItem.LanguageName))
                {
                    TabTitle = SelectedItem.LanguageName;
                }
            };
            ((DelegateCommand)SaveItemCommand).RaiseCanExecuteChanged();

            Id = id;

            if (Id != default)
            {
                TabTitle = SelectedItem.LanguageName;
            }
            else
            {
                SelectedItem.LanguageName = SelectedItem.Model.LanguageName;
            }

            await InitializeFormatCollection();

            async Task InitializeFormatCollection()
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
            => await languageLookupDataService.GetLanguageLookupAsync(nameof(GenreDetailViewModel));

        private async void OnChangeEditedLanguageExecute(Guid? languageId)
        {
            if (this.domainService.Repository.HasChanges())
            {
                //var result = await metroDialogService
                //   .ShowOkCancelDialogAsync(
                //   "You have made changes. Changing editable format will loose all unsaved changes. Are you sure you still want to switch?",
                //   "Close the view?");

                //if (result == MessageDialogResult.Canceled)
                //{
                //    return;
                //}
            }

            domainService.Repository.ResetTracking(SelectedItem.Model);
            HasChanges = domainService.Repository.HasChanges();

            await LoadAsync((Guid)languageId);
        }

        public override LanguageWrapper CreateWrapper(Language entity)
        {
            var wrapper = new LanguageWrapper(entity);
            return wrapper;
        }
    }
}
