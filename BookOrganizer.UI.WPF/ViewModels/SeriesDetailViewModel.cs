using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Lookups;
using BookOrganizer.UI.WPF.Repositories;
using BookOrganizer.UI.WPF.Services;
using GongSolutions.Wpf.DragDrop;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class SeriesDetailViewModel : BaseDetailViewModel<Series>, ISeriesDetailViewModel, IDropTarget
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
            AddBookToSeriesCommand = new DelegateCommand<Guid?>(OnAddBookToSeriesExecute, OnAddBookToSeriesCanExecute);

            this.bookLookupDataService = bookLookupDataService;

            Books = new ObservableCollection<LookupItem>();
            AllBooks = new ObservableCollection<LookupItem>();

            SelectedItem = new Series();
        }

        public ICommand AddSeriesPictureCommand { get; }
        public ICommand FilterBookListCommand { get; }
        public ICommand AddBookToSeriesCommand { get; }

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

        public override void SwitchEditableStateExecute()
        {
            base.SwitchEditableStateExecute();

            InitializeBookCollection();

            void InitializeBookCollection()
            {
                if (!Books.Any())
                {
                    var tempBookCollection = AllBooks.Where(item => !SelectedItem.BooksInSeries
                                                     .Any(x => x.Id == item.Id))
                                                     .OrderBy(b => b.DisplayMember);

                    PopulateBooksCollection(tempBookCollection);
                }
            }
        }

        public override void OnShowSelectedBookExecute(Guid? id)
        {
            if (UserMode.Item2 == Enums.DetailViewState.ViewMode)
            {
                base.OnShowSelectedBookExecute(id);
            }
            else
            {
                var book = SelectedItem.BooksInSeries.First(b => b.Id == id);
                SelectedItem.BooksInSeries.Remove(book);

                var sro = SelectedItem.SeriesReadOrder.First(b => b.BookId == id);
                SelectedItem.SeriesReadOrder.Remove(sro);

                Books.Add(new LookupItem { Id = book.Id, DisplayMember = book.Title, Picture = book.BookCoverPicturePath });

                var countOfBooksInSeries = SelectedItem.SeriesReadOrder.Count();
                SelectedItem.NumberOfBooks = countOfBooksInSeries;

                var inst = sro.Instalment;
                if (inst < ++countOfBooksInSeries)
                {
                    foreach (var item in SelectedItem.SeriesReadOrder)
                    {
                        if (item.Instalment > inst)
                            item.Instalment--;
                    }
                }
            }

        }

        private bool OnAddBookToSeriesCanExecute(Guid? id)
            => (id is null || id == Guid.Empty) ? false : true;

        private async void OnAddBookToSeriesExecute(Guid? id)
        {
            var book = await bookLookupDataService.GetBookById((Guid)id);
            SelectedItem.BooksInSeries.Add(book);

            var sro = new SeriesReadOrder
            {
                BookId = (Guid)id,
                Book = book,
                Series = SelectedItem,
                SeriesId = SelectedItem.Id,
                Instalment = SelectedItem.SeriesReadOrder.Count() + 1
            };

            SelectedItem.SeriesReadOrder.Add(sro);
            Books.Remove(Books.First(b => b.Id == id));

            SelectedItem.NumberOfBooks = SelectedItem.SeriesReadOrder.Count();
        }

        private void OnFilterBookListExecute(string filter)
        {
            if (filter != string.Empty && filter != null)
            {
                var filteredCollection = AllBooks.Where(item => !SelectedItem.BooksInSeries
                                                 .Any(x => x.Id == item.Id))
                                                 .Where(item => item.DisplayMember.IndexOf(filter, StringComparison.OrdinalIgnoreCase) != -1)
                                                 .OrderBy(b => b.DisplayMember);

                PopulateBooksCollection(filteredCollection);
            }
            else
            {
                var allExcludingBooksInSeries = AllBooks.Where(item => !SelectedItem.BooksInSeries
                                                        .Any(x => x.Id == item.Id))
                                                        .OrderBy(b => b.DisplayMember);

                PopulateBooksCollection(allExcludingBooksInSeries);
            }
        }

        private void PopulateBooksCollection(IOrderedEnumerable<LookupItem> tempBookCollection)
        {
            Books.Clear();
            foreach (var item in tempBookCollection)
            {
                Books.Add(item);
            }
        }

        private async Task<IEnumerable<LookupItem>> GetBookList()
            => await bookLookupDataService.GetBookLookupAsync();

        private async void OnAddSeriesPictureExecute()
        {
            SelectedItem.PicturePath = FileExplorerService.BrowsePicture() ?? SelectedItem.PicturePath;
            await LoadAsync(this.Id);
        }

        public void DragOver(IDropInfo dropInfo)
        {
            SeriesReadOrder sourceItem = dropInfo.Data as SeriesReadOrder;
            SeriesReadOrder targetItem = dropInfo.TargetItem as SeriesReadOrder;

            if (sourceItem != null && targetItem != null)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            SeriesReadOrder sourceItem = dropInfo.Data as SeriesReadOrder;
            SeriesReadOrder targetItem = dropInfo.TargetItem as SeriesReadOrder;

            var originalSourceInstalment = sourceItem.Instalment;
            var originalTargetInstalment = targetItem.Instalment;


            if (originalTargetInstalment == (originalSourceInstalment + 1)
                || originalTargetInstalment == (originalSourceInstalment - 1))
            {
                originalSourceInstalment = sourceItem.Instalment;

                targetItem.Instalment = originalSourceInstalment;
                sourceItem.Instalment = originalTargetInstalment;

                SelectedItem.SeriesReadOrder.Remove(sourceItem);
                SelectedItem.SeriesReadOrder.Add(sourceItem);
                SelectedItem.SeriesReadOrder.Remove(targetItem);
                SelectedItem.SeriesReadOrder.Add(targetItem);
            }
            else
            {
                sourceItem.Instalment = targetItem.Instalment;
                SelectedItem.SeriesReadOrder.Remove(sourceItem);

                foreach (var item in SelectedItem.SeriesReadOrder)
                {
                    if (item.Instalment > originalSourceInstalment && item.Instalment <= sourceItem.Instalment)
                    {
                        item.Instalment--;
                    }
                }

                SelectedItem.SeriesReadOrder.Add(sourceItem);

                var temporaryReadOrderCollection = new ObservableCollection<SeriesReadOrder>();
                temporaryReadOrderCollection.AddRange(SelectedItem.SeriesReadOrder.OrderBy(a => a.Instalment));
                SelectedItem.SeriesReadOrder.Clear();

                foreach (var item in temporaryReadOrderCollection)
                {
                    SelectedItem.SeriesReadOrder.Add(item);
                }
            }
        }
    }
}
