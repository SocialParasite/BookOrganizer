using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BookOrganizer.Domain;
using BookOrganizer.Domain.Services;
using BookOrganizer.UI.WPFCore.Wrappers;
using Prism.Commands;
using Prism.Events;
using Serilog;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public class GenreDetailViewModel : BaseDetailViewModel<Genre, GenreWrapper>
    {
        private GenreWrapper selectedItem;
        private readonly IGenreLookupDataService genreLookupDataService;

        public GenreDetailViewModel(IEventAggregator eventAggregator,
            ILogger logger,
            IDomainService<Genre> domainService,
            IGenreLookupDataService genreLookupDataService)
            : base(eventAggregator, logger, domainService)
        {
            this.genreLookupDataService = genreLookupDataService ?? throw new ArgumentNullException(nameof(genreLookupDataService));

            ChangeEditedGenreCommand = new DelegateCommand<Guid?>(OnChangeEditedGenreExecute);

            SelectedItem = CreateWrapper(domainService.CreateItem());

            Genres = new ObservableCollection<LookupItem>();
        }

        public ICommand ChangeEditedGenreCommand { get; }

        public override GenreWrapper SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value ?? throw new ArgumentNullException(nameof(SelectedItem));
                OnPropertyChanged();
            }
        }

        public ObservableCollection<LookupItem> Genres { get; set; }

        public async override Task LoadAsync(Guid id)
        {
            var format = await domainService.Repository.GetSelectedAsync(id) ?? new Genre();

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
                if (e.PropertyName == nameof(SelectedItem.Name)
                    || e.PropertyName == nameof(SelectedItem.Name))
                {
                    TabTitle = SelectedItem.Name;
                }
            };
            ((DelegateCommand)SaveItemCommand).RaiseCanExecuteChanged();

            Id = id;

            if (Id != default)
            {
                TabTitle = SelectedItem.Name;
            }
            else
            {
                this.SwitchEditableStateExecute();
                SelectedItem.Name = SelectedItem.Model.Name;
            }

            await InitializeFormatCollection();

            async Task InitializeFormatCollection()
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
            => await genreLookupDataService.GetGenreLookupAsync(nameof(GenreDetailViewModel));

        private async void OnChangeEditedGenreExecute(Guid? genreId)
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

            await LoadAsync((Guid)genreId);
        }

        public override GenreWrapper CreateWrapper(Genre entity)
        {
            var wrapper = new GenreWrapper(entity);
            return wrapper;
        }
    }
}
