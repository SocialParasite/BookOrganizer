using BookOrganizer.Data.Lookups;
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
    public class NationalityDetailViewModel : BaseDetailViewModel<Nationality>
    {
        private readonly INationalityLookupDataService nationalityLookupService;
        private string name;

        public NationalityDetailViewModel(IEventAggregator eventAggregator,
            IMetroDialogService metroDialogService,
            IRepository<Nationality> nationalityRepository,
            INationalityLookupDataService nationalityLookupService)
            : base(eventAggregator, metroDialogService)
        {
            Repository = nationalityRepository;
            this.nationalityLookupService = nationalityLookupService ?? throw new ArgumentNullException(nameof(nationalityLookupService));

            ChangeEditedNationCommand = new DelegateCommand<Guid?>(OnChangeEditedNationExecute);

            SelectedItem = new Nationality();

            UserMode = (!UserMode.Item1, DetailViewState.EditMode, Brushes.LightGray, !UserMode.Item4).ToTuple();

            Nations = new ObservableCollection<LookupItem>();
        }

        public ICommand ChangeEditedNationCommand { get; }

        [Required]
        [MinLength(1, ErrorMessage = "Nations name should be at minimum 1 character long.")]
        [MaxLength(32, ErrorMessage = "Nations name should be maximum of 32 characters long.")]
        public string Name
        {
            get => name;
            set
            {
                ValidatePropertyInternal(nameof(Name), value);
                name = value;
                OnPropertyChanged();
                TabTitle = value;
                ((DelegateCommand)SaveItemCommand).RaiseCanExecuteChanged();
                SelectedItem.Name = value;
            }
        }

        public ObservableCollection<LookupItem> Nations { get; set; }

        public async override Task LoadAsync(Guid id)
        {
            SelectedItem = await Repository.GetSelectedAsync(id) ?? new Nationality();

            Id = id;

            if (Id != default)
            {
                Name = SelectedItem.Name;

                TabTitle = Name;
            }

            await InitializeLanguageCollection();

            SelectedItem.PropertyChanged += (s, e) =>
            {
                SetChangeTracker();
            };

            async Task InitializeLanguageCollection()
            {
                if (!Nations.Any())
                {
                    Nations.Clear();

                    foreach (var item in await GetListOfNations())
                    {
                        Nations.Add(item);
                    }
                }
            }
        }

        private async Task<IEnumerable<LookupItem>> GetListOfNations()
            => await nationalityLookupService.GetNationalityLookupAsync(nameof(NationalityDetailViewModel));

        private async void OnChangeEditedNationExecute(Guid? nationalityId)
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

            await LoadAsync((Guid)nationalityId);
        }

    }
}
