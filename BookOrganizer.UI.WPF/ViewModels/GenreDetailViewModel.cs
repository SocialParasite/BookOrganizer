using BookOrganizer.Data.Lookups;
using BookOrganizer.Data.Repositories;
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
    public class GenreDetailViewModel : BaseDetailViewModel<Genre>
    {
        private readonly IGenreLookupDataService genreLookupService;
        private string name;

        public GenreDetailViewModel(IEventAggregator eventAggregator,
            IMetroDialogService metroDialogService,
            IRepository<Genre> genreRepository,
            IGenreLookupDataService genreLookupService)
            : base(eventAggregator, metroDialogService)
        {
            Repository = genreRepository;
            this.genreLookupService = genreLookupService ?? throw new ArgumentNullException(nameof(genreLookupService));

            ChangeEditedGenreCommand = new DelegateCommand<Guid?>(OnChangeEditedGenreExecute);

            SelectedItem = new Genre();

            UserMode = (!UserMode.Item1, DetailViewState.EditMode, Brushes.LightGray, !UserMode.Item4).ToTuple();

            Genres = new ObservableCollection<LookupItem>();
        }

        public ICommand ChangeEditedGenreCommand { get; }

        [Required]
        [MinLength(1, ErrorMessage = "Genre name should be at minimum 1 character long.")]
        [MaxLength(32, ErrorMessage = "Genre name should be maximum of 32 characters long.")]
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

        public ObservableCollection<LookupItem> Genres { get; set; }

        public async override Task LoadAsync(Guid id)
        {
            SelectedItem = await Repository.GetSelectedAsync(id) ?? new Genre();

            Id = id;

            if (Id != default)
            {
                Name = SelectedItem.Name;

                TabTitle = Name;
            }

            await InitializeGenreCollection();

            SelectedItem.PropertyChanged += (s, e) =>
            {
                SetChangeTracker();
            };

            async Task InitializeGenreCollection()
            {
                if (!Genres.Any())
                {
                    Genres.Clear();

                    foreach (var item in await GetGenreList())
                    {
                        Genres.Add(item);
                    }
                }
            }
        }

        private async Task<IEnumerable<LookupItem>> GetGenreList()
            => await genreLookupService.GetGenreLookupAsync(nameof(GenreDetailViewModel));

        private async void OnChangeEditedGenreExecute(Guid? genreId)
        {
            if (this.Repository.HasChanges())
            {
                var result = await metroDialogService
                   .ShowOkCancelDialogAsync(
                   "You have made changes. Changing editable genre will loose all unsaved changes. Are you sure you still want to switch?",
                   "Close the view?");

                if (result == MessageDialogResult.Canceled)
                {
                    return;
                }
            }

            Repository.ResetTracking(SelectedItem);
            HasChanges = Repository.HasChanges();

            await LoadAsync((Guid)genreId);
        }
    }
}
