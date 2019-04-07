using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Lookups;
using BookOrganizer.UI.WPF.Repositories;
using BookOrganizer.UI.WPF.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class SeriesDetailViewModel : BaseDetailViewModel<Series>, ISeriesDetailViewModel
    {
        private readonly IBookLookupDataService bookLookupDataService;
        private string name;

        public SeriesDetailViewModel(IEventAggregator eventAggregator,
            IMetroDialogService metroDialogService,
            IRepository<Series> seriesRepo,
            IBookLookupDataService bookLookupDataService)
            : base(eventAggregator, metroDialogService)
        {
            Repository = seriesRepo ?? throw new ArgumentNullException(nameof(seriesRepo));

            AddSeriesPictureCommand = new DelegateCommand(OnAddSeriesPictureExecute);
            FilterBookListCommand = new DelegateCommand<string>(OnFilterBookListExecute);

            this.bookLookupDataService = bookLookupDataService;

            Books = new ObservableCollection<LookupItem>();
            AllBooks = new ObservableCollection<LookupItem>();

            SelectedItem = new Series();
        }

        private async void OnFilterBookListExecute(string filter)
        {
            if (filter != string.Empty && filter != null)
            {
                var testCollection = AllBooks.Where(item => !SelectedItem.BooksInSeries
                               .Any(x => x.Id == item.Id))
                               .Where(item => item.DisplayMember.IndexOf(filter, StringComparison.OrdinalIgnoreCase) != -1)
                               .OrderBy(b => b.DisplayMember);

                Books.Clear();
                foreach (var item in testCollection)
                {
                    Books.Add(item);
                }
            }
        }

        public ICommand AddSeriesPictureCommand { get; }
        public ICommand FilterBookListCommand { get; set; }

        public ObservableCollection<LookupItem> Books { get; set; }
        public ObservableCollection<LookupItem> AllBooks { get; set; }

        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); TabTitle = value; SelectedItem.Name = value; }
        }

        public async override Task LoadAsync(Guid id)
        {
            SelectedItem = await Repository.GetSelectedAsync(id) ?? null;

            Id = id;

            if (Id != Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                TabTitle = SelectedItem.Name;
                Name = SelectedItem.Name;
            }
            else
                this.SwitchEditableStateExecute();

            await PopulateAllBooksCollection();
            SetDefaultSeriesPictureIfNoneSet();

            void SetDefaultSeriesPictureIfNoneSet()
            {
                if (SelectedItem.PicturePath is null)
                    SelectedItem.PicturePath = FileExplorerService.GetImagePath();
            }

            async Task PopulateAllBooksCollection()
            {
                foreach (var item in await GetBookList())
                {
                    AllBooks.Add(item);
                }

            }
        }

        public async override void SwitchEditableStateExecute()
        {
            base.SwitchEditableStateExecute();

            await InitializeBookCollection();

            async Task InitializeBookCollection()
            {
                if (!Books.Any())
                {

                    //var tempBookCollection = await GetBookList();
                    //tempBookCollection = tempBookCollection.Where(item => !SelectedItem.BooksInSeries
                    //           .Any(x => x.Id == item.Id))
                    //           .OrderBy(b => b.DisplayMember);

                    var tempBookCollection = AllBooks.Where(item => !SelectedItem.BooksInSeries
                               .Any(x => x.Id == item.Id))
                               .OrderBy(b => b.DisplayMember);

                    Books.Clear();
                    foreach (var item in tempBookCollection)
                    {
                        Books.Add(item);
                    }
                }
            }
        }

        private async Task<IEnumerable<LookupItem>> GetBookList()
            => await bookLookupDataService.GetBookLookupAsync();

        private async void OnAddSeriesPictureExecute()
        {
            SelectedItem.PicturePath = FileExplorerService.BrowsePicture() ?? SelectedItem.PicturePath;
            await LoadAsync(this.Id);
        }
    }
}
