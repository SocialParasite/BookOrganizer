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
    public class FormatDetailViewModel : BaseDetailViewModel<Format>
    {
        private readonly IFormatLookupDataService formatLookupService;
        private string name;

        public FormatDetailViewModel(IEventAggregator eventAggregator,
            IMetroDialogService metroDialogService,
            IRepository<Format> formatRepository,
            IFormatLookupDataService formatLookupService)
            : base(eventAggregator, metroDialogService)
        {
            Repository = formatRepository;
            this.formatLookupService = formatLookupService ?? throw new ArgumentNullException(nameof(formatLookupService));

            ChangeEditedFormatCommand = new DelegateCommand<Guid?>(OnChangeEditedFormatExecute);

            SelectedItem = new Format();

            UserMode = (!UserMode.Item1, DetailViewState.EditMode, Brushes.LightGray, !UserMode.Item4).ToTuple();

            Formats = new ObservableCollection<LookupItem>();
        }

        public ICommand ChangeEditedFormatCommand { get; }

        [Required]
        [MinLength(1, ErrorMessage = "Format name should be at minimum 1 character long.")]
        [MaxLength(32, ErrorMessage = "Format name should be maximum of 32 characters long.")]
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

        public ObservableCollection<LookupItem> Formats { get; set; }

        public async override Task LoadAsync(Guid id)
        {
            SelectedItem = await Repository.GetSelectedAsync(id) ?? new Format();

            Id = id;

            if (Id != default)
            {
                Name = SelectedItem.Name;

                TabTitle = Name;
            }

            await InitializeFormatCollection();

            SelectedItem.PropertyChanged += (s, e) =>
            {
                SetChangeTracker();
            };

            async Task InitializeFormatCollection()
            {
                if (!Formats.Any())
                {
                    Formats.Clear();

                    foreach (var item in await GetFormatList())
                    {
                        Formats.Add(item);
                    }
                }
            }
        }

        private async Task<IEnumerable<LookupItem>> GetFormatList()
            => await formatLookupService.GetFormatLookupAsync(nameof(FormatDetailViewModel));

        private async void OnChangeEditedFormatExecute(Guid? formatId)
        {
            if (this.Repository.HasChanges())
            {
                var result = await metroDialogService
                   .ShowOkCancelDialogAsync(
                   "You have made changes. Changing editable format will loose all unsaved changes. Are you sure you still want to switch?",
                   "Close the view?");

                if (result == MessageDialogResult.Canceled)
                {
                    return;
                }
            }

            Repository.ResetTracking(SelectedItem);
            HasChanges = Repository.HasChanges();

            await LoadAsync((Guid)formatId);
        }
    }
}
